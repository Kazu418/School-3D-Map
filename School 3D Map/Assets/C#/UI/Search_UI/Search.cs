using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using NUnit.Framework;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;


public class Search : MonoBehaviour
{
    //info_Master管理
        private Info_Master info_Master;

    //パスのコントロール
        private PathController pathController;

    public VisualTreeAsset itemTemplate;
    private ListView locationListView;
    private TextField textInput;
    
    private VisualElement root;
    public SerachWindowController searchWindowController;

    private StatusAnimationController statusAnimationController;

    private SmoothCameraRotation cameraController;
    
    private FlagCreator flagCreator;

    public bool isFirstDrag = false;

    /// <summary>
    /// SchoolLocationに移行する
    /// </summary>
    SchoolLocation schoolLocation;
    public List<SchoolLocation> sortedLocations = new List<SchoolLocation>();
    public SchoolLocation[] filteredLocations;
    public int[] defaultH;
    
    void OnEnable()
    {    
        //info_masterを管理
        info_Master = GameObject.Find("Master").GetComponent<Info_Master>();

        //フラグ生成コンポーネント取得
        flagCreator = GameObject.Find("Search_Flag_Master").GetComponent<FlagCreator>();

        //パスファイル取得
        pathController = GameObject.Find("Search_UI").GetComponent<PathController>();

        //serarch用のUI要素の検索
        root = GetComponent<UIDocument>().rootVisualElement;
        textInput = root.Q<TextField>("Text_Input");
        locationListView = root.Q<ListView>("Location_List_View");

        //menu用のUI要素の検索
        statusAnimationController = GameObject.Find("Status_UI").GetComponent<StatusAnimationController>();

        //SearchWindowControllerの検索
        searchWindowController = GetComponent<SerachWindowController>();

        //CameraのC#スクリプト取得
        cameraController = GameObject.Find("Main Camera").GetComponent<SmoothCameraRotation>();

        defaultH = info_Master.defaultH;


        //SchoolLocationのデータを取得
            schoolLocation = info_Master.AllData;
            //AllDataから表示する内容をフィルタする。Hierarchyのフィルタは2~3で制限。
            sortLocation(schoolLocation,0,info_Master.searchHierarchyEnd);
            filteredLocations = sortedLocations.ToArray();
        SetupListView();

        textInput.RegisterValueChangedCallback(evt =>{
            string input = evt.newValue.ToLower();
            filteredLocations = sortedLocations
            .Where(location => location.LocationName.ToLower().Contains(input))
            .ToArray();
            //locationは、data.locationsの要素を格納するためにある変数
            SetupListView();
        });
    }

    private void Update(){
        if(Input.GetMouseButtonDown(0) && isFirstDrag == true){
            StartCoroutine(statusAnimationController.TransitionToSubStatus());
            isFirstDrag = false;
        }
    }
    void SetupListView(){
        locationListView.itemsSource = filteredLocations;

        locationListView.makeItem = () => {
            var item = itemTemplate.CloneTree();  // テンプレートを複製
            return item;
        };
        locationListView.bindItem = (element, index) => {
            var location = filteredLocations[index];
            //indexは、リストビューの中のアイテムの順番を記録しており、一度アイテムの描画が終わっても、全て終わるまで繰り返している。

            var locationNameLabel = element.Q<Label>("Location_Name");
            var eventNameLabel = element.Q<Label>("Event_Name");
            var locationPhoto = element.Q<VisualElement>("Location_Photo");
            var classNum = element.Q<Label>("Class_Num");
            var gradeNum = element.Q<Label>("Grade_Num");

            locationNameLabel.text = location.LocationName;
            eventNameLabel.text = location.EventName;
            classNum.text = location.ClassNumber.ToString();
            gradeNum.text = location.GradeNumber.ToString();
            locationPhoto.style.backgroundImage = new StyleBackground(location.LocationPhoto);

            var buttonGo = element.Q<Button>("Button_GO");
            if(buttonGo != null){
                buttonGo.name = $"Button_GO_{index}";

                buttonGo.RemoveFromClassList("onButtonClicked");
                buttonGo.AddToClassList("button");

                buttonGo.clicked += () =>{
                    flagCreator.DeleteCurrentFlag();

                    buttonGo.RemoveFromClassList("button");
                    buttonGo.AddToClassList("onButtonClicked");

                    StartCoroutine(OnGoButtonClicked(index));
                    StartCoroutine(ResetButtonClassAfterDelay(buttonGo, 0.1f));
                };
            }
            else{
                Debug.Log("ボタンはNull");
            }
        };
        locationListView.Rebuild();
    }
    
    IEnumerator OnGoButtonClicked(int index){
        var location = filteredLocations[index];
        
       // flagCreator.CreateFlag(location, true, 0);

        //info_Masterに反映
        info_Master.New_SetCurrent(location);

        //カメラをコントロール
        cameraController.ChangeLocation(location);

        pathController.setPath();

        //flagCreator.CreateFlag(location);

        var info = info_Master.Info;
        statusAnimationController.ChangeLabelText(location);
        yield return new WaitForSeconds(0.5f);
    }

    private System.Collections.IEnumerator ResetButtonClassAfterDelay(Button button, float delay)
    {
        yield return new WaitForSeconds(delay);
        button.RemoveFromClassList("onButtonClicked");
        button.AddToClassList("button");
        yield return new WaitForSeconds(0.3f);
        statusAnimationController.isMainStatus = false;
        searchWindowController.hideWindow();
        isFirstDrag = true;
    }

    public void sortLocation(SchoolLocation location,int startHierarchy,int endHierarchy){
        if(location == null) return;

        var hierarchy = location.Hierarchy;
        if(startHierarchy <= hierarchy && endHierarchy >= hierarchy){
            sortedLocations.Add(location);
        }

        if(location.ChildLocation != null){
            foreach(var child in location.ChildLocation){
                sortLocation(child,startHierarchy,endHierarchy);
            }
        }
    }
}
