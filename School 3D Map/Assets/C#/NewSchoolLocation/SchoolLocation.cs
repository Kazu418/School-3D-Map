using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SchoolLocation", menuName = "Scriptable Objects/SchoolLocation")]
public class SchoolLocation : ScriptableObject
{   
    public bool isVisible = true;
    //この設定によって、次の内容の編集可能かどうかが設定される
    [Range(0,3)]
    public int Hierarchy = 3;
    //Hierarchy 0→3
        public string LocationName;
        public Texture2D LocationPhoto;
    //Hierarchy 0→2
        public  GameObject ModelPrefab;
        public SchoolLocation[] ChildLocation;
    //Hierarchy 1→3
        public SchoolLocation ParentLocation;
    //Hierarchy 2→3
        public int Floor;
    //Hierarchy 3
        public String EventName;
        public int GradeNumber;
        public int ClassNumber;
        public int RoomType;
    
    /// <summary>
    /// ここから別
    /// </summary>
    //座標設定
    [Header("既存座標設定")]
    [Tooltip("既存の部屋の座標を使用するかどうかを決める。")]
    public bool isOrdinaryPosition;
    [Tooltip("is_ordinaryPositionがオンの場合、Asset内の表中の部屋の番号を入力する")]
    public int ordinaryPosition_Number;
    [Header("特殊座標設定")]
    [Tooltip("先ほどの is_ordinaryPositionがオフの場合には具体的な位置を設定する。")]

    //Hierarchy 1,3
    public float PositionX_InAll;
    [Tooltip("先ほどの is_ordinaryPositionがオフの場合には具体的な位置を設定する。")]
    public float PositionZ_InAll;

    //Hierarchy 3
    public float PositionX_InHub;
    public float PositionZ_InHub;
}
