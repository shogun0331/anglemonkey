using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapTool))]
public class EditorMaptool : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapTool map = (MapTool)target;

        if (GUILayout.Button("Load Resources Objects "))
        {
            map.LoadResources();
        }


        if (GUILayout.Button("Load MapLevel "))
        {
            map.PaserMapLevel();
        }





    }

}
