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

         // posX = rect.x;
         // posY = rect.y + EditorGUIUtility.singleLineHeight + 5;
         // width = 40;

         // EditorGUI.LabelField(new Rect(posX, posY, width, height), "Type:");

         // posX += width;
         // width = 70;

         // EditorGUI.PropertyField
         // (
         //     new Rect(posX, posY, width, height),
         //     element.FindPropertyRelative("dataType"), GUIContent.none
         // );

         // posX += width;
         // width = 45;

         // EditorGUI.LabelField(new Rect(posX, posY, width, height), "Value:");

         // posX += width;
         // width = 50;

         // EditorGUI.PropertyField
         // (
         //     new Rect(posX, posY, width, height),
         //     element.FindPropertyRelative("valueOfEachdata"), GUIContent.none
         // );

         //     //--------------------------------------------------------------------------------------
         //     //LEADER BOARD
         //     //--------------------------------------------------------------------------------------

         //     posY += EditorGUIUtility.singleLineHeight + 5;
         // posX = rect.x;

         // width = 80;

         // EditorGUI.LabelField(new Rect(posX, posY, width, height), "LeaderBoard");

         // posX += width;
         // width = 15;

         // EditorGUI.PropertyField
         //(
         //    new Rect(posX, posY, width, height),
         //    element.FindPropertyRelative("haveLeaderboard"), GUIContent.none
         //);

         // posX += width + 20;
         // width = 30;

         // EditorGUI.LabelField(new Rect(posX, posY, width, height), "Tag:");

         // posX += width;
         // width = 60;

         // EditorGUI.TagField(new Rect(posX, posY, width, height), element.FindPropertyRelative("leaderboardTag").stringValue);

         //     //--------------------------------------------------------------------------------------
         //     //Achievments and score
         //     //--------------------------------------------------------------------------------------


         //     posY += EditorGUIUtility.singleLineHeight + 5;
         // posX = rect.x;
         // width = 45;

         // EditorGUI.LabelField(new Rect(posX, posY, width, height), "Rules:");

         // posX += width;
         // width = 60;

         // EditorGUI.PropertyField
         //(
         //    new Rect(posX, posY, width, height),
         //    element.FindPropertyRelative("achievementRules"), GUIContent.none
         //);

         // posX += width;
         // width = 60;

         // EditorGUI.PropertyField
         //(
         //    new Rect(posX, posY, width, height),
         //    element.FindPropertyRelative("achievementRules"), GUIContent.none
         //);

     };
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("First part is the style of the enemy. \nSecond is the time that it will span after the last enemy spwaned.", MessageType.Info);
        serializedObject.Update();
        list.DoLayoutList();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceToBeAffected"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("typeGeneration"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("typeDetection"), true);

        serializedObject.ApplyModifiedProperties();

    }
}
