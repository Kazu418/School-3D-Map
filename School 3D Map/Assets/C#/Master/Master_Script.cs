using System;
using System.Collections;
using UnityEngine;

public class Master_Script : MonoBehaviour
{
    /*
        Master_Scriptでは、アプリ全体の管理を行う。

        初期化順
        1.LoadingUIの起動
        2.初期モデルの生成
        3.CameraController/他のUIの起動

        他の void startの部分をIEnumeratorを使って、Master_Scpritの変数の状態を読み取って、
        開始を遅らせることで、順番を管理。
    */

    //rankの階層数
    static private int rank = 3;

    //全体の初期化順番の管理配列
    public bool[] isInitialized = new bool[rank];

    //各ランクの初期化状態管理
    public int[] Initialize_Reaction  = new int[rank];

    /// <summary>
    /// 
    /// 初期化に使っている↑の配列は、C#の性質によって初期化しなくても、intは0、boolはfalseとして埋められる。
    /// それに該当するのは、メンバ変数(クラス上で定義される変数)と、配列のみ。ローカル変数にこれは通用しない。
    /// 初期化をvoid Awakeで行わないのは、他のスクリプトのOnEnableとかぶって、順序立てた初期化がうまくいかなくなるため。
    /// 
    /// </summary>

    void Awake()
    {
        //初期化
        StartCoroutine(Initialize());
    }

    void Update()
    {

    }

    IEnumerator Initialize(){
        //階層順に初期化
        for(int i = 0;i<rank;i++){
            isInitialized[i] = true;
            while(!(Initialize_Reaction[i]==0)){
                yield return null;
            }
        }
    }
}
