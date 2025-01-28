using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class Info_Master : MonoBehaviour
{    
    //Master管理用
        private Master_Script Master;
        private int rank = 0;

    //現在のステータスの表示内容の設定（Hubの名前を表示する部分のみ）
    public int[] defaultH = new int[] {1};
    public int searchHierarchyStart = 0;
    public int searchHierarchyEnd = 3;

    //設定用
        public Room_Manager room_Manager;


    //校舎の名前
        [HideInInspector]
        public string[] Hub_Name;

        [HideInInspector]
        public LocationsList Current_Location;
    //このアプリでの階層分けの深度
        static public int depth = 4;
    
    /// <summary>
    /// 新しいロケーションのスクリプタブルオブジェクト用のデータ
    /// </summary>
    public SchoolLocation AllData;
    public SchoolLocation FirstData;
    public SchoolLocation[] Info;
    public GameObject[] Model;
    public int Hierarchy;
    public SchoolLocation location;




    private void OnEnable(){
        Master = GameObject.Find("Master").GetComponent<Master_Script>();
        Master.Initialize_Reaction[rank]++;
    }

    IEnumerator Start(){
        while(!Master.isInitialized[rank]){
            yield return null;
        } 
        
        //校舎の名前設定
            Room_Place_Manager[] nameList = room_Manager.room_Place_Managers;
            Hub_Name = new string[nameList.Length];

            for(int i = 0;i<nameList.Length;i++){
                Hub_Name[i] = nameList[i].HubName;
            }

        //初期設定
            New_SetCurrent(FirstData);
            for(int i = 0;i<4;i++){
                var name = Info[i].LocationName;
            }

        Master.Initialize_Reaction[rank]--;
    }

public void New_SetCurrent(SchoolLocation Location) {
    if (Location == null) {
        Debug.LogError("LocationがNullです");
        return;
    }
    //共有用の変数に変数に情報を提供
    location = Location;

    int hierarchy = Location.Hierarchy;
    Info = new SchoolLocation[hierarchy + 1]; // インデックス 0 ～ hierarchy まで

    // 現在のLocationから親を遡って配列に格納
    SchoolLocation current = Location;
    for (int i = hierarchy; i >= 0; i--) {
        Info[i] = current;
        current = current.ParentLocation; // 親に移動

        // 最上位階層でParentLocationがnullの場合のチェック
        if (current == null && i > 0) {
            Debug.LogError($"階層 {i} でParentLocationが存在しません");
            break;
        }
    }
    //現在のヒエラルキーをInfo_Masterの持つ正式な情報として登録
    Hierarchy = hierarchy;

    int modelCount;
    for(int i = hierarchy;i>=0;i--){
        var location = Info[i];
        SchoolLocation parent;
        if(i == 0){
            if(location.ModelPrefab != null){
                modelCount = 1;
                Model = new GameObject[modelCount];

                Model[0] = location.ModelPrefab;
            }
            else{
                Debug.LogError("All用のデータが存在しません");
            }
            break;
        }

        parent = Info[i-1];
        
        if(parent.ChildLocation[0].ModelPrefab != null){
 
            modelCount = parent.ChildLocation.Length;

            Model = new GameObject[modelCount];

            for(int j = 0;j<modelCount;j++){
                var modelPrefab = parent.ChildLocation[j].ModelPrefab;
                if(modelPrefab != null){
                    Model[j] = modelPrefab;
                }
                else if(j != 0 && Model[j-1] != null){
                    Model[j] = Model[j-1];
                }
                else{
                    Debug.Log("モデルデータが見当たりません。");
                }
            }   
            
            break;
        }
    }
}  
}
