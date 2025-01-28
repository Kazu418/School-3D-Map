using UnityEngine;

[CreateAssetMenu(fileName = "LocationsList", menuName = "Scriptable Objects/LocationsList")]
public class LocationsList : ScriptableObject
{   
    public bool isVisible = true;
    public string locationName;
    public string eventName;
    public Texture2D eventPhoto;
    public int gradeNumber;
    public int classNumber;
    public int FloaNumber;
    public int HubNumber;
    public int RoomType;

    [Header("既存座標設定")]
    [Tooltip("既存の部屋の座標を使用するかどうかを決める。")]
    public bool isOrdinaryPosition;
    [Tooltip("is_ordinaryPositionがオンの場合、Asset内の表中の部屋の番号を入力する")]
    public int ordinaryPosition_Number;
    [Header("特殊座標設定")]
    [Tooltip("先ほどの is_ordinaryPositionがオフの場合には具体的な位置を設定する。")]
    public float PositionX;
    [Tooltip("先ほどの is_ordinaryPositionがオフの場合には具体的な位置を設定する。")]
    public float PositionZ;

    [Header("この場所の階層を選択")]
    [Range(0,2)]
    public int Hierarchy = 2;
}
