using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public class ShiftPath : MonoBehaviour
{
    //現在のロケーションを取得するためのInfo_Master
        private Info_Master info_Master;
    //ハブのクローンを生成する場合のHub_Managaer
        private Hub_Manager hub_Manager;
    //カメラのコントロールをとる
        private SmoothCameraRotation cameraController;
    //パスを更新する関数を使用
        private PathController pathController;

    void OnEnable(){
        info_Master = GameObject.Find("Master").GetComponent<Info_Master>();
        hub_Manager = GameObject.Find("3D_Model_Master").GetComponent<Hub_Manager>();
        cameraController = GameObject.Find("Main Camera").GetComponent<SmoothCameraRotation>();
        pathController = GameObject.Find("Search_UI").GetComponent<PathController>();
    }
    
    public void shiftPath(int backCount){
        int hierarchy = info_Master.Hierarchy;
        int newHierarchy = hierarchy-backCount;
        SchoolLocation[] info = info_Master.Info;
        
        if(newHierarchy >= 0 && newHierarchy <= hierarchy){
            info_Master.New_SetCurrent(info[newHierarchy]);
            pathController.setPath();
            ChangeHub();
        }
        else{
            print(newHierarchy+"ヒエラルキーの範囲の設定ミス");
        }

    }

    public void ChangeHub(){
        cameraController.stopCameraTracking();
        hub_Manager.LoadFloaData();
        cameraController.startCameraTracking();
    }
}
