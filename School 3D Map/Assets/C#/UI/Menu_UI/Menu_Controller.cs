using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UIElements;

public class Menu_Controller : MonoBehaviour
{
    private VisualElement root;
    private Button FocusOn_Button;
    private Button Flag_Button;
    private Button Center_Button;
    private VisualElement Selection;
    private VisualElement Opacity_Controll;
    private bool FocusOn_Button_Eable;
    public bool isFlag;
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        FocusOn_Button = root.Q<Button>("FocusOn_Button");
        Flag_Button = root.Q<Button>("Flag_Button");
        Center_Button = root.Q<Button>("Center_Button");
        Selection = root.Q<VisualElement>("Selection");
        Opacity_Controll = root.Q<VisualElement>("Opacity_Controll");
        FocusOn_Button_Eable = false;

        FocusOn_Button.clicked += OnFocusOn_ButtonClicked;
        Flag_Button.clicked += OnFlag_ButtonClicked;
        Center_Button.clicked += OnCenter_ButtonClicked;

        
        //初期設定
        OnCenter_ButtonClicked();

        Opacity_Controll.AddToClassList("Opacity_FocusOn_Button");
        Opacity_Controll.RemoveFromClassList("Opacity_FocusOn_Button_Clicked");
        Opacity_Controll.style.opacity = 0.0f;
        //初期設定終了  
    }

    private void OnFocusOn_ButtonClicked(){
        if(FocusOn_Button_Eable == false){
            FocusOn_Button.RemoveFromClassList("FocusOn_Button");
            FocusOn_Button.AddToClassList("FocusOn_Button_Clicked");
            Opacity_Controll.AddToClassList("Opacity_FocusOn_Button_Clicked");
            Opacity_Controll.RemoveFromClassList("Opacity_FocusOn_Button");
            Opacity_Controll.style.opacity = 1.0f;
            FocusOn_Button_Eable = true;
        }
        else{
            FocusOn_Button.RemoveFromClassList("FocusOn_Button_Clicked");
            FocusOn_Button.AddToClassList("FocusOn_Button");
            Opacity_Controll.AddToClassList("Opacity_FocusOn_Button");
            Opacity_Controll.RemoveFromClassList("Opacity_FocusOn_Button_Clicked");
            Opacity_Controll.style.opacity = 0.0f;
            FocusOn_Button_Eable = false;
        }
    }
    private void OnFlag_ButtonClicked(){
        Selection.AddToClassList("Select_Flag");
        Selection.RemoveFromClassList("Select_Center");
        isFlag = true;
    }
    private void OnCenter_ButtonClicked(){
        Selection.AddToClassList("Select_Center");
        Selection.RemoveFromClassList("Select_Flag");
        isFlag = false;
    }
}
