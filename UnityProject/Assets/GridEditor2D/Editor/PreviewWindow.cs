using UnityEngine;
using UnityEditor;
using System.Collections;

namespace GridEditor2D
{
    public class PreviewWindow : Editor
    {
        private Texture2D background;
        private string tag = "";
        private int layer = 0;
        private bool addCollider = false;
        private bool addPolycollider = false;
        private bool flipHorizontally = false;
        private bool flipVertically = false;
        private bool addRigibody = false;
        private bool optimize = false;
        private bool scaleToGrid = false;
        private object oldGo;
        private float alpha = 1;
        private string objName = "";
        GUIStyle textureStyle;

        public void OnScene(SceneView sceneView, Grid grid)
        {

            if (!grid || !grid.selectedObj)
                return;

            if (Event.current.type == EventType.Repaint)
                return;

            Handles.BeginGUI();
            GUILayout.Window(4, new Rect(Screen.width - 220, Screen.height - 350, 200, 220), (id) =>
            {

                bool isGo = false;

                if (grid.selectedObj is GameObject)
                    isGo = true;

                if (oldGo != grid.selectedObj)
                {

                    tag = "";
                    objName = grid.selectedObj.name;
                    layer = 0;
                    addCollider = false;

                    if (!grid.ScaleToGridAsDefault)
                        scaleToGrid = false;
                    else
                        scaleToGrid = true;

                    flipHorizontally = false;
                    flipVertically = false;
                    addRigibody = false;
                    optimize = false;
                    addCollider = false;
                    addPolycollider = false;
                    alpha = 1;

                    if (grid.OptimizeColliderAsDefault)
                        optimize = true;
                    else
                        optimize = false;

                    if (isGo)
                    {
                        GameObject temp = (GameObject)grid.selectedObj;
                        if (temp.GetComponent<BoxCollider2D>())
                            addCollider = true;
                        if (temp.GetComponent<Rigidbody2D>())
                            addRigibody = true;
                        tag = temp.tag;
                        layer = temp.layer;
                        optimize = false;
                        if (temp.GetComponent<PolygonCollider2D>())
                            addPolycollider = true;
                        if (temp.GetComponent<SpriteRenderer>())
                            alpha = temp.GetComponent<SpriteRenderer>().color.a;
                    }
                }

                if (optimize)
                {
                    addRigibody = false;
                    addPolycollider = false;
                }

                if (addCollider && addPolycollider)
                    addPolycollider = false;

                EditorGUILayout.BeginHorizontal();
                objName = EditorGUILayout.TextField(objName);//, EditorStyles.whiteLabel);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (isGo)
                    EditorGUILayout.LabelField("Type: Prefab", EditorStyles.whiteLabel);
                else
                    EditorGUILayout.LabelField("Type: Sprite", EditorStyles.whiteLabel);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                Texture2D prefabIcon2 = null;
                if (isGo)
                {
                    prefabIcon2 = AssetPreview.GetAssetPreview(grid.selectedObj);
                    if (prefabIcon2 == null)
                        GUILayout.Button(EmptyIcon, GUILayout.Height(80), GUILayout.Width(200));
                    else
                        GUILayout.Button(prefabIcon2, GUILayout.Height(80), GUILayout.Width(200));
                }
                else
                {

                    Sprite t = (Sprite)grid.selectedObj;
                    float height = t.textureRect.height;
                    float width = t.textureRect.width;

                    if (80 < height)
                    {
                        width = (width / height) * 80;
                        height = 80;
                    }

                    if (200 < width)
                    {
                        height = (height / width) * 200;
                        width = 200;
                    }

                    textureStyle = new GUIStyle(GUI.skin.button);
                    textureStyle.margin = new RectOffset(2, 2, 2, 2);
                    textureStyle.normal.background = null;

                    GUILayout.Button("", textureStyle, GUILayout.Width(width), GUILayout.Height(height));
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
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Add Box Collider 2D", EditorStyles.whiteLabel, GUILayout.Width(180));
                addCollider = EditorGUILayout.Toggle(addCollider);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Add Polycon Collider 2D", EditorStyles.whiteLabel, GUILayout.Width(180));
                addPolycollider = EditorGUILayout.Toggle(addPolycollider);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Optimize Collider", EditorStyles.whiteLabel, GUILayout.Width(180));
                optimize = EditorGUILayout.Toggle(optimize);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Add Rigidbody2D", EditorStyles.whiteLabel, GUILayout.Width(180));
                addRigibody = EditorGUILayout.Toggle(addRigibody);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Flip Horizontally", EditorStyles.whiteLabel, GUILayout.Width(180));
                flipHorizontally = EditorGUILayout.Toggle(flipHorizontally);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Flip Vertically", EditorStyles.whiteLabel, GUILayout.Width(180));
                flipVertically = EditorGUILayout.Toggle(flipVertically);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scale To Grid", EditorStyles.whiteLabel, GUILayout.Width(180));
                scaleToGrid = EditorGUILayout.Toggle(scaleToGrid);
                EditorGUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Tag", EditorStyles.whiteLabel, GUILayout.Width(60));
                tag = EditorGUILayout.TagField(tag, GUILayout.Width(140));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Layer", EditorStyles.whiteLabel, GUILayout.Width(60));
                layer = EditorGUILayout.LayerField(layer, GUILayout.Width(140));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Alpha", EditorStyles.whiteLabel, GUILayout.Width(60));
                alpha = EditorGUILayout.Slider(alpha, 0, 1, GUILayout.Width(140));
                GUILayout.EndHorizontal();

                oldGo = grid.selectedObj;

                GUI.DragWindow();

            }, "GridEditor2D Preview");

            Handles.EndGUI();
        }

        public void CreatedGO(GameObject go, Grid grid, bool isGO)
        {
            Apply(grid, go);
        }


        void Apply(Grid grid, GameObject go)
        {

            if (tag != "")
                go.tag = tag;

            go.layer = layer;
            go.name = objName;

            if (go.GetComponent<SpriteRenderer>())
            {
                SpriteRenderer r = go.GetComponent<SpriteRenderer>();
                r.color = new Color(r.color.r, r.color.g, r.color.b, alpha);

                if (scaleToGrid)
                {
                    Vector3 scale = go.transform.localScale;
                    scale.y = grid.height * scale.y / go.GetComponent<SpriteRenderer>().bounds.size.y;
                    scale.x = grid.width * scale.x / go.GetComponent<SpriteRenderer>().bounds.size.x;
                    go.transform.localScale = scale;
                }
            }

            if (addRigibody) optimize = false;

            if (addCollider)
            {
                if (!go.GetComponent<BoxCollider2D>())
                {
                    go.AddComponent<BoxCollider2D>();
                }
                if (optimize && grid.optimized)
                {
                    if (!go.GetComponent<TileManager>())
                        go.AddComponent<TileManager>();
                    go.GetComponent<TileManager>().addCollider = true;
                }
            }
            else
            {
                if (go.GetComponent<BoxCollider2D>())
                    DestroyImmediate(go.GetComponent<BoxCollider2D>());
            }

            if (optimize && !addCollider || addPolycollider)
                if (go.GetComponent<TileManager>())
                    DestroyImmediate(go.GetComponent<TileManager>());

            if (addPolycollider)
            {
                if (!go.GetComponent<PolygonCollider2D>())
                {
                    go.AddComponent<PolygonCollider2D>();
                }
            }
            else
            {
                if (go.GetComponent<PolygonCollider2D>())
                    DestroyImmediate(go.GetComponent<PolygonCollider2D>());
            }


            if (addRigibody && !optimize)
            {
                if (!go.GetComponent<Rigidbody2D>())
                    go.AddComponent<Rigidbody2D>();
            }
            else
            {
                if (go.GetComponent<Rigidbody2D>())
                    DestroyImmediate(go.GetComponent<Rigidbody2D>());
            }

            if (flipVertically)
            {
                if (go.transform.localEulerAngles.y != 180)
                    go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x, 180, 0);
                else
                    go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x, 0, 0);
            }

            if (flipHorizontally)
            {
                if (go.transform.localEulerAngles.x != 180)
                    go.transform.localEulerAngles = new Vector3(180, go.transform.localEulerAngles.y, 0);
                else
                    go.transform.localEulerAngles = new Vector3(0, go.transform.localEulerAngles.y, 0);

            }

        }


        private Texture2D emptyIcon;
        public Texture2D EmptyIcon
        {
            get
            {
                if ((UnityEngine.Object)emptyIcon == (UnityEngine.Object)null)
                    emptyIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "empty.png", typeof(Texture2D)) as Texture2D;

                return emptyIcon;
            }
        }

        private string iconsPath;
        private string IconsPath
        {
            get
            {
                if (string.IsNullOrEmpty(iconsPath))
                {

                    string path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
                    path = path.Substring(0, path.LastIndexOf('/') + 1);
                    iconsPath = path + "Icons/";

                }
                return iconsPath;
            }
        }

    }
}
