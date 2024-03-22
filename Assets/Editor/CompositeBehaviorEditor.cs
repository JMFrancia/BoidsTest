// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
//     
// [CustomEditor(typeof(CompositeBehavior))]
// public class CompositeBehaviorEditor : Editor
// {
//     
//     
//     public override void OnInspectorGUI()
//     {
//         //setup
//         var compositeBehavior = (CompositeBehavior)target;
//         
//         //Creates "cursor" for the inspector
//         Rect rect = NewLineRect();
//
//         var allBehaviors = compositeBehavior.Behaviors;
//         
//         //check for behaviors
//         if(allBehaviors == null || allBehaviors.Length == 0)
//         {
//             EditorGUILayout.HelpBox("No behaviors in array", MessageType.Warning);
//             EditorGUILayout.EndHorizontal();
//             rect = NewLineRect();
//             return;
//         }
//
//         //Header
//         rect.x = 30f;
//         rect.width = EditorGUIUtility.currentViewWidth - 95f;
//         EditorGUI.LabelField(rect, "Behaviors");
//         rect.x = EditorGUIUtility.currentViewWidth - 65f;
//         rect.width = 60f;
//         EditorGUI.LabelField(rect, "Weights");
//         rect.y += EditorGUIUtility.singleLineHeight * 1.2f;
//         
//         //Check for any changes in part of editor where behaviors are listed
//         EditorGUI.BeginChangeCheck();
//
//         //List of behaviors
//         for (int n = 0; n < allBehaviors.Length; n++)
//         {
//             rect.x = 5f;
//             rect.width = 20f;
//             EditorGUI.LabelField(rect, n.ToString());
//             var behavior = allBehaviors[n];
//             rect.x = 30f;
//             rect.width = EditorGUIUtility.currentViewWidth - 95f;
//             EditorGUI.ObjectField(rect, behavior.Behavior, typeof(AbstractFlockBehavior), false);
//             rect.x = EditorGUIUtility.currentViewWidth - 65f;
//             rect.width = 60f;
//             behavior.Weight = EditorGUI.FloatField(rect, behavior.Weight); // use serialized property?
//             allBehaviors[n] = behavior;
//             rect.y += EditorGUIUtility.singleLineHeight * 1.1f;
//         }
//
//         //If any changes were detected, set the object as dirty so Unity saves it
//         if (EditorGUI.EndChangeCheck())
//         {
//             EditorUtility.SetDirty(compositeBehavior);
//         }
//
//         //Add behavior button
//         EditorGUILayout.EndHorizontal();
//         rect.x = 5f;
//         rect.width = EditorGUIUtility.currentViewWidth - 10f;
//         rect.y += EditorGUIUtility.singleLineHeight * 0.5f;
//         if (GUILayout.Button("Add Behavior"))
//         {
//             Debug.Log("Button pushed");
//             AddBehavior(compositeBehavior);
//             EditorUtility.SetDirty(compositeBehavior);
//         }
//         rect.y += EditorGUIUtility.singleLineHeight * 1.5f;
//         if (allBehaviors.Length > 0)
//         {
//             if (GUI.Button(rect, "Remove Behavior"))
//             {
//                 RemoveBehavior(compositeBehavior);
//                 EditorUtility.SetDirty(compositeBehavior);
//             }
//         }
//     }
//
//     void AddBehavior(CompositeBehavior cb)
//     {
//         if (cb == null)
//             return;
//         
//         int oldCount = cb.Behaviors?.Length ?? 0;
//         var newBehaviors = new CompositeBehavior.WeightedBehavior[oldCount + 1];
//         for(int n = 0; n < oldCount; n++)
//         {
//             newBehaviors[n] = cb.Behaviors[n];
//         }
//         newBehaviors[oldCount] = new CompositeBehavior.WeightedBehavior(null, 1f);
//         cb.Behaviors = newBehaviors;
//     }
//     
//     void RemoveBehavior(CompositeBehavior cb)
//     {
//         if (cb == null)
//             return;
//         
//         int oldCount = cb.Behaviors.Length;
//         if (oldCount == 1)
//         {
//             cb.Behaviors = null;
//             return;
//         }
//
//         var newBehaviors = new CompositeBehavior.WeightedBehavior[oldCount - 1];
//         for(int n = 0; n < oldCount - 1; n++)
//         {
//             newBehaviors[n] = cb.Behaviors[n];
//         }
//         newBehaviors[oldCount].Behavior = null;
//         cb.Behaviors = newBehaviors;
//     }
//
//     private Rect NewLineRect()
//     {
//         var result = EditorGUILayout.BeginHorizontal();
//         result.height = EditorGUIUtility.singleLineHeight;
//         return result;
//     }
// }
