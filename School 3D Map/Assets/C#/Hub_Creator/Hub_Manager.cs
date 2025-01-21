using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Hub_Manager : MonoBehaviour
{   
    //Hubデータ読み込み
    public Hub_Data[] Hub_data;

    //Script固有
    public GameObject[] Current_Hub_3D = new GameObject[5];
    
    public void LoadFloaData(int HubIndex,Transform parent){
        if(HubIndex < 0 || HubIndex >= Hub_data.Length){
            Debug.Log("存在しない棟のデータを指定しています。");
            return;
        }

        Hub_Data selectedHub = Hub_data[HubIndex];
        for(int i = 0;i<selectedHub.Num_Floors;i++){
            UnityEngine.Vector3 newPos = new UnityEngine.Vector3(0,i*100f,0);
            var floor = Instantiate(selectedHub.Floor_3D_Prefab,newPos,UnityEngine.Quaternion.identity, parent);
            
            Current_Hub_3D[i] = floor;
        }
    }
}
