using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SchoolLocation))]
public class SchoolLocationSaver : Editor
{
  public void Enable()
  {
    // ���X��Inspector�̕`��
    base.OnInspectorGUI();

    SchoolLocation schoolLocation = (SchoolLocation)target;

    // �f�[�^�ɕύX���������ꍇ�ASetDirty�ŃG�f�B�^�ɕύX��ʒm
    EditorUtility.SetDirty(schoolLocation);

    // �K�v�ɉ����āA�ύX��ɃA�Z�b�g��ۑ�
    AssetDatabase.SaveAssets();
  }
}
