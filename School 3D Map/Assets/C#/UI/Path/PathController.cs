using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.IO;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class PathController : MonoBehaviour
{   
    //Master管理用
        private Master_Script Master;
        private int rank = 3;

    //PathButton.uxml関連データをロード
        public VisualTreeAsset PathButton;

        private VisualElement root;
        private VisualElement Path_Element;

    //現在情報の取得
        private Info_Master info_Master;
    //場所をシフトする
        private ShiftPath shiftPath;

    private void OnEnable(){
        Master = GameObject.Find("Master").GetComponent<Master_Script>();
        info_Master = GameObject.Find("Master").GetComponent<Info_Master>();

        root = GetComponent<UIDocument>().rootVisualElement;
        Path_Element = root.Q<VisualElement>("Path_Element");

        shiftPath = GameObject.Find("Search_UI").GetComponent<ShiftPath>();

        Master.Initialize_Reaction[rank]++;
    }
    IEnumerator Start(){
        while(!Master.isInitialized[rank]){
            yield return null;
        }
        setPath();
        Master.Initialize_Reaction[rank]--;
    }
    public void setPath(){
        //一旦全て消去
        Path_Element.Clear();

        SchoolLocation[] info = info_Master.Info;

        int hierarchy = info_Master.Hierarchy;

        for(int i = 0;i<hierarchy+1;i++){
            var button = CreatePathUIClone();
            if(button != null){
                int fixedHierarchyIndex = hierarchy - i;
                button.clicked += () =>{
                    shiftPath.shiftPath(fixedHierarchyIndex);
                };

                button.text = info[i].LocationName;
            }
            else{
                print("Path Buttonが存在しません");
            }
        }
    }
    private Button CreatePathUIClone(){
        var PathButtonUI = PathButton.CloneTree();

        var button = PathButtonUI.Q<Button>("Path_Button");
        Path_Element.Add(PathButtonUI);

        return button;
    }

}
