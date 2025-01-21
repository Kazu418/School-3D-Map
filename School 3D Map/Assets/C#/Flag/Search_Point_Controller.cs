using UnityEngine;
using UnityEngine.Rendering;

public class FlagCreator : MonoBehaviour
{
    public Room_Manager room_Manager;
    public Flag_Inspector_Controller flag_Inspector_Controller;
    
    private Hub_Manager hub_Manager;

    //Script固有変数
    public float OffsetPosY = 2.53f;
    private GameObject currentFlag;
    private void OnEnable(){
        hub_Manager = GameObject.Find("3D_Model_Master").GetComponent<Hub_Manager>();
    }
    
    public void CreateFlag(LocationsList FlagLocation){
        float PosX,PosZ;

        int floor = FlagLocation.FloaNumber;
        int FlagType = FlagLocation.RoomType;

        if(FlagLocation.isOrdinaryPosition){
            //roomManager.room_Place_Manegers[]（棟で分けた位置情報のリスト)→room_Placess[](棟の中の部屋の位置の番号のリスト)→位置
            PosX = room_Manager.room_Place_Managers[FlagLocation.HubNumber].room_Places[FlagLocation.ordinaryPosition_Number].PositionX;
            PosZ = room_Manager.room_Place_Managers[FlagLocation.HubNumber].room_Places[FlagLocation.ordinaryPosition_Number].PositionZ;

        }
        else{
            //SchoolLocationの特殊座標設定を行った際にこちらの座標になる
            PosX = FlagLocation.PositionX;
            PosZ = FlagLocation.PositionZ;
        }
        float PosY = hub_Manager.Current_Hub_3D[floor-1].transform.position.y + OffsetPosY;
        Vector3 CreatePos = new Vector3(PosX,PosY,PosZ);

        Flag_Inpector Object_Flag = flag_Inspector_Controller.flag_Inpectors[FlagType];

        currentFlag = Instantiate(Object_Flag.FlagData,CreatePos,Quaternion.identity,this.transform);
    }
    public void DeleteCurrentFlag(){
        if(currentFlag != null){
            Destroy(currentFlag);
        }
    }
    
}