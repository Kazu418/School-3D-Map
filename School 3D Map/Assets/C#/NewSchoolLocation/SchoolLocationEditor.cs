using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SchoolLocation))]
public class SchoolLocationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // スクリプタブルオブジェクトの参照を取得
        SchoolLocation schoolLocation = (SchoolLocation)target;

        // Hierarchy の値に基づいてプロパティの表示をカスタマイズ
        schoolLocation.isVisible = EditorGUILayout.Toggle("Is Visible", schoolLocation.isVisible);

        schoolLocation.Hierarchy = EditorGUILayout.IntSlider("Hierarchy", schoolLocation.Hierarchy, 0, 3);

        // Hierarchy 0 → 3
        if (schoolLocation.Hierarchy >= 0)
        {
            schoolLocation.LocationName = EditorGUILayout.TextField("Location Name", schoolLocation.LocationName);
            schoolLocation.LocationPhoto = (Texture2D)EditorGUILayout.ObjectField("Location Photo", schoolLocation.LocationPhoto, typeof(Texture2D), false);
        }
        //Hierarchy 3の時のみ
        if (schoolLocation.Hierarchy == 3)
        {
            schoolLocation.EventName = EditorGUILayout.TextField("Event Name", schoolLocation.EventName);
            schoolLocation.GradeNumber = EditorGUILayout.IntField("Grade Number", schoolLocation.GradeNumber);
            schoolLocation.ClassNumber = EditorGUILayout.IntField("Class Number", schoolLocation.ClassNumber);
            schoolLocation.RoomType = EditorGUILayout.IntField("Room Type", schoolLocation.RoomType);
        }

        // Hierarchy 0 → 2
        if (schoolLocation.Hierarchy <= 2)
        {
            schoolLocation.ModelPrefab = (GameObject)EditorGUILayout.ObjectField("Model Prefab", schoolLocation.ModelPrefab, typeof(GameObject), false);

            SerializedProperty childLocations = serializedObject.FindProperty("ChildLocation");
            EditorGUILayout.PropertyField(childLocations, true);
        }

        // Hierarchy 1 → 3
        if (schoolLocation.Hierarchy >= 1)
        {
            schoolLocation.ParentLocation = (SchoolLocation)EditorGUILayout.ObjectField("Parent Location", schoolLocation.ParentLocation, typeof(SchoolLocation), false);
        }

        // Hierarchy 2 → 3
        if (schoolLocation.Hierarchy >= 2)
        {
            schoolLocation.Floor = EditorGUILayout.IntField("Floor", schoolLocation.Floor);
        }

        // 座標設定
        EditorGUILayout.LabelField("座標設定", EditorStyles.boldLabel);

        // Hierarchy 1,3
        if (schoolLocation.Hierarchy == 1 || schoolLocation.Hierarchy == 3)
        {
            schoolLocation.isOrdinaryPosition = EditorGUILayout.Toggle("Is Ordinary Position", schoolLocation.isOrdinaryPosition); 

            if (schoolLocation.isOrdinaryPosition)
            {
                schoolLocation.ordinaryPosition_Number = EditorGUILayout.IntField("Ordinary Position Number", schoolLocation.ordinaryPosition_Number);
            }
            else{
            schoolLocation.PositionX_InAll = EditorGUILayout.FloatField("Position X (In All)", schoolLocation.PositionX_InAll);
            schoolLocation.PositionZ_InAll = EditorGUILayout.FloatField("Position Z (In All)", schoolLocation.PositionZ_InAll);
            }
        }

        // Hierarchy 3
        if (schoolLocation.Hierarchy == 3)
        {
            if (!schoolLocation.isOrdinaryPosition)
            {
                schoolLocation.PositionX_InHub = EditorGUILayout.FloatField("Position X (In Hub)", schoolLocation.PositionX_InHub);
                schoolLocation.PositionZ_InHub = EditorGUILayout.FloatField("Position Z (In Hub)", schoolLocation.PositionZ_InHub);
            }
        }

        // 変更を保存
        serializedObject.ApplyModifiedProperties();
    }
}