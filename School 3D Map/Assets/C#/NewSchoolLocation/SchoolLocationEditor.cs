using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SchoolLocation))]
public class SchoolLocationEditor : Editor
{
  private SerializedProperty Find(string name) => serializedObject.FindProperty(name);

  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    // 基本設定
    EditorGUILayout.PropertyField(Find("isVisible"));
    EditorGUILayout.IntSlider(Find("Hierarchy"), 0, 3);

    int hierarchyValue = Find("Hierarchy").intValue;

    // Hierarchy 0→3
    if (hierarchyValue >= 0)
    {
      EditorGUILayout.PropertyField(Find("LocationName"));
      EditorGUILayout.PropertyField(Find("LocationPhoto"));
    }

    // Hierarchy 3
    if (hierarchyValue == 3)
    {
      EditorGUILayout.PropertyField(Find("EventName"));
      EditorGUILayout.PropertyField(Find("GradeNumber"));
      EditorGUILayout.PropertyField(Find("ClassNumber"));
      EditorGUILayout.PropertyField(Find("RoomType"));
    }

    // Hierarchy 0→2
    if (hierarchyValue <= 2)
    {
      EditorGUILayout.PropertyField(Find("ModelPrefab"));
      EditorGUILayout.PropertyField(Find("ChildLocation"), true);
    }

    // Hierarchy 1→3
    if (hierarchyValue >= 1)
    {
      EditorGUILayout.PropertyField(Find("ParentLocation"));
    }

    // Hierarchy 2→3
    if (hierarchyValue >= 2)
    {
      EditorGUILayout.PropertyField(Find("Floor"));
    }

    // 座標設定
    EditorGUILayout.Space();
    EditorGUILayout.LabelField("座標設定", EditorStyles.boldLabel);

    EditorGUILayout.PropertyField(Find("isOrdinaryPosition"));
    if (Find("isOrdinaryPosition").boolValue)
    {
      EditorGUILayout.PropertyField(Find("ordinaryPosition_Number"));
    }
    else
    {
      EditorGUILayout.PropertyField(Find("PositionX_InAll"));
      EditorGUILayout.PropertyField(Find("PositionZ_InAll"));
    }

    if (hierarchyValue == 3 && !Find("isOrdinaryPosition").boolValue)
    {
      EditorGUILayout.PropertyField(Find("PositionX_InHub"));
      EditorGUILayout.PropertyField(Find("PositionZ_InHub"));
    }

    // 変更を適用
    serializedObject.ApplyModifiedProperties();

    // 即時保存（オプション）
    if (GUI.changed)
    {
      EditorUtility.SetDirty(target);
      AssetDatabase.SaveAssets();
    }
  }
}