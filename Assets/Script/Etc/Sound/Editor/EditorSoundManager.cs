#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundManager))]
public class EditorSoundManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SoundManager manager = (SoundManager)target;

        if (GUILayout.Button("Load Sound"))
        {
            manager.LoadSound();
        }
    }
}
#endif