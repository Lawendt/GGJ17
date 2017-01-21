using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : Editor
{
    ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("enemy"),
                true, true, true, true);

        list.elementHeight = EditorGUIUtility.singleLineHeight;

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Enemies");
        };

        float posX, posY, width, height;

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
     {
         posX = rect.x;
         posY = rect.y;
         width = 100;
         height = EditorGUIUtility.singleLineHeight;

         var element = list.serializedProperty.GetArrayElementAtIndex(index);

         EditorGUI.PropertyField
       (
           new Rect(posX, posY, width, height),
           element.FindPropertyRelative("type"), GUIContent.none
       );

         posX += width + 10;
         //posY += height;
         width = 100;

         EditorGUI.PropertyField
         (
             new Rect(posX, posY, width, height),
             element.FindPropertyRelative("time"), GUIContent.none
         );



     };
    }

    public override void OnInspectorGUI()
    {
        EnemyManager.TypeGeneration typeGen = ((EnemyManager)target).typeGeneration;
        if (typeGen == EnemyManager.TypeGeneration.readFromList)
            EditorGUILayout.HelpBox("First part is the style of the enemy. \nSecond is the time that it will span after the last enemy spwaned.", MessageType.Info);
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("typeGeneration"), true);
        if (typeGen == EnemyManager.TypeGeneration.readFromList)
            list.DoLayoutList();
        if (typeGen == EnemyManager.TypeGeneration.random)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minRandom"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxRandom"), true);
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceToBeAffected"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("typeDetection"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("timetoWaitToEnjoy"), true);

        serializedObject.ApplyModifiedProperties();

    }
}
