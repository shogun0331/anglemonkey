using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalizingManager))]
public class EditorLocalizingManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LocalizingManager manager = (LocalizingManager)target;

        if (GUILayout.Button("Load JSON"))
        {
            manager.PaserData();
        }

    }
 }
