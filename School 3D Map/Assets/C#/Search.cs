using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
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
    
    void OnEnable()
    {   
        var root = GetComponent<UIDocument>().rootVisualElement;
        textInput = root.Q<TextField>("Text_Input");
        locationListView = root.Q<ListView>("Location_List_View");

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

    private void ResetButtonClass(){
        var buttonGO = GetComponent<Button>();
        buttonGO.AddToClassList("button");
        buttonGO.RemoveFromClassList("onButtonClicked");
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
        };
        locationListView.Rebuild();
    }
}
