using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlockAgent))]
public class FlockAgentEditor : Editor
{
   public override void OnInspectorGUI()
   {
      DrawDefaultInspector();
      FlockAgent agent = (FlockAgent)target;

      if (agent == null)
          return;

      EditorGUILayout.LabelField("Flock", agent?.Flock?.FlockName ?? "None");
      EditorGUILayout.LabelField("Behaviors");
      foreach (var behavior in agent.Flock.AllBehaviors)
      {
          EditorGUILayout.LabelField(behavior.Behavior.name, behavior.Weight.ToString());
          if(behavior.Behavior is AbstractFilteredFlockBehavior filteredBehavior)
          {
              EditorGUILayout.LabelField("Filter:", filteredBehavior._filter?.name ?? "None");
          }
      }
   }

   private Rect NewLineRect()
     {
         var result = EditorGUILayout.BeginHorizontal();
         result.height = EditorGUIUtility.singleLineHeight;
         return result;
     }
}
