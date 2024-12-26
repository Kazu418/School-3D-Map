using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class SerachWindowController:MonoBehaviour
{
    private bool isFolded = true;
    private VisualElement root;
    public VisualElement search;
    public TextField textField;

    void OnEnable(){
        root = GetComponent<UIDocument>().rootVisualElement;
        textField = root.Q<TextField>("Text_Input");
        search = root.Q<VisualElement>("Search");

        if(textField == null||search==null){
            Debug.LogError("UI Document,Search内のUI要素が見つかりません。");
        }

        textField.RegisterCallback<FocusEvent>(OnFocus);
        textField.RegisterCallback<BlurEvent>(OnBlur);

        search.AddToClassList("Folded");
    }

    private void OnFocus(FocusEvent evt){
        search.RemoveFromClassList("Folded");
        search.AddToClassList("unFolded");
    }
    private void OnBlur(BlurEvent evt){
        search.RemoveFromClassList("unFolded");
        search.AddToClassList("Folded");
    }

    

}
