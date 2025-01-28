using System;
using System.Collections;
using System.Numerics;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class SmoothCameraRotation : MonoBehaviour
{   
    //Master管理用
    private int rank = 2;
    private Master_Script Master;

    //現在写っているHubの3Dデータクローンの配列を参照
    private Hub_Manager hub_Manager;

    //Info_Master
    private Info_Master info_Master;

    //Update関数管理
    private bool isCanUpdate;

    [Header("ターゲット設定")]
    private Transform target; // カメラが回転する中心となるオブジェクト（Field）

    [Header("カメラ設定")]
    public float Targetdistance = 5f; // ターゲットとの直線距離
    [UnityEngine.Range(-89f, 89f)] public float verticalAngle = 30f; // 垂直方向の角度 (X軸回転)

    [Header("回転設定")]
    public float rotationSpeed = 200f; // マウス入力に対する回転速度の感度
    public float damping = 5f; // 減衰速度
    public float accelerationSmoothTime = 0.2f; // 初速のスムーズさ

    private float rotationVelocity = 0f; // 現在の回転速度
    private float targetRotationVelocity = 0f; // 目標の回転速度
    private UnityEngine.Vector3 previousMousePosition;
    private float smoothVelocity = 0f; // スムーズな加速度の補間値
    [SerializeField] private bool isDragging = false;
    public SerachWindowController searchWindowController;
    public Menu_Controller Menu_Controller;
    public bool isDragTimeOut = false;
    private bool isFlag = false;
    private GameObject Point_Search;
    private GameObject Floa_3D_Deta;
    private float MoveDistance;
    private float prespectiveDistance;
    private float MoveAnimationDulation = 0.6f;
    [Header("初期階層")]
    public int Floor;

    private UnityEngine.Vector3 currentVelocity = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 currentVelocityPrespective = UnityEngine.Vector3.zero; // PrespectiveObjectのための補間用
    public GameObject PrespectiveObject;

    //statusのHubと、Floorの変更用のスクリプトをロード
    private StatusAnimationController statusAnimationController;
    //フロアのみの変更のためのLocationsList型のものを作るための関数
    private Function function;


    private void OnEnable(){
        Master = GameObject.Find("Master").GetComponent<Master_Script>();
        stopCameraTracking();

        info_Master = GameObject.Find("Master").GetComponent<Info_Master>();
        function = GameObject.Find("Master").GetComponent<Function>();

        hub_Manager = GameObject.Find("3D_Model_Master").GetComponent<Hub_Manager>();

        Menu_Controller = GameObject.Find("Search_UI").GetComponent<Menu_Controller>();
        Point_Search = GameObject.Find("Point_Search");

        statusAnimationController = GameObject.Find("Status_UI").GetComponent<StatusAnimationController>();

        Master.Initialize_Reaction[this.rank]++;
    }
    IEnumerator Start()
    {   
        while(!Master.isInitialized[rank]){
            yield return null;
        }
        Floor = info_Master.location.Floor;//一回のデータは0番目

        Floa_3D_Deta = hub_Manager.Current_Hub_3D[0];
        PrespectiveObject = Instantiate(PrespectiveObject,Floa_3D_Deta.transform.position,UnityEngine.Quaternion.identity);
        
        target = PrespectiveObject.transform;
        if (target == null)
        {
            Debug.LogError("ターゲットが設定されていません。");
            yield break;
        }
        SetCameraToTarget();

        Master.Initialize_Reaction[this.rank]--;
        isCanUpdate = true;
    }

    void Update()
    {   
        if(isCanUpdate){
            if (target == null)
            {
                Debug.LogError("ターゲットが設定されていません。");
                return;
            }
            if (Menu_Controller.isFlag != isFlag)
            {
                isDragTimeOut = true;
                isFlag = !isFlag;
            }
            //センターにカメラを戻すときのアップデート処理
            if(isDragTimeOut == true){
                MoveToAnother();
                if(MoveDistance<=0.1&&prespectiveDistance<=5){
                    isDragTimeOut = false;
                }
            }
            if(isDragTimeOut != true||isDragTimeOut==true){
                HandleInput();
                ApplyRotation();
            }
            if(UnityEngine.Vector3.Distance(Floa_3D_Deta.transform.position,PrespectiveObject.transform.position) >= 5){
                isDragTimeOut = true;
            }
            if(Input.GetKeyDown(KeyCode.Space)){
                Floor++;
                ChangeLocation(null,Floor);
            }
        }
    }

    /// <summary>
    /// ユーザー入力の処理
    /// </summary>
    private void HandleInput()
    {
        if (!searchWindowController.isDragging)
        {
            // 入力開始
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                previousMousePosition = Input.mousePosition;
            }

            // 入力中
            if (Input.GetMouseButton(0))
            {
                UnityEngine.Vector3 mouseDelta = Input.mousePosition - previousMousePosition;

                // マウス移動速度（目標速度）の計算
                targetRotationVelocity = (mouseDelta.x / Time.deltaTime) * rotationSpeed * Time.deltaTime;

                // スムーズに現在の速度へ移行
                rotationVelocity = Mathf.SmoothDamp(
                    rotationVelocity, targetRotationVelocity, ref smoothVelocity, accelerationSmoothTime
                );

                previousMousePosition = Input.mousePosition;
            }
            else
            {
                // マウスを離したとき
                if (isDragging)
                {
                    isDragging = false;
                }

                // 回転速度を減速
                rotationVelocity = Mathf.Lerp(rotationVelocity, 0, damping * Time.deltaTime);

                if (Mathf.Abs(rotationVelocity) < 0.01f)
                {
                    rotationVelocity = 0f;
                }
            }
        }
    }

    /// <summary>
    /// カメラの回転を適用
    /// </summary>
    private void ApplyRotation()
    {
        if (rotationVelocity != 0f)
        {
            // カメラをターゲットの周りで回転
            transform.RotateAround(target.position, UnityEngine.Vector3.up, rotationVelocity);
        }
    }

    /// <summary>
    /// カメラの位置と向きをターゲットに合わせる
    /// </summary>
    private void SetCameraToTarget()
    {
        // カメラの角度を計算
        UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler(verticalAngle, 0, 0);

        // カメラの位置をターゲットから一定の距離離す
        UnityEngine.Vector3 offset = rotation * new UnityEngine.Vector3(0, 0, -Targetdistance);

        // カメラの位置と向きを設定
        transform.position = target.position + offset;
        transform.LookAt(target);
    }

    /// <summary>
    /// フラグが変わった時にターゲットを切り替えて移動
    /// </summary>
    private void MoveToAnother()
    {
        if(isFlag == true){
            PrespectiveObject.transform.position = UnityEngine.Vector3.SmoothDamp(PrespectiveObject.transform.position,Point_Search.transform.position, ref currentVelocityPrespective,MoveAnimationDulation);
            MoveDistance = UnityEngine.Vector3.Distance(PrespectiveObject.transform.position,Point_Search.transform.position);
        }
        else{
            PrespectiveObject.transform.position = UnityEngine.Vector3.SmoothDamp(PrespectiveObject.transform.position,Floa_3D_Deta.transform.position, ref currentVelocityPrespective,MoveAnimationDulation);
            MoveDistance = UnityEngine.Vector3.Distance(PrespectiveObject.transform.position,Floa_3D_Deta.transform.position);
        }

        UnityEngine.Vector3 offset = new UnityEngine.Vector3(-Targetdistance * Mathf.Sin(45f * Mathf.Deg2Rad), Targetdistance * Mathf.Sin(45f * Mathf.Deg2Rad), -Targetdistance * Mathf.Cos(45f * Mathf.Deg2Rad));
        UnityEngine.Vector3 targetPosition = target.position + offset;
        transform.position = UnityEngine.Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, MoveAnimationDulation); // 1fは移動のスムーズさの時間
        prespectiveDistance = UnityEngine.Vector3.Distance(transform.position,targetPosition);

        // カメラがターゲットを常に向くようにする
        
        transform.LookAt(target);
    }
    public void ChangeLocation(SchoolLocation location = null,int Floor = -1){

        if(location != null){
            var New_Floor = location.Floor;
            if(New_Floor == 0){
                New_Floor = 1;
            }

            stopCameraTracking();
            hub_Manager.LoadFloaData();
            startCameraTracking(New_Floor);
        }
        else{
            if(Floor != -1){
                startCameraTracking(Floor);
            }
            else{
                Debug.Log("ChangeLocationは引数が一つも指定していないため利用できない");
            }
        }
        isDragTimeOut = true;

        return;
    }
    public void stopCameraTracking(){
        isCanUpdate = false;
    }
    public void startCameraTracking(int floor = 1){
        Floa_3D_Deta = hub_Manager.Current_Hub_3D[floor-1];
        isCanUpdate = true;
    }
    
}