using UnityEngine;
using UnityEditor;
using System.Collections;

namespace GridEditor2D
{
    public class GridSettings : EditorWindow
    {

        Grid grid;
        Sprite go;

        public void Init()
        {
            grid = (Grid)FindObjectOfType(typeof(Grid));

        }

        void OnGUI()
        {
            Init();
            if (!grid) return;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Color:", GUILayout.Width(100));
            grid.color = EditorGUILayout.ColorField(grid.color, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Grid Width ");
            grid.width = EditorGUILayout.FloatField(grid.width, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Grid Height ");
            grid.height = EditorGUILayout.FloatField(grid.height, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Auto adjust grid size by dropping a sprite");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            go = (Sprite)EditorGUILayout.ObjectField(go, typeof(Sprite), true);
            if (go != null)
                GridSizeBySprite();
            GUILayout.EndHorizontal();

            EditorUtility.SetDirty(grid);
            SceneView.RepaintAll();
        }

        void GridSizeBySprite()
        {
            if (0 < go.bounds.size.x && 0 < go.bounds.size.y)
            {
                grid.width = go.bounds.size.x;
                grid.height = go.bounds.size.y;
            }
        }
    }
}