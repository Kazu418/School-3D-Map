using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class FlagCreator : MonoBehaviour
{
    public Room_Manager room_Manager;
    public Flag_Inspector_Controller flag_Inspector_Controller;
    
    private Hub_Manager hub_Manager;

    //Script固有変数
    public float OffsetPosY = 2.53f;
    private GameObject currentSearchFlag;

    private int index = 0;
    private void OnEnable(){
        hub_Manager = GameObject.Find("3D_Model_Master").GetComponent<Hub_Manager>();
    }
    
    public GameObject CreateFlag(SchoolLocation FlagLocation, bool isSearch, int index){
        index++;
        SchoolLocation Hub = FlagLocation;
        int HubNumber = -1;
        while(Hub.Hierarchy >= 2){
          Hub = Hub.ParentLocation;
        }

        if(Hub.LocationName == "Main Hub"){
          HubNumber = 1;
        }
        else if(Hub.LocationName == "Library"){
          HubNumber = 1;
        }
        else{
          Debug.LogError("ベータテストエラー 反復参照したオブジェクトの名前が想定と違います。");
        }

        float PosX,PosZ;

        int floor = FlagLocation.Floor;
        int FlagType = FlagLocation.RoomType;

      if (FlagLocation.isOrdinaryPosition){
            //roomManager.room_Place_Manegers[]（棟で分けた位置情報のリスト)→room_Placess[](棟の中の部屋の位置の番号のリスト)→位置
            PosX = room_Manager.room_Place_Managers[HubNumber].room_Places[FlagLocation.ordinaryPosition_Number].PositionX;
            PosZ = room_Manager.room_Place_Managers[HubNumber].room_Places[FlagLocation.ordinaryPosition_Number].PositionZ;

      }
      else{
            //SchoolLocationの特殊座標設定を行った際にこちらの座標になる
            PosX = FlagLocation.PositionX_InHub;
            PosZ = FlagLocation.PositionZ_InHub;
      }

      //MainHubや、Allなどはフロアがないため、0が出てくる。そこでエラーが出ないために１にしている。
      if(floor == 0) floor++;
      
      float PosY = hub_Manager.Current_Hub_3D[floor-1].transform.position.y + OffsetPosY;
      Vector3 CreatePos = new Vector3(PosX,PosY,PosZ);

      Flag_Inpector Object_Flag = flag_Inspector_Controller.flag_Inpectors[FlagType];

      if (isSearch){
        currentSearchFlag = Instantiate(Object_Flag.FlagData, CreatePos, Quaternion.identity, this.transform);
        return null;
      }
      else {
        Debug.Log("cloned.");
        GameObject clone = Instantiate(Object_Flag.FlagData, CreatePos, Quaternion.identity, this.transform);
        clone.name = $"{Object_Flag}_{index}";
        return clone;
      }     
    }
    public void DeleteCurrentFlag(){
        if(currentSearchFlag != null){
            Destroy(currentSearchFlag);
        }
    }
    
}