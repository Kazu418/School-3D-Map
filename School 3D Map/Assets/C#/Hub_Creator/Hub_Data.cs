using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Floor_Data", menuName = "Scriptable Objects/Floa_Data")]
public class Hub_Data : ScriptableObject
{
    [Header("基本情報")]
    public int Hub_Name_Num;
    [Tooltip("0でMain Hub、1で図書館")]
    public int Num_Floors;
    public GameObject Floor_3D_Prefab;
}
