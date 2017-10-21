using UnityEngine;
using UnityEditor;
using System.Collections;

namespace GridEditor2D
{
    [CustomEditor(typeof(LevelEditorLayer))]
    public class LayerLock : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LevelEditorLayer myScript = (LevelEditorLayer)target;

            if (GUILayout.Button("Apply Sorting Layer"))
            {
                myScript.SetSortingLayer();
            }
            if (myScript.locked)
            {
                if (GUILayout.Button("Unlock Layer"))
                {
                    myScript.Unlock();
                }
            }
            else
            {
                if (GUILayout.Button("Lock Layer"))
                {
                    myScript.Lock();
                }
            }
        }
    }
}
