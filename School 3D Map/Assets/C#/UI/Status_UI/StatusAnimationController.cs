using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class StatusAnimationController : MonoBehaviour
{   
    //Master管理用
    private int rank = 2;
    private Master_Script Master;

    //Update関数抑制
    private bool isCanUpdate;

    //トランジションアニメーション専用
    private VisualElement root;
    private VisualElement mainStatus;
    private VisualElement subStatus;
    private VisualElement statusBar;
    private VisualElement mainDistinationDisplay;
    public bool isMainStatus;

    //兼用
    private Label mainFloa;
    private Label mainHub;
    private Label subFloor;
    private Label subHub;
    private Label mainDestination;
    private Label subDestination;


    //現在情報を取得
    private Info_Master info_Master;

    private void OnEnable(){

        Master = GameObject.Find("Master").GetComponent<Master_Script>();
        info_Master = GameObject.Find("Master").GetComponent<Info_Master>();

        isCanUpdate = false;

        root = GetComponent<UIDocument>().rootVisualElement;

        //トランジションアニメーション専用
        mainStatus = root.Q<VisualElement>("Main_Status");
        subStatus = root.Q<VisualElement>("Sub_Status");
        statusBar = root.Q<VisualElement>("Status_Bar");
        mainDistinationDisplay = root.Q<VisualElement>("Main_Distination_Display");

        //テキストのみ変更とトランジション兼用
        mainFloa = root.Q<Label>("Main_Floa");
        mainHub = root.Q<Label>("Main_Hub");
        subFloor = root.Q<Label>("Sub_Distination_Floa");
        subHub = root.Q<Label>("Sub_Distination_Hub");
        mainDestination = root.Q<Label>("Main_Distination");
        subDestination = root.Q<Label>("Sub_Distination");

        //HubNameを全ての校舎の名前が書かれたScriptableObjectから読み込み

        Master.Initialize_Reaction[rank]++;
    }
    IEnumerator Start(){
        while(!Master.isInitialized[rank]){
            yield return null;
        }

        isMainStatus = false;

        initialize_Label_Text();
        initializeStatusAnimation();
        Master.Initialize_Reaction[rank]--;

        isCanUpdate = true;
    }

    private void initializeStatusAnimation(){
        statusBar.ClearClassList();
        mainStatus.ClearClassList();
        subStatus.ClearClassList();
        statusBar.AddToClassList("Sub_Status");
        mainStatus.AddToClassList("Hide_Status");
        subStatus.AddToClassList("Show_Status");

        mainFloa.style.left = new StyleLength(new Length(100, LengthUnit.Percent));
        mainHub.style.left = new StyleLength(new Length(100, LengthUnit.Percent));
        mainDistinationDisplay.style.left = new StyleLength(new Length(100, LengthUnit.Percent));
    }

    public IEnumerator TransitionToStatusHide(){
        subStatus.style.left = new StyleLength(new Length(100, LengthUnit.Percent));
        yield return new WaitForSeconds(0.1f);
        mainDistinationDisplay.style.left = new StyleLength(new Length(100, LengthUnit.Percent));
        yield return new WaitForSeconds(0.1f);
        mainHub.style.left = new StyleLength(new Length(100, LengthUnit.Percent));
        yield return new WaitForSeconds(0.1f);
        mainFloa.style.left = new StyleLength(new Length(100, LengthUnit.Percent));
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator TransitionToMainStatus(){

        subStatus.style.left = new StyleLength(new Length(100, LengthUnit.Percent));
        yield return new WaitForSeconds(0.1f);
        statusBar.AddToClassList("Main_Status");
        statusBar.RemoveFromClassList("Sub_Status");

        yield return new WaitForSeconds(0.7f);

        subStatus.RemoveFromClassList("Show_Status");
        subStatus.AddToClassList("Hide_Status");

        mainStatus.RemoveFromClassList("Hide_Status");
        mainStatus.AddToClassList("Show_Status");

        mainFloa.style.left = new StyleLength(new Length(28, LengthUnit.Percent));
        yield return new WaitForSeconds(0.1f);
        mainHub.style.left = new StyleLength(new Length(45, LengthUnit.Percent));
        yield return new WaitForSeconds(0.1f);
        mainDistinationDisplay.style.left = new StyleLength(new Length(58, LengthUnit.Percent));
    }

    public IEnumerator TransitionToSubStatus(){
        mainDistinationDisplay.style.left = new StyleLength(new Length(100, LengthUnit.Percent));
        yield return new WaitForSeconds(0.1f);
        mainHub.style.left = new StyleLength(new Length(100, LengthUnit.Percent));
        yield return new WaitForSeconds(0.1f);
        mainFloa.style.left = new StyleLength(new Length(100, LengthUnit.Percent));
        yield return new WaitForSeconds(0.1f);

        statusBar.AddToClassList("Sub_Status");
        statusBar.RemoveFromClassList("Main_Status");

        yield return new WaitForSeconds(0.4f);

        mainStatus.AddToClassList("Hide_Status");
        mainStatus.RemoveFromClassList("Show_Status");

        subStatus.AddToClassList("Show_Status");
        subStatus.RemoveFromClassList("Hide_Status");

        subStatus.style.left = new StyleLength(new Length(35, LengthUnit.Percent));
    }
    public void ChangeLabelText(SchoolLocation location){
            var info = info_Master.Info;
            var defaultS = info_Master.defaultH;

            var DestinationName = location.LocationName;
            string Direction;
            string HubName;

            if(location.Hierarchy <= 1){
                HubName = "OUTSIDE";
                Direction = "W";

                mainFloa.text = Direction;
                subFloor.text = Direction;
            }
            else{
                HubName = info[defaultS[0]].LocationName;
                var FloorNum = location.Floor;

                mainFloa.text = FloorNum.ToString()+("F");
                subFloor.text = FloorNum.ToString()+("F");
            }

            mainHub.text = HubName;
            subHub.text = HubName;
        
            mainDestination.text = DestinationName;
            subDestination.text = DestinationName;
    }
    private void initialize_Label_Text(){
        var location = info_Master.location;
        var ParentName = location.ParentLocation.LocationName;

        mainFloa.text = location.Floor.ToString()+("F");
        subFloor.text = location.Floor.ToString()+("F");
        mainHub.text = ParentName;
        subHub.text = ParentName;
        mainDestination.text = location.LocationName;
        subDestination.text = location.LocationName;
    }
}