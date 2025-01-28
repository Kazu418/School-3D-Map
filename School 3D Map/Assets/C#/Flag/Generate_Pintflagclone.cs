using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine;
using System.Collections.Generic;

public class Generate_Pointflagclone : MonoBehaviour
{
  private Info_Master info_Master;

  public displayList display_list;
  private LocationsList[] locations;
  private List<GameObject> flagClones = new List<GameObject> ();
  private FlagCreator flagCreator;

  //Script�ǂݍ���
  private Master_Script Master;
  private Hub_Manager hub_Manager;

  //Master��ł̂��̃X�N���v�g��rank
  private int rank = 2;

  /// <summary>
  /// schoolLocationに移行
  /// </summary>
    private SchoolLocation[] schoolLocationsList;
  void OnEnable()
  {
    Master = GameObject.Find("Master").GetComponent<Master_Script>();
    info_Master = GameObject.Find("Master").GetComponent<Info_Master>();
    hub_Manager = GetComponent<Hub_Manager>();

    Master.Initialize_Reaction[this.rank]++;
  }

  IEnumerator Start()
  {
    //�t���O�����R���|�[�l���g�擾
    flagCreator = GameObject.Find("Search_Flag_Master").GetComponent<FlagCreator>();

    while (!Master.isInitialized[rank])
    {
      yield return null;
    }

    schoolLocationsList = info_Master.schoolLocationsList.ToArray();

    locations = display_list.locations;
    for (int i = 0; i < schoolLocationsList.Length; i++)
    {
      addflagclones(i);
    }
 
    Master.Initialize_Reaction[this.rank]--;
  }
  private void addflagclones(int index)
  {
    var location = schoolLocationsList[index];
    flagClones.Add(flagCreator.CreateFlag(location, false, index));
  }
  public int Get_IndexNum(GameObject touchedflag)
  {
    string objectname = touchedflag.name;

    int index = objectname[objectname.Length - 1] - 48;

    return (index);
  } 
}
