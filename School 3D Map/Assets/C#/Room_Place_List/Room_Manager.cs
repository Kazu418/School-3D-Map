using UnityEngine;

[CreateAssetMenu(fileName = "Room_Manager", menuName = "Scriptable Objects/Room_Manager")]
public class Room_Manager : ScriptableObject
{
    public Room_Place_Manager[] room_Place_Managers;
}
