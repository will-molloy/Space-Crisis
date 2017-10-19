using UnityEditor;
using UnityEngine;
using System.Collections;

namespace GridEditor2D
{
    public class LevelEditorMenu : Editor
    {
        public static GridWindow f;
        public static Grid grid;
        [MenuItem("Window/GridEditor2D/Editor")]
        public static void ShowEditor()
        {
            if (f == null)
            {
                f = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow), false, "GridEditor2D");
                f.Show();
                f.minSize = new Vector2(220f, 400f);
                f.Init();
            }
            else
            {
                if (f.grid)
                {
                    if (!f.grid.DrawEditorInSceneGUI)
                        f.Show();
                }
            }
        }

        public static void HideEditor()
        {
            if (f != null)
            {
                f = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow), false, "GridEditor2D");
                f.Close();
                DestroyImmediate(f);
                f = (GridWindow)ScriptableObject.CreateInstance(typeof(GridWindow));
                f.Init();
            }
        }

        public static void HideSceneEditor()
        {
            if (f != null)
            {
                DestroyImmediate(f);
                ShowEditor();
            }
        }
        [MenuItem("Window/GridEditor2D/Sprite Sheet Picker")]
        public static void ShowPicker()
        {
            SpriteSheetPicker window = (SpriteSheetPicker)EditorWindow.GetWindow(typeof(SpriteSheetPicker), false, "SpriteSheetPicker");
            window.minSize = new Vector2(100, 100);
        }
        [MenuItem("Window/GridEditor2D/Sprite Maker")]
        public static void ShowMaker()
        {
            SpriteMaker window = (SpriteMaker)EditorWindow.GetWindow(typeof(SpriteMaker), false, "SpriteMaker");
            window.minSize = new Vector2(100, 100);
        }

        [MenuItem("Window/GridEditor2D/Grid Settings")]
        public static void ShowSettings()
        {
            GridSettings window = (GridSettings)EditorWindow.GetWindow(typeof(GridSettings), false, "Grid Settings");
            window.minSize = new Vector2(310f, 100f);
        }

        [MenuItem("Window/GridEditor2D/Manual")]
        public static void OpenManual()
        {
            Application.OpenURL("https://docs.google.com/document/d/1QwIoZ8N_BU9oJ1qCEJASHn203qJ6fcbKWEwLC53pAqc/edit");
        }
    }
}