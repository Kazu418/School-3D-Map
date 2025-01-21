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

    //他のスクリプトからのUI操作用の変数
    public int Hub_Name_Num;
    public int Floor_Num;

    //現在このスクリプトで設定しているHubとFloor
    private int Current_Hub_Name_Num;
    private int Current_Floor_Num;

    //Hubの名前を登録する配列
    private string[] Hub_Name = {"Main Hub"};

    private void OnEnable(){

        Master = GameObject.Find("Master").GetComponent<Master_Script>();

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

        Master.Initialize_Reaction[rank]++;
    }
    IEnumerator Start(){
        while(!Master.isInitialized[rank]){
            yield return null;
        }

        //変数の初期化
        Current_Hub_Name_Num = 0;
        Current_Floor_Num = 1;

        Hub_Name_Num = 0;
        Floor_Num = 1;

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
    public void ChangeLabelText(int New_Hub_Num,int New_Floor_Num){
        if(Current_Floor_Num != New_Floor_Num){
            mainFloa.text = New_Floor_Num.ToString()+("F");
            subFloor.text = New_Floor_Num.ToString()+("F");
        }
        if(Current_Hub_Name_Num != New_Hub_Num){
            mainHub.text = Hub_Name[New_Hub_Num];
            subHub.text = Hub_Name[New_Hub_Num];
        }
        Current_Floor_Num = New_Floor_Num;
        Current_Hub_Name_Num = New_Hub_Num;
    }
    private void initialize_Label_Text(){
        mainFloa.text = Current_Floor_Num.ToString()+("F");
        subFloor.text = Current_Floor_Num.ToString()+("F");
        mainHub.text = Hub_Name[Current_Hub_Name_Num];
        subHub.text = Hub_Name[Current_Hub_Name_Num];
    }
}