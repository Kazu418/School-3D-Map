using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class Info_Master : MonoBehaviour
{    
    //Master管理用
        private Master_Script Master;
        private int rank = 0;

    //現在のステータスの表示内容の設定（StatusのHubの名前を表示する部分のみ）
        /// <summary>
        /// StatusのUI部分は、このinfo_MasterのInfoから情報を取得している。その場合に、MainHubなどのHubの表示部分は、固定でHubの名前を表示したいので、Infoの何番目に入っているかを示している。
        /// </summary>
        public int[] defaultH = new int[] {1};
        public int searchHierarchyStart = 0;
        public int searchHierarchyEnd = 3;

    //設定用
        public Room_Manager room_Manager;

        public LocationsList Current_Location;
    //このアプリでの階層分けの深度
        static public int depth = 4;
    
    /// <summary>
    /// 新しいロケーションのスクリプタブルオブジェクト用のデータ
    /// </summary>
        public SchoolLocation AllData;
        public SchoolLocation FirstData;
        public SchoolLocation[] Info;
        public GameObject[] Model;
        public int Hierarchy;
        public SchoolLocation location;
    
    /// <summary>
    /// Flagを作成する部分では、SchoolLocationが階層型（単純なリストではなく、Hubとフロア、また特定の教室が、親子関係によって結びついている仕組み）
    /// それが邪魔であるので、階層型を考えずに全てをリストに変換する
    /// </summary>
        public List<SchoolLocation> schoolLocationsList;


    private void OnEnable(){
        Master = GameObject.Find("Master").GetComponent<Master_Script>();
        Master.Initialize_Reaction[rank]++;
    }

    IEnumerator Start(){
        while(!Master.isInitialized[rank]){
            yield return null;
        }
        //フラグ用の配列を生成
        sortLocation(AllData);

        //初期設定
            New_SetCurrent(FirstData);
            for(int i = 0;i<4;i++){
                var name = Info[i].LocationName;
            }

        Master.Initialize_Reaction[rank]--;
    }

public void New_SetCurrent(SchoolLocation Location) {
    if (Location == null) {
        Debug.LogError("LocationがNullです");
        return;
    }
    //共有用の変数に変数に情報を提供
    location = Location;

    int hierarchy = Location.Hierarchy;
    Info = new SchoolLocation[hierarchy + 1]; // インデックス 0 ～ hierarchy まで

    // 現在のLocationから親を遡って配列に格納
    SchoolLocation current = Location;
    for (int i = hierarchy; i >= 0; i--) {
        Info[i] = current;
        current = current.ParentLocation; // 親に移動

        // 最上位階層でParentLocationがnullの場合のチェック
        if (current == null && i > 0) {
            Debug.LogError($"階層 {i} でParentLocationが存在しません");
            break;
        }
    }
    //現在のヒエラルキーをInfo_Masterの持つ正式な情報として登録
    Hierarchy = hierarchy;

    int modelCount;
    for(int i = hierarchy;i>=0;i--){
        var location = Info[i];
        SchoolLocation parent;
        if(i == 0){
            if(location.ModelPrefab != null){
                modelCount = 1;
                Model = new GameObject[modelCount];

                Model[0] = location.ModelPrefab;
            }
            else{
                Debug.LogError("All用のデータが存在しません");
            }
            break;
        }

        parent = Info[i-1];
        
        if(parent.ChildLocation[0].ModelPrefab != null){
 
            modelCount = parent.ChildLocation.Length;

            Model = new GameObject[modelCount];

            for(int j = 0;j<modelCount;j++){
                var modelPrefab = parent.ChildLocation[j].ModelPrefab;
                if(modelPrefab != null){
                    Model[j] = modelPrefab;
                }
                else if(j != 0 && Model[j-1] != null){
                    Model[j] = Model[j-1];
                }
                else{
                    Debug.Log("モデルデータが見当たりません。");
                }
            }   
            
            break;
        }
    }
}  
    private void sortLocation(SchoolLocation location){
        if(location == null) return;

        var hierarchy = location.Hierarchy;
        if(1 <= hierarchy){
            schoolLocationsList.Add(location);
        }

        if(location.ChildLocation != null){
            foreach(var child in location.ChildLocation){
                sortLocation(child);
            }
        }
    }
}
