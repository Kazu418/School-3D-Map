using UnityEngine;

public class FlagData : MonoBehaviour
{
  public FlagsList flagdata;
  private int type;
  private int id;
  private int status;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
    {
    type = flagdata.Type;
    id = flagdata.ID;
    status = flagdata.Status;
    Debug.Log($"Type: {type}, ID: {id}, Status: {status}");
  }
}
