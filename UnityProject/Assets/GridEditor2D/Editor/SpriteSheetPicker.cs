using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using System.IO;
using System.Linq;

namespace GridEditor2D
{
    public class SpriteSheetPicker : EditorWindow
    {

        Grid grid;
        private string[] files;
        private string[] options;
        private Sprite[] allSprites;
        public GUIStyle textureStyle;
        public GUIStyle textureStyleAct;
        private Vector2 scrollPos = Vector2.zero;

        public static void ShowWindow()
        {
            SpriteSheetPicker window = (SpriteSheetPicker)EditorWindow.GetWindow(typeof(SpriteSheetPicker), false, "SpriteSheetPicker");
            window.minSize = new Vector2(100, 100);
        }

        public void Init()
        {
            grid = (Grid)FindObjectOfType(typeof(Grid));
        }

        void OnGUI()
        {

            Init();
            if (grid)
            {

                textureStyle = new GUIStyle(GUI.skin.button);
                textureStyle.margin = new RectOffset(2, 2, 2, 2);
                textureStyle.normal.background = null;
                textureStyleAct = new GUIStyle(textureStyle);
                textureStyleAct.margin = new RectOffset(0, 0, 0, 0);
                textureStyleAct.normal.background = textureStyle.active.background;

                if (!Directory.Exists(Application.dataPath + "/GridEditor2D/Sprite Sheets/"))
                {
                    //Directory.CreateDirectory(Application.dataPath + "/spritemaps/");
                    AssetDatabase.CreateFolder("Assets/GridEditor2D", "Sprite Sheets");
                    AssetDatabase.Refresh();
                    Debug.Log("Created Sprite Sheets Directory");
                }

                files = Directory.GetFiles(Application.dataPath + "/GridEditor2D/Sprite Sheets/", "*.png");
                options = new string[files.Length];


                for (int i = 0; i < files.Length; i++)
                {
                    options[i] = files[i].Replace(Application.dataPath + "/GridEditor2D/Sprite Sheets/", "");
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Sprite Sheet", GUILayout.Width(90));
                grid.spriteMapIndex = EditorGUILayout.Popup(grid.spriteMapIndex, options, GUILayout.Width(175));
                EditorGUILayout.LabelField("Zoom:", GUILayout.Width(50));
                grid.spriteMapZoomFactor = (int)EditorGUILayout.Slider(grid.spriteMapZoomFactor, 1f, 10f);
                GUILayout.EndHorizontal();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                GUILayout.BeginHorizontal();
                float ctr = 0.0f;
                if (options.Length > grid.spriteMapIndex)
                {
                    allSprites = AssetDatabase.LoadAllAssetsAtPath("Assets/GridEditor2D/Sprite Sheets/" + options[grid.spriteMapIndex]).Select(x => x as Sprite).Where(x => x != null).ToArray();
                    foreach (Sprite singsprite in allSprites)
                    {
                        if (ctr > singsprite.textureRect.x)
                        {
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                        }
                        ctr = singsprite.textureRect.x;
                        if (grid.selectedObj == singsprite)
                        {
                            GUILayout.Button("", textureStyleAct, GUILayout.Width((singsprite.textureRect.width + 6) * grid.spriteMapZoomFactor), GUILayout.Height((singsprite.textureRect.height + 4) * grid.spriteMapZoomFactor));
                            GUI.DrawTextureWithTexCoords(new Rect(GUILayoutUtility.GetLastRect().x + 3f,
                                                                  GUILayoutUtility.GetLastRect().y + 2f,
                                                                  GUILayoutUtility.GetLastRect().width - 6f,
                                                                  GUILayoutUtility.GetLastRect().height - 4f),
                                                         singsprite.texture,
                                                         new Rect(singsprite.textureRect.x / (float)singsprite.texture.width,
                                     singsprite.textureRect.y / (float)singsprite.texture.height,
                                     singsprite.textureRect.width / (float)singsprite.texture.width,
                                     singsprite.textureRect.height / (float)singsprite.texture.height));
                        }
                        else
                        {
                            if (GUILayout.Button("", textureStyle, GUILayout.Width((singsprite.textureRect.width + 6) * grid.spriteMapZoomFactor), GUILayout.Height((singsprite.textureRect.height + 2) * grid.spriteMapZoomFactor)))
                            {
                                grid.selectedObj = singsprite;
                                grid.tileMap = true;
                            }

                            GUI.DrawTextureWithTexCoords(GUILayoutUtility.GetLastRect(), singsprite.texture,
                                                         new Rect(singsprite.textureRect.x / (float)singsprite.texture.width,
                                                                     singsprite.textureRect.y / (float)singsprite.texture.height,
                                                                     singsprite.textureRect.width / (float)singsprite.texture.width,
                                                                     singsprite.textureRect.height / (float)singsprite.texture.height));
                        }
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
                SceneView.RepaintAll();
                EditorUtility.SetDirty(grid);
            }
        }
    }
}