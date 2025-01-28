using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Hub_Data_Initializer : MonoBehaviour
{   
    //Script読み込み
    private Master_Script Master;
    private Hub_Manager hub_Manager;
    private Info_Master Info_Master;
    
    //Script固有変数

    //Master上でのこのスクリプトのrank
    private int rank = 1;

    
    void OnEnable(){
        Master = GameObject.Find("Master").GetComponent<Master_Script>();
        hub_Manager = GetComponent<Hub_Manager>();

        Master.Initialize_Reaction[this.rank]++;
    }
    IEnumerator Start()
    {   
        while(!Master.isInitialized[rank]){
            yield return null;
        }

        hub_Manager.LoadFloaData();
        Debug.Log("Hub_Data_Initializer実行完了");
        Master.Initialize_Reaction[this.rank]--;
    }
}
