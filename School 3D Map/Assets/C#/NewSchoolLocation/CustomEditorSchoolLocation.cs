using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SchoolLocation))]
public class SchoolLocationSaver : Editor
{
  public void Enable()
  {
    // 元々のInspectorの描画
    base.OnInspectorGUI();

    SchoolLocation schoolLocation = (SchoolLocation)target;

    // データに変更があった場合、SetDirtyでエディタに変更を通知
    EditorUtility.SetDirty(schoolLocation);

    // 必要に応じて、変更後にアセットを保存
    AssetDatabase.SaveAssets();
  }
}
