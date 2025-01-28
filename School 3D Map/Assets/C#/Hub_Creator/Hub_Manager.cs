using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Hub_Manager : MonoBehaviour
{   
    //ハブのデータを読み込む
        private Info_Master info_Master;
    //フロアの最大数
        static public int MaxFloor = 5;
    //Hubデータ読み込み
        public GameObject[] Data;

    //Script固有
        public GameObject[] Current_Hub_3D = new GameObject[MaxFloor];

    private void OnEnable(){
        info_Master = GameObject.Find("Master").GetComponent<Info_Master>();
    }
    
    public void LoadFloaData(){
        Data = info_Master.Model;
        
        if(Current_Hub_3D != null){
            for(int i = 0;i<MaxFloor;i++){
                Destroy(Current_Hub_3D[i]);
            }
        }

        if(Data == null){
            Debug.Log("ロードする3Dデータが存在しません。");
            return;
        }

        for(int i = 0;i<Data.Length;i++){
            UnityEngine.Vector3 newPos = new UnityEngine.Vector3(0,i*100f,0);
            var floor = Instantiate(Data[i],newPos,UnityEngine.Quaternion.identity, this.transform);
            
            Current_Hub_3D[i] = floor;
        }
    }
}
