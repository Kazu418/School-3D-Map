using UnityEngine;

[CreateAssetMenu(fileName = "FlagsList", menuName = "Scriptable Objects/FlagsList")]
public class FlagsList : ScriptableObject
{
  public int Type; //「0」:校舎別用「1」:階層別用
  public int ID; //例)100番教室⇒100,11番教室⇒11
  public int Status; //「0」:不可「1」:可
}
