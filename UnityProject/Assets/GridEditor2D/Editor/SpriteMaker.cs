using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using System.IO;
using System.Linq;

namespace GridEditor2D
{
    public class SpriteMaker : EditorWindow
    {

        Grid grid;
        private string[] files;
        private string[] options;
        private Sprite[] allSprites;
        public GUIStyle textureStyle;
        private Vector2 scrollPos = Vector2.zero;
        private bool intalized = false;
        private int gwidth = 2;
        private int gheight = 1;
        private bool erase = false;
        private List<Sprite> texList = new List<Sprite>();
        private string spritename = "sprite";
        private bool pointScale = true;
        Sprite reference;
        Texture2D refTex;

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


                if (!Directory.Exists(Application.dataPath + "/GridEditor2D/Sprite Maker/"))
                {
                    AssetDatabase.CreateFolder("Assets/GridEditor2D", "Sprite Maker");
                    AssetDatabase.Refresh();
                    Debug.Log("Created Sprite Maker Directory");
                }


                if (!intalized)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Reference Sprite:", GUILayout.Width(110));
                    reference = (Sprite)EditorGUILayout.ObjectField(reference, typeof(Sprite), true);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Height:", GUILayout.Width(110));
                    gheight = EditorGUILayout.IntField(gheight);
                    if (gheight < 1) gheight = 1;
                    if (100 < gheight) gheight = 100;
                    if (gheight % 2 == 0) gheight++;
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Width:", GUILayout.Width(110));
                    gwidth = EditorGUILayout.IntField(gwidth);
                    if (gwidth < 1) gwidth = 1;
                    if (100 < gwidth) gwidth = 100;
                    if (gwidth % 2 == 0) gwidth++;
                    GUILayout.EndHorizontal();
                    if (GUILayout.Button("Generate"))
                    {
                        if (reference != null)
                        {
                            refTex = TextureFromSprite(reference);
                            int rows = gheight * gwidth;
                            texList = new List<Sprite>();
                            for (int i = 0; i < rows; i++)
                                texList.Add(null);
                            intalized = true;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Sprite Maker", "Please select a reference sprite!", "Okay");
                        }
                    }


                }
                else
                {

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();

                    if (GUILayout.Button(new GUIContent(BrushIcon, "Draw Mode"), GUILayout.Width(80)))
                    {
                        erase = false;
                    }
                    if (GUILayout.Button(new GUIContent(EraserIcon, "Erase"), GUILayout.Width(80)))
                    {
                        erase = true;
                    }
                    pointScale = GUILayout.Toggle(pointScale, "Point Scale", GUILayout.Width(80));
                    EditorGUILayout.EndVertical();

                    GUILayout.Button("", GUIStyle.none, GUILayout.Width(refTex.width + 6), GUILayout.Height(refTex.height + 2));

                    if (grid.selectedObj != null)
                    {
                        Sprite t = null;
                        if (grid.selectedObj is Sprite)
                        {
                            t = (Sprite)grid.selectedObj;
                            if (t != null)
                            {
                                GUI.DrawTextureWithTexCoords(new Rect(GUILayoutUtility.GetLastRect().x,
                                                                  GUILayoutUtility.GetLastRect().y,
                                                                  GUILayoutUtility.GetLastRect().width,
                                                                  GUILayoutUtility.GetLastRect().height),
                                                         t.texture,
                                                         new Rect(t.textureRect.x / (float)t.texture.width,
                                     t.textureRect.y / (float)t.texture.height,
                                     t.textureRect.width / (float)t.texture.width,
                                     t.textureRect.height / (float)t.texture.height));
                            }
                        }
                    }

                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                    EditorGUILayout.EndHorizontal();

                    int ctr = 0;
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                    GUILayout.BeginHorizontal();
                    for (int i = 0; i < texList.Count; i++)
                    {

                        if (ctr == gwidth)
                        {
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            ctr = 0;
                        }
                        ctr++;

                        if (GUILayout.Button("", GUIStyle.none, GUILayout.Width(refTex.width + 6), GUILayout.Height(refTex.height + 2)))
                        {
                            if (erase)
                            {
                                texList[i] = null;
                            }
                            else
                            {
                                if (grid.selectedObj is Sprite)
                                    if ((Sprite)grid.selectedObj)
                                        texList[i] = (Sprite)grid.selectedObj;
                            }
                        }

                        if (texList[i] != null)
                        {
                            GUI.DrawTextureWithTexCoords(new Rect(GUILayoutUtility.GetLastRect().x,
                                                              GUILayoutUtility.GetLastRect().y,
                                                              GUILayoutUtility.GetLastRect().width,
                                                              GUILayoutUtility.GetLastRect().height),
                                                     texList[i].texture,
                                                     new Rect(texList[i].textureRect.x / (float)texList[i].texture.width,
                                 texList[i].textureRect.y / (float)texList[i].texture.height,
                                 texList[i].textureRect.width / (float)texList[i].texture.width,
                                 texList[i].textureRect.height / (float)texList[i].texture.height));
                        }
                        else
                        {
                            GUI.DrawTextureWithTexCoords(new Rect(GUILayoutUtility.GetLastRect().x,
                                                           GUILayoutUtility.GetLastRect().y,
                                                           GUILayoutUtility.GetLastRect().width,
                                                           GUILayoutUtility.GetLastRect().height),
                                                  Empty,
                                                  new Rect(reference.textureRect.x / (float)reference.texture.width,
                              reference.textureRect.y / (float)reference.texture.height,
                              reference.textureRect.width / (float)reference.texture.width,
                              reference.textureRect.height / (float)reference.texture.height));
                        }

                    }

                    GUILayout.EndHorizontal();
                    EditorGUILayout.EndScrollView();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Sprite Name:", GUILayout.Width(100));
                    spritename = EditorGUILayout.TextField(spritename);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Generate Sprite"))
                    {
                        if (spritename != "")
                            MakeImage();
                        else
                            EditorUtility.DisplayDialog("Sprite Maker", "Please name your Sprite!", "Okay");
                    }
                    EditorGUILayout.EndHorizontal();
                    if (GUILayout.Button("Delete"))
                    {
                        if (!EditorUtility.DisplayDialog("Sprite Maker", "Do you really want to delete the sprite?", "Yes", "No"))
                        {
                        }
                        else
                        {
                            intalized = false;
                            return;
                        }
                    }
                }
                SceneView.RepaintAll();
                EditorUtility.SetDirty(grid);
            }
        }

        private void MakeImage()
        {

            Texture2D atlas;
            int textureWidthCounter = 0;
            int textureHeightCounter = 0;
            int width, height;
            Texture2D refT = TextureFromSprite(reference);
            width = refT.width * gwidth;
            height = refT.height * gheight;

            // make your new texture
            atlas = new Texture2D(width, height, TextureFormat.RGBA32, false);
            atlas.wrapMode = TextureWrapMode.Clamp;

            int cur = 0;
            // loop through your textures
            textureHeightCounter = height - refTex.height;
            for (int i = 0; i < gheight; i++)
            {

                for (int x = 0; x < gwidth; x++)
                {
                    Texture2D tex = null;

                    if (texList[cur] != null)
                    {

                        tex = TextureFromSprite(texList[cur], refT.width, refT.height);
                        Color[] p = tex.GetPixels();
                        atlas.SetPixels(textureWidthCounter, textureHeightCounter, tex.width, tex.height, p);
                        atlas.Apply();
                    }
                    else
                    {
                        int y = 0;
                        Color color = new Color(0, 0, 0, 0);
                        while (y < refT.height)
                        {
                            int xx = 0;
                            while (xx < refT.width)
                            {
                                atlas.SetPixel(textureWidthCounter + xx, textureHeightCounter + y, color);
                                ++xx;
                            }
                            ++y;
                        }
                        atlas.Apply();
                    }
                    textureWidthCounter += refT.width;
                    cur++;
                }
                textureHeightCounter -= refT.height;
                textureWidthCounter = 0;


            }
            byte[] bytes = atlas.EncodeToPNG();

            if (!File.Exists((Application.dataPath + "/GridEditor2D/Sprite Maker/" + spritename + ".png")))
            {
                File.WriteAllBytes(Application.dataPath + "/GridEditor2D/Sprite Maker/" + spritename + ".png", bytes);
            }
            else
            {
                if (EditorUtility.DisplayDialog("Sprite Maker", "File already exists, do you want to override it?", "Yes", "No"))
                {
                    File.WriteAllBytes(Application.dataPath + "/GridEditor2D/Sprite Maker/" + spritename + ".png", bytes);
                }

            }
            AssetDatabase.Refresh();
        }


        public Texture2D TextureFromSprite(Sprite t, int w, int h)
        {
            Texture2D newText = TextureFromSprite(t);
            if (pointScale)
                TextureScale.Point(newText, w, h);
            else
                TextureScale.Bilinear(newText, w, h);

            return newText;
        }

        public Texture2D TextureFromSprite(Sprite t)
        {

            if (t.rect.width != t.texture.width)
            {
                Color[] newColors = t.texture.GetPixels((int)t.textureRect.x,
                                                 (int)t.textureRect.y,
                                                 (int)t.textureRect.width,
                                                 (int)t.textureRect.height);

                Texture2D newText = new Texture2D((int)t.textureRect.width, (int)t.textureRect.height);

                newText.SetPixels(newColors);
                newText.Apply();
                return newText;

            }
            else
            {
                return t.texture;
            }
        }


        private Texture2D brushIcon;
        public Texture2D BrushIcon
        {
            get
            {
                if ((UnityEngine.Object)brushIcon == (UnityEngine.Object)null)
                    brushIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "toolBrush.png", typeof(Texture2D)) as Texture2D;

                return brushIcon;
            }
        }

        private Texture2D empty;
        public Texture2D Empty
        {
            get
            {
                if ((UnityEngine.Object)empty == (UnityEngine.Object)null)
                    empty = AssetDatabase.LoadAssetAtPath(IconsPath + "editor.png", typeof(Texture2D)) as Texture2D;

                return empty;
            }
        }

        private Texture2D eraserIcon;
        public Texture2D EraserIcon
        {
            get
            {
                if ((UnityEngine.Object)eraserIcon == (UnityEngine.Object)null)
                    eraserIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "toolEraser.png", typeof(Texture2D)) as Texture2D;

                return eraserIcon;
            }
        }

        private string iconsPath;
        private string IconsPath
        {
            get
            {
                if (string.IsNullOrEmpty(iconsPath))
                {
                    iconsPath = "Assets/GridEditor2D/Icons/";
                }

                return iconsPath;
            }
        }
    }
}