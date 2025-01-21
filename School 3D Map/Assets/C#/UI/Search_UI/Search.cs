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
    public displayList data;
    public VisualTreeAsset itemTemplate;
    private ListView locationListView;
    private TextField textInput;
    private LocationsList[] filteredLocations;
    private VisualElement root;
    public SerachWindowController searchWindowController;

    private StatusAnimationController statusAnimationController;

    private SmoothCameraRotation cameraController;
    
    private FlagCreator flagCreator;

    public bool isFirstDrag = false;
    
    
    void OnEnable()
    {   
        //フラグ生成コンポーネント取得
        flagCreator = GameObject.Find("Search_Flag_Master").GetComponent<FlagCreator>();

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

        filteredLocations = data.locations;
        SetupListView();

        textInput.RegisterValueChangedCallback(evt =>{
            string input = evt.newValue.ToLower();
            filteredLocations = data.locations
            .Where(location => location.locationName.ToLower().Contains(input))
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

            locationNameLabel.text = location.locationName;
            eventNameLabel.text = location.eventName;
            classNum.text = location.classNumber.ToString();
            gradeNum.text = location.gradeNumber.ToString();
            locationPhoto.style.backgroundImage = new StyleBackground(location.eventPhoto);

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

        flagCreator.CreateFlag(location);

        statusAnimationController.ChangeLabelText(location.HubNumber,location.FloaNumber);
        yield return new WaitForSeconds(0.5f);
        //カメラをコントロール(校舎間移動はまだ機能しない)
        cameraController.ChangeFloor(location.FloaNumber);
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
}
