using System.ComponentModel.Design.Serialization;
using Mono.Cecil;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UIElements;

public class SerachWindowController:MonoBehaviour
{
    private bool isFolded = true;
    private VisualElement root;
    public VisualElement search;
    public VisualElement view;
    private VisualElement searchBar;
    private VisualElement searchButton;
    public TextField textField;
    private ListView locationListView;


    private Vector2 touchStartPosition;
    private float SwipeThreshold = 0.5f;
    public bool isDragging = false;
    private float windowStartPositionY;
    private float screenHeight;

    void OnEnable(){

        root = GetComponent<UIDocument>().rootVisualElement;
        textField = root.Q<TextField>("Text_Input");
        search = root.Q<VisualElement>("Search");
        view = root.Q<VisualElement>("View");
        searchBar = root.Q<VisualElement>("Search_Bar");
        searchButton = root.Q<VisualElement>("Search_Button");
        locationListView = root.Q<ListView>("Location_List_View");

        if(textField == null||search==null||locationListView == null){
            Debug.LogError("UI Document,Search内のUI要素が見つかりません。");
        }

        textField.RegisterCallback<FocusEvent>(OnFocus);

        search.RegisterCallback<PointerDownEvent>(OnPointerDown);
        view.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        view.RegisterCallback<PointerUpEvent>(OnPointerUp);
        view.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        view.RegisterCallback<PointerDownEvent>(evt=>{
            if(!isFolded){
                hideWindow();
            }
            evt.StopPropagation();
        });

        //Uiサイズ制御
        searchBar.RegisterCallback<GeometryChangedEvent>(evt =>{
            float searchBarWidth = searchBar.resolvedStyle.width;
            float borderRadiusRate = 0.05f;
            searchBar.SetBorderRadius(searchBarWidth*borderRadiusRate);
            //searchBarWidthは578px

            float searchButtonSizeRate = 0.043f;
            searchButton.style.width = searchBarWidth*searchButtonSizeRate;
            searchButton.style.height = searchBarWidth*searchButtonSizeRate;

            float searchButtonRadiusRate = 0.01f;
            searchButton.SetBorderRadius(searchBarWidth*searchButtonRadiusRate);
        });
        //レイアウトセット前に画面サイズを設定しようとすると、正確な値が返ってこない。
        textField.RegisterCallback<GeometryChangedEvent>(evt=>{
           float textFieldWidth = textField.resolvedStyle.width;
           float radiusRate = 0.036f;
           textField.SetBorderRadius(textFieldWidth*radiusRate);

           float fontSizeRate = 0.03f; 
           textField.style.fontSize = textFieldWidth*fontSizeRate;
        });


        //ボタンのクリックイベントの追加
        locationListView.RegisterCallback<GeometryChangedEvent>(evt =>{
            foreach(var item in locationListView.itemsSource){
            var buttonGo = locationListView.Q<Button>("Button_GO");
            if(buttonGo != null){
                buttonGo.clicked += () =>{
                    buttonGo.RemoveFromClassList("button");
                    buttonGo.AddToClassList("onButtonClicked");
                    print("ボタンクリックされました");

                    Invoke(nameof(ResetButtonClass),0.5f);
                };
            }
            else{
                print("ボタンはNull");
            }
        }
        });
    }

    //これより下、RegisterCallbackによって実行され、（ユーザーが特定の動作をした場合の）実行する関数
    private void OnFocus(FocusEvent evt){
        showWindow();
    }

    private void OnPointerDown(PointerDownEvent evt){
        touchStartPosition = evt.position;
        windowStartPositionY = search.resolvedStyle.top;
        isDragging = true;
        evt.StopPropagation();
    }
    private void OnPointerMove(PointerMoveEvent evt){
        if(!isDragging) return;
        float deltaY = evt.position.y - touchStartPosition.y;

        search.style.top = new StyleLength(windowStartPositionY + deltaY);
    }
    private void OnPointerUp(PointerUpEvent evt){
        if(!isDragging) return;
        float deltaY = evt.position.y - touchStartPosition.y;

        moveToFixedPosition(deltaY);
        
    }
    private void OnPointerLeave(PointerLeaveEvent evt){
        float deltaY = evt.position.y - touchStartPosition.y;

        moveToFixedPosition(deltaY);
    }

//これより下、実行される関数の中で使われる
    private void moveToFixedPosition(float deltaY){
        isDragging = false;

        if(Mathf.Abs(deltaY)>screenHeight*SwipeThreshold){
            if(deltaY < 0){
                showWindow();
            }
            else{
                hideWindow();
            }
        }
        else{
            if(isFolded){
                hideWindow();
            }
            else{
                showWindow();
            }
        }
    }
    private void showWindow(){
        search.style.top = new StyleLength(Length.Percent(8));
        isFolded = false;
    }
    private void hideWindow(){
        search.style.top = new StyleLength(Length.Percent(92));
        isFolded = true;
    }
    //ボタンクリック時のアニメーション関数
    private void ResetButtonClass(){
        var buttonGo = root.Q<Button>("Button_GO");
        buttonGo.RemoveFromClassList("onButtonClicked");
        buttonGo.AddToClassList("button");

        print("ボタン　戻る");
    }

}

public static class VisualElementExtensions{
        public static void SetBorderRadius(this VisualElement element, float radius)
    {
        element.style.borderTopLeftRadius = radius;
        element.style.borderTopRightRadius = radius;
        element.style.borderBottomLeftRadius = radius;
        element.style.borderBottomRightRadius = radius;
    }
}