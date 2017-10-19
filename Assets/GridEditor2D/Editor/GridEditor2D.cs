using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using System.IO;
using System.Linq;

namespace GridEditor2D
{
    public class GridWindow : EditorWindow
    {
        public Grid grid;

        private Vector2 scrollPos = Vector2.zero;
        private PreviewWindow preview;
        private SelectedWindow selectWindow;
        private GameObject activeGo;
        private string[] files;
        private string[] options;
        private Sprite[] allSprites;

        public bool isPlay = false;
        public GUIStyle textureStyle;
        public GUIStyle textureStyleAct;
        public Rect windowRect = new Rect(5, 20, 220, 600);

        public void Init()
        {
            grid = (Grid)FindObjectOfType(typeof(Grid));

            if (SceneView.onSceneGUIDelegate != GridUpdate)
                SceneView.onSceneGUIDelegate += GridUpdate;

            EditorApplication.playmodeStateChanged = PlaymodeStateChanged;
        }

        void OnDestroy()
        {
      
            if (SceneView.onSceneGUIDelegate == GridUpdate)
                SceneView.onSceneGUIDelegate -= this.GridUpdate;
        }

        void PlaymodeStateChanged()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                if (!grid)
                    return;
                grid.gridEnabled = false;
                Repaint();
                return;
            }

        }

        void HotKeySwitchPrefab(int up)
        {
            List<Object> pList = PrefabGroup(grid.currentPrefabList);
            int cur = pList.FindIndex(o => o.name == grid.selectedObj.name);
            if (up == 1)
            {
                if (cur == pList.Count - 1)
                    cur = 0;
                else
                    cur++;
            }
            else
            {
                if (cur == 0)
                    cur = pList.Count - 1;
                else
                    cur--;
            }
            grid.selectedObj = pList[cur];
        }

        //Hotkey Check
        void HotKeyCheck()
        {
            Event hotkey_e = Event.current;
            switch (hotkey_e.type)
            {
                case EventType.KeyDown:
                    if (hotkey_e.shift)
                    {

                        switch (hotkey_e.keyCode)
                        {
                            case KeyCode.UpArrow:
                                HotKeySwitchPrefab(1);
                                hotkey_e.Use();
                                break;
                            case KeyCode.DownArrow:
                                HotKeySwitchPrefab(0);
                                hotkey_e.Use();
                                break;
                            case KeyCode.P:
                                grid.selected = Grid.DRAWOPTION.paint;
                                if (!grid.DrawEditorInSceneGUI)
                                    Repaint();
                                break;
                            case KeyCode.E:
                                grid.selected = Grid.DRAWOPTION.erase;
                                if (!grid.DrawEditorInSceneGUI)
                                    Repaint();
                                break;
                            case KeyCode.S:
                                grid.selected = Grid.DRAWOPTION.select;
                                if (!grid.DrawEditorInSceneGUI)
                                    Repaint();
                                break;
                            case KeyCode.D:
                                grid.gridEnabled = !grid.gridEnabled;
                                if (!grid.DrawEditorInSceneGUI)
                                    Repaint();
                                hotkey_e.Use();
                                break;
                            case KeyCode.G:
                                grid.showGrid = !grid.showGrid;
                                if (!grid.DrawEditorInSceneGUI)
                                    Repaint();
                                break;
                            case KeyCode.T:
                                SpriteSheetPicker.ShowWindow();
                                break;
                            case KeyCode.H:
                                HideLayerHelper(!grid.hideOtherLayer);
                                if (!grid.DrawEditorInSceneGUI)
                                    Repaint();
                                break;
                            case KeyCode.L:
                                SwitchLayer(false);
                                if (!grid.DrawEditorInSceneGUI)
                                    Repaint();
                                break;
                            case KeyCode.K:
                                SwitchLayer(true);
                                if (!grid.DrawEditorInSceneGUI)
                                    Repaint();
                                break;
                        }
                    }
                    break;
            }
        }


        void GridUpdate(SceneView sceneview)
        {
            HotKeyCheck();
            if (grid)
            {
                if (grid.editor == null)
                {
                    GameObject newgo = new GameObject("Editor");
                    newgo.transform.position = Vector3.zero;
                    newgo.transform.parent = grid.transform;
                    grid.editor = newgo.transform;

                }
                if (grid.DrawEditorInSceneGUI)
                {
                    Handles.BeginGUI();
                    windowRect = GUILayout.Window(1, windowRect, DoWindow, "GridEditor2D");
                    Handles.EndGUI();
                }
            }


            if (!grid || !grid.gridEnabled)
                return;

            if (grid.selected == Grid.DRAWOPTION.select)
            {
                if (selectWindow)
                    selectWindow.OnScene(sceneview, grid);
                else
                    selectWindow = (SelectedWindow)ScriptableObject.CreateInstance(typeof(SelectedWindow));
            }

            Tools.current = Tool.Move;

            Event e = Event.current;

            if (grid.layerList.Count == 0)
                AddLayer();


            if (grid.selected != Grid.DRAWOPTION.select)
            {

                if (preview)
                    preview.OnScene(sceneview, grid);
                else
                    preview = (PreviewWindow)ScriptableObject.CreateInstance(typeof(PreviewWindow));

                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                if ((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) && e.button == 0 && grid.selectedObj != null)
                {
                    e.Use();
                    Vector2 mousePos = Event.current.mousePosition;
                    mousePos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y;
                    Vector3 mouseWorldPos = SceneView.currentDrawingSceneView.camera.ScreenPointToRay(mousePos).origin;
                    mouseWorldPos.z = 0;
                    if (grid.snapToGrid)
                    {
                        if (grid.width > 0.05f && grid.height > 0.05f)
                        {
                            mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x / grid.width) * grid.width + grid.width / 2.0f;
                            mouseWorldPos.y = Mathf.Ceil(mouseWorldPos.y / grid.height) * grid.height - grid.height / 2.0f;
                        }
                    }
                    else
                    {
                        if (e.type == EventType.MouseDrag)
                            return;
                    }

                    GameObject[] allgo = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
                    int brk = 0;
                    if (grid.selected == Grid.DRAWOPTION.paint)
                    {
                        for (int i = 0; i < allgo.Length; i++)
                        {

                            if (Mathf.Approximately(allgo[i].transform.position.x, mouseWorldPos.x) && Mathf.Approximately(allgo[i].transform.position.y, mouseWorldPos.y) && Mathf.Approximately(allgo[i].transform.position.z, mouseWorldPos.z))
                            {
                                if (allgo[i].transform.parent == grid.currentLayerTransform)
                                {
                                    brk++;
                                    break;
                                }
                            }
                        }
                        if (brk == 0)
                        {

                            bool isGo = false;

                            GameObject newgo = null;
                            if (grid.selectedObj is Sprite)
                            {

                                newgo = new GameObject(grid.selectedObj.name, typeof(SpriteRenderer));
                                newgo.GetComponent<SpriteRenderer>().sprite = (Sprite)grid.selectedObj;

                            }
                            else
                            {
                                newgo = (GameObject)PrefabUtility.InstantiatePrefab(grid.selectedObj);
                                Transform[] allChildren = newgo.GetComponentsInChildren<Transform>();
                                foreach (Transform child in allChildren)
                                    if (child.gameObject.GetComponent<Renderer>())
                                        child.gameObject.GetComponent<Renderer>().sortingOrder = grid.currentLayer;

                                isGo = true;
                            }


                            if (newgo.GetComponent<Renderer>())
                                newgo.GetComponent<Renderer>().sortingOrder = grid.currentLayer;

                            newgo.transform.parent = grid.currentLayerTransform;
                            newgo.transform.position = mouseWorldPos;

                            preview.CreatedGO(newgo, grid, isGo);

                            Undo.RegisterCreatedObjectUndo(newgo, "Create " + newgo.name);

                            grid.currentLayerTransform.gameObject.GetComponent<LevelEditorLayer>().dirty = true;

                            Selection.activeGameObject = newgo;

                            return;
                        }
                    }

                    else if (grid.selected == Grid.DRAWOPTION.erase)
                    {
                        e.Use();
                        for (int i = 0; i < allgo.Length; i++)
                        {
                            if (Mathf.Approximately(allgo[i].transform.position.x, mouseWorldPos.x) && Mathf.Approximately(allgo[i].transform.position.y, mouseWorldPos.y) && Mathf.Approximately(allgo[i].transform.position.z, mouseWorldPos.z))
                            {
                                if (allgo[i].transform.parent == grid.currentLayerTransform)
                                {
                                    Undo.DestroyObjectImmediate(allgo[i]);
                                    grid.currentLayerTransform.gameObject.GetComponent<LevelEditorLayer>().dirty = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {

                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                Vector2 mousePos = Event.current.mousePosition;
                mousePos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y;
                Vector3 mouseWorldPos = SceneView.currentDrawingSceneView.camera.ScreenPointToRay(mousePos).origin;
                mouseWorldPos.z = 0;

                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    e.Use();

                    Selection.activeGameObject = null;
                    GameObject[] allgo = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
                    int brk = 0;
                    for (int i = 0; i < allgo.Length; i++)
                    {
                        if (Mathf.Approximately(allgo[i].transform.position.x, mouseWorldPos.x) && Mathf.Approximately(allgo[i].transform.position.y, mouseWorldPos.y) && Mathf.Approximately(allgo[i].transform.position.z, mouseWorldPos.z))
                        {
                            if (allgo[i].transform.parent == grid.currentLayerTransform)
                            {
                                brk++;
                                activeGo = allgo[i];
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < allgo.Length; i++)
                    {
                        if (allgo[i].GetComponent<SpriteRenderer>() != null && allgo[i].GetComponent<SpriteRenderer>().bounds.Contains(mouseWorldPos))
                        {
                            if (allgo[i].transform.parent == grid.currentLayerTransform)
                            {
                                brk++;
                                activeGo = allgo[i];
                                break;
                            }
                        }
                    }
                    if (brk == 0)
                        activeGo = null;

                    grid.goFocus = activeGo;
                    EditorGUIUtility.PingObject(activeGo);
                    Selection.activeGameObject = activeGo;

                }
                if (e.type == EventType.MouseDrag && e.button == 0 && activeGo != null)
                {
                    e.Use();
                    if (grid.width > 0.05f && grid.height > 0.05f)
                    {
                        mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x / grid.width) * grid.width + grid.width / 2.0f;
                        mouseWorldPos.y = Mathf.Ceil(mouseWorldPos.y / grid.height) * grid.height - grid.height / 2.0f;
                    }
                    if (activeGo.transform.parent == grid.currentLayerTransform)
                    {
                        activeGo.transform.position = mouseWorldPos;
                        grid.currentLayerTransform.gameObject.GetComponent<LevelEditorLayer>().dirty = true;
                    }
                }
            }

        }

        void AddLayer()
        {

            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/GridEditor2D/Prefabs/LevelLayer.prefab", typeof(GameObject)));
            obj.transform.position = new Vector3(0, 0, 0);
            grid.layerList.Add(obj.transform);
            grid.currentLayer = grid.layerList.Count - 1;
            obj.GetComponent<LevelEditorLayer>().sortingLayer = grid.layerList.Count - 1;
            obj.name = obj.name;
            obj.transform.parent = grid.editor;

            if (grid.currentLayerTransform != null)
                grid.currentLayerTransform.gameObject.GetComponent<LevelEditorLayer>().Lock();

            grid.currentLayerTransform = obj.transform;
            grid.currentLayerTransform.gameObject.GetComponent<LevelEditorLayer>().Unlock();
        }

        void DeleteLayer()
        {

            if (!EditorUtility.DisplayDialog("Delete current Layer?",
                "Are you sure you want to delete the current Layer?\nThis can not be undone!", "Yes", "No"))
                return;

            if (1 < grid.layerList.Count)
            {
                grid.layerList.Remove(grid.currentLayerTransform);
                DestroyImmediate(grid.currentLayerTransform.gameObject);

                for (int i = grid.currentLayer; i <= grid.layerList.Count - 1; i++)
                {
                    grid.layerList[i].gameObject.GetComponent<LevelEditorLayer>().Unlock();
                    grid.layerList[i].gameObject.GetComponent<LevelEditorLayer>().sortingLayer = i;
                    grid.layerList[i].gameObject.GetComponent<LevelEditorLayer>().SetSortingLayer();
                    grid.layerList[i].gameObject.GetComponent<LevelEditorLayer>().dirty = true;
                    grid.layerList[i].gameObject.GetComponent<LevelEditorLayer>().Lock();
                }

                if (0 < grid.currentLayer)
                    grid.currentLayer--;
                else
                    if (0 < grid.layerList.Count - 1)
                        grid.currentLayer = grid.layerList.Count - 1;

                grid.currentLayerTransform = grid.layerList[grid.currentLayer];
                grid.currentLayerTransform.gameObject.GetComponent<LevelEditorLayer>().Unlock();

                if (grid.hideOtherLayer)
                    HideLayerHelper(true);
            }
            else
            {
                grid.layerList.Remove(grid.currentLayerTransform);
                DestroyImmediate(grid.currentLayerTransform.gameObject);
            }
        }

        void SwitchLayer(bool back)
        {
            if (grid.layerList.Count == 0)
                return;
            if (back)
            {
                if (grid.currentLayer == 0) return;

                grid.currentLayerTransform.gameObject.GetComponent<LevelEditorLayer>().Lock();
                grid.currentLayer--;
                grid.currentLayerTransform = grid.layerList[grid.currentLayer];
                grid.currentLayerTransform.gameObject.GetComponent<LevelEditorLayer>().Unlock();

            }
            else
            {
                if (grid.currentLayer == grid.layerList.Count - 1) return;

                grid.currentLayerTransform.gameObject.GetComponent<LevelEditorLayer>().Lock();
                grid.currentLayer++;
                grid.currentLayerTransform = grid.layerList[grid.currentLayer];
                grid.currentLayerTransform.gameObject.GetComponent<LevelEditorLayer>().Unlock();
            }

            EditorGUIUtility.PingObject(grid.currentLayerTransform.gameObject);
            Selection.activeGameObject = grid.currentLayerTransform.gameObject;
            if (grid.hideOtherLayer)
                HideLayerHelper(true);

        }

        void OnGUI()
        {
            if (grid)
            {
                if (!grid.DrawEditorInSceneGUI)
                    DoWindow(0);
            }
            else
            {
                DoWindow(0);
            }
        }

        void DoWindow(int id)
        {
            Init();
            if (grid)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Layer: " + grid.currentLayer.ToString(), GUILayout.Width(70));

                if (GUILayout.Button(new GUIContent(LeftIcon, "Switch between layers")))
                    SwitchLayer(true);

                if (GUILayout.Button(new GUIContent(RightIcon, "Switch between layers")))
                    SwitchLayer(false);

                if (GUILayout.Button(new GUIContent(AdIconB, "Add a new layer")))
                    AddLayer();

                if (GUILayout.Button(new GUIContent(BinIcon, "Delete current layer")))
                    DeleteLayer();

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (grid.gridEnabled)
                {
                    if (GUILayout.Button((new GUIContent(PauseIcon, "Disable edit mode"))))
                    {

                        HideLayerHelper(false);
                        grid.gridEnabled = false;
                    }
                }
                else
                {
                    if (GUILayout.Button((new GUIContent(ForwardIcon, "Enable edit mode"))))
                        grid.gridEnabled = true;
                }

                switch (grid.selected)
                {

                    case Grid.DRAWOPTION.paint:

                        if (GUILayout.Button(new GUIContent(BrushIcon, "Draw Mode")))
                            grid.selected = Grid.DRAWOPTION.select;

                        break;

                    case Grid.DRAWOPTION.select:

                        if (GUILayout.Button(new GUIContent(PointerIcon, "Select Mode")))
                            grid.selected = Grid.DRAWOPTION.paint;
                        break;

                    default:
                        if (GUILayout.Button(new GUIContent(CrossIcon, "Currently in delete mode")))
                            grid.selected = Grid.DRAWOPTION.paint;
                        break;
                }

                if (grid.selected != Grid.DRAWOPTION.erase)
                {
                    if (GUILayout.Button(new GUIContent(EraserIcon, "Delete Mode")))
                        grid.selected = Grid.DRAWOPTION.erase;
                }
                else
                {
                    if (GUILayout.Button(new GUIContent(EraserIcon, "Delete Mode")))
                        grid.selected = Grid.DRAWOPTION.paint;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (!grid.hideOtherLayer)
                {
                    if (GUILayout.Button("Hide other Layers"))
                        HideLayerHelper(true);
                }
                else
                {
                    if (GUILayout.Button("Show other Layers"))
                        HideLayerHelper(false);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (grid.showGrid)
                {
                    if (GUILayout.Button("Hide Grid"))
                        grid.showGrid = false;
                }
                else
                {
                    if (GUILayout.Button("Show Grid"))
                        grid.showGrid = true;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (grid.snapToGrid)
                {
                    if (GUILayout.Button("Snap to Grid"))
                        grid.snapToGrid = false;
                }
                else
                {

                    if (GUILayout.Button("Free Hand"))
                        grid.snapToGrid = true;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Optimize Colliders"))
                    ColliderBuilder.OptimizeAll();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Restore Colliders"))
                    ColliderBuilder.RestoreAllCollider();

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Sprite Sheet Picker"))
                {
                    SpriteSheetPicker.ShowWindow();
                    return;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (grid.complexView)
                {
                    if (GUILayout.Button("Complex View"))
                        grid.complexView = false;

                }
                else
                {
                    if (GUILayout.Button("Simple View"))
                        grid.complexView = true;
                }
                EditorGUILayout.EndHorizontal();

                int prefCounter = 0;

                if (!grid.complexView && !grid.DrawEditorInSceneGUI)
                {
                    EditorGUILayout.BeginHorizontal();
                    grid.itemsPerRow = (int)EditorGUILayout.Slider(grid.itemsPerRow, 4f, 20f);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true));

                if (grid.complexView)
                {

                    foreach (Object obj in grid.prefabs)
                    {
                        if (obj)
                        {

                            bool isGo = false;
                            if (obj is GameObject) isGo = true;


                            Texture2D prefabIcon = null;

                            if (isGo)
                                prefabIcon = AssetPreview.GetAssetPreview(obj);
                            else
                            {
                                Sprite t = (Sprite)obj;
                                prefabIcon = t.texture;
                            }

                            EditorGUILayout.BeginHorizontal();

                            EditorGUILayout.BeginVertical();
                            EditorGUILayout.LabelField(obj.name, GUILayout.Width(130));

                            EditorGUILayout.BeginHorizontal();


                            if (grid.selectedObj == obj)
                            {
                                if (GUILayout.Button(new GUIContent(FlagIcon, "Select Prefab"), GUILayout.Width(64)))
                                    if (isGo)
                                        Select((GameObject)obj);
                                    else
                                        Select((Sprite)obj);
                            }
                            else
                            {

                                if (GUILayout.Button(new GUIContent(CheckIcon, "Select Prefab"), GUILayout.Width(64)))
                                    if (isGo)
                                        Select((GameObject)obj);
                                    else
                                        Select((Sprite)obj);
                            }

                            if (GUILayout.Button(new GUIContent(BinIcon, "Delete Prefab from list"), GUILayout.Width(64)))
                            {
                                if (isGo)
                                    Delete((GameObject)obj);
                                else
                                    Delete((Sprite)obj);
                                break;

                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.EndVertical();



                            if (prefabIcon == null)
                            {
                                if (GUILayout.Button(new GUIContent(EmptyIcon, obj.name), GUILayout.Height(50), GUILayout.Width(50)))
                                    if (isGo)
                                    {
                                        Select((GameObject)obj);
                                        grid.tileMap = false;
                                    }
                                    else
                                    {
                                        Select((Sprite)obj);
                                        grid.tileMap = false;
                                    }
                            }
                            else
                            {
                                if (GUILayout.Button(new GUIContent(prefabIcon, obj.name), GUILayout.Height(50), GUILayout.Width(50)))
                                    if (isGo)
                                    {
                                        Select((GameObject)obj);
                                        grid.tileMap = false;
                                    }
                                    else
                                    {
                                        Select((Sprite)obj);
                                        grid.tileMap = false;
                                    }
                            }

                            EditorGUILayout.BeginVertical();
                            if (GUILayout.Button(new GUIContent(UpIcon, "Move item up"), GUILayout.Width(25), GUILayout.Height(25)))
                            {
                                MovePrefab(true, prefCounter);
                                break;
                            }
                            if (GUILayout.Button(new GUIContent(DownIcon, "Move item down"), GUILayout.Width(25), GUILayout.Height(25)))
                            {
                                MovePrefab(false, prefCounter);
                                break;
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                            prefCounter++;

                        }
                    }
                }

                else
                {
                    int ticker = 0;
                    foreach (Object obj in grid.prefabs)
                    {

                        if (obj)
                        {

                            bool isGo = false;
                            if (obj is GameObject) isGo = true;

                            if (ticker == 0)
                                EditorGUILayout.BeginHorizontal();

                            Texture2D prefabIcon = null;

                            if (isGo)
                                prefabIcon = AssetPreview.GetAssetPreview(obj);
                            else
                            {
                                Sprite t = (Sprite)obj;
                                prefabIcon = t.texture;
                            }


                            if (prefabIcon == null)
                            {
                                if (GUILayout.Button(new GUIContent(EmptyIcon, obj.name), GUILayout.Height(50), GUILayout.Width(50)))
                                {
                                    if (isGo)
                                    {
                                        Select((GameObject)obj);
                                        grid.tileMap = false;
                                    }
                                    else
                                    {
                                        Select((Sprite)obj);
                                        grid.tileMap = false;
                                    }

                                }
                            }
                            else
                            {
                                if (GUILayout.Button(new GUIContent(prefabIcon, obj.name), GUILayout.Height(50), GUILayout.Width(50)))
                                    if (isGo)
                                    {
                                        Select((GameObject)obj);
                                        grid.tileMap = false;
                                    }
                                    else
                                    {
                                        Select((Sprite)obj);
                                        grid.tileMap = false;
                                    }
                            }

                            if (ticker == grid.itemsPerRow - 1)
                            {
                                EditorGUILayout.EndHorizontal();
                                ticker = 0;
                            }
                            else
                            {
                                ticker++;
                            }

                            prefCounter++;

                        }
                    }

                    if (ticker != 0)
                        EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent(LeftIcon, "Switch between prefab lists")))
                    SwitchGroup(true);

                if (GUILayout.Button(new GUIContent(RightIcon, "Switch between prefab lists")))
                    SwitchGroup(false);

                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent(AdIcon, "Select the prefabs in the project view and then hit the button.")))
                    CreateNewPrefab();

                if (GUILayout.Button(new GUIContent(BinIconB, "Delete current prefab list")))
                    RemovePrefabs();
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                if (grid.DrawEditorInSceneGUI)
                {
                    if (GUILayout.Button("Draw Editor as window"))
                    {
                        grid.DrawEditorInSceneGUI = false;
                        LevelEditorMenu.HideSceneEditor();
                        Repaint();
                        return;
                    }
                }
                else
                {
                    if (GUILayout.Button("Draw Editor in SceneView"))
                    {
                        grid.DrawEditorInSceneGUI = true;
                        LevelEditorMenu.HideEditor();
                        return;
                    }
                }
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.LabelField("GridEditor2D V.1.15");
                EditorUtility.SetDirty(grid);

            }
            else
            {
                if (GUILayout.Button("Create Grid"))
                {

                    GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/GridEditor2D/Prefabs/Grid.prefab", typeof(GameObject)));
                    obj.transform.position = new Vector3(0, 0, 0);

                }
            }
            if (grid)
                if (grid.DrawEditorInSceneGUI)
                    GUI.DragWindow();
        }

        List<Object> PrefabGroup(int index)
        {
            switch (index)
            {
                case 0:
                    return grid.prefabs0;
                case 1:
                    return grid.prefabs1;
                case 2:
                    return grid.prefabs2;
                case 3:
                    return grid.prefabs3;
                case 4:
                    return grid.prefabs4;
                case 5:
                    return grid.prefabs5;
                case 6:
                    return grid.prefabs6;
                case 7:
                    return grid.prefabs7;
                case 8:
                    return grid.prefabs8;
                case 9:
                    return grid.prefabs9;
            }
            return null;
        }

        void DeletePrefabGroup(int index)
        {
            switch (index)
            {
                case 0:
                    grid.prefabs0 = new List<Object>();
                    break;
                case 1:
                    grid.prefabs1 = new List<Object>();
                    break;
                case 2:
                    grid.prefabs2 = new List<Object>();
                    break;
                case 3:
                    grid.prefabs3 = new List<Object>();
                    break;
                case 4:
                    grid.prefabs4 = new List<Object>();
                    break;
                case 5:
                    grid.prefabs5 = new List<Object>();
                    break;
                case 6:
                    grid.prefabs6 = new List<Object>();
                    break;
                case 7:
                    grid.prefabs7 = new List<Object>();
                    break;
                case 8:
                    grid.prefabs8 = new List<Object>();
                    break;
                case 9:
                    grid.prefabs9 = new List<Object>();
                    break;
            }
            return;
        }

        void SwitchGroup(bool left)
        {
            if (left)
            {
                if (grid.currentPrefabList == 0)
                    return;
                grid.currentPrefabList--;
            }
            else
            {
                if (grid.currentPrefabList == 9)
                    return;
                grid.currentPrefabList++;
            }
            grid.prefabs = PrefabGroup(grid.currentPrefabList);
        }

        void MovePrefab(bool up, int index)
        {
            Object temp;

            temp = (Object)grid.prefabs[index];

            if (up)
            {
                if (index == 0)
                    return;
                grid.prefabs.Remove(grid.prefabs[index]);
                index--;

            }
            else
            {
                if (index == grid.prefabs.Count - 1)
                    return;
                grid.prefabs.Remove(grid.prefabs[index]);
                index++;
            }

            grid.prefabs.Insert(index, temp);
        }

        void HideLayerHelper(bool hide)
        {
            grid.hideOtherLayer = hide;
            foreach (Transform ly in grid.layerList)
            {
                if (ly == grid.currentLayerTransform)
                {
                    ly.gameObject.SetActive(true);
                    continue;
                }

                if (hide)
                    ly.gameObject.SetActive(false);
                else
                    ly.gameObject.SetActive(true);
            }
        }

        void RemovePrefabs()
        {
            if (!EditorUtility.DisplayDialog("Remove all Prefabs?",
                            "Are you sure you want to remove all prefabs from the list?\nthis can not be undone!", "Yes", "No"))
                return;

            DeletePrefabGroup(grid.currentPrefabList);
            grid.prefabs = new List<Object>();
        }

        void LockObj()
        {
            if (grid.locked)
            {
                grid.locked = false;
                grid.UnLock();
            }
            else
            {
                grid.locked = true;
                grid.Lock();
            }
        }

        void Select(Object select)
        {
            grid.selectedObj = select;
        }
        void Delete(Object delete)
        {
            grid.prefabs.Remove(delete);

        }

        public int GetSpritePixelPerUnit(Texture2D asset)
        {

            if (asset != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(asset);
                TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                if (importer != null)
                {
                    return (int)importer.spritePixelsPerUnit;
                }
            }


            return 100;
        }

        void CreateNewPrefab()
        {

            Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Unfiltered);
            foreach (Object obj in selectedAsset)
            {

                if (obj is GameObject)
                    PrefabGroup(grid.currentPrefabList).Add((GameObject)obj);
                if (obj is Texture2D)
                {
                    Texture2D t = (Texture2D)obj;
                    Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f), GetSpritePixelPerUnit(t));
                    temp.name = t.name;
                    PrefabGroup(grid.currentPrefabList).Add(temp);
                }
                grid.prefabs = PrefabGroup(grid.currentPrefabList);
            }

            OnGUI();
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

        private Texture2D leftIcon;
        public Texture2D LeftIcon
        {
            get
            {
                if ((UnityEngine.Object)leftIcon == (UnityEngine.Object)null)
                    leftIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "arrowLeft.png", typeof(Texture2D)) as Texture2D;

                return leftIcon;
            }
        }

        private Texture2D rightIcon;
        public Texture2D RightIcon
        {
            get
            {
                if ((UnityEngine.Object)rightIcon == (UnityEngine.Object)null)
                    rightIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "arrowRight.png", typeof(Texture2D)) as Texture2D;

                return rightIcon;
            }
        }

        private Texture2D downIcon;
        public Texture2D DownIcon
        {
            get
            {
                if ((UnityEngine.Object)downIcon == (UnityEngine.Object)null)
                    downIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "arrowDown.png", typeof(Texture2D)) as Texture2D;

                return downIcon;
            }
        }


        private Texture2D upIcon;
        public Texture2D UpIcon
        {
            get
            {
                if ((UnityEngine.Object)upIcon == (UnityEngine.Object)null)
                    upIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "arrowUp.png", typeof(Texture2D)) as Texture2D;

                return upIcon;
            }
        }

        private Texture2D adIcon;
        public Texture2D AdIcon
        {
            get
            {
                if ((UnityEngine.Object)adIcon == (UnityEngine.Object)null)
                    adIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "plus.png", typeof(Texture2D)) as Texture2D;

                return adIcon;
            }
        }

        private Texture2D adIconB;
        public Texture2D AdIconB
        {
            get
            {
                if ((UnityEngine.Object)adIconB == (UnityEngine.Object)null)
                    adIconB = AssetDatabase.LoadAssetAtPath(IconsPath + "plusB.png", typeof(Texture2D)) as Texture2D;

                return adIconB;
            }
        }


        private Texture2D checkIcon;
        public Texture2D CheckIcon
        {
            get
            {
                if ((UnityEngine.Object)checkIcon == (UnityEngine.Object)null)
                    checkIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "checkmark.png", typeof(Texture2D)) as Texture2D;

                return checkIcon;
            }
        }


        private Texture2D binIcon;
        public Texture2D BinIcon
        {
            get
            {
                if ((UnityEngine.Object)binIcon == (UnityEngine.Object)null)
                    binIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "trashcanOpen.png", typeof(Texture2D)) as Texture2D;

                return binIcon;
            }
        }

        private Texture2D binIconB;
        public Texture2D BinIconB
        {
            get
            {
                if ((UnityEngine.Object)binIconB == (UnityEngine.Object)null)
                    binIconB = AssetDatabase.LoadAssetAtPath(IconsPath + "trashcanOpenB.png", typeof(Texture2D)) as Texture2D;

                return binIconB;
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

        private Texture2D flagIcon;
        public Texture2D FlagIcon
        {
            get
            {
                if ((UnityEngine.Object)flagIcon == (UnityEngine.Object)null)
                    flagIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "flag.png", typeof(Texture2D)) as Texture2D;

                return flagIcon;
            }
        }

        private Texture2D pointerIcon;
        public Texture2D PointerIcon
        {
            get
            {
                if ((UnityEngine.Object)pointerIcon == (UnityEngine.Object)null)
                    pointerIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "pointer.png", typeof(Texture2D)) as Texture2D;

                return pointerIcon;
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


        private Texture2D pauseIcon;
        public Texture2D PauseIcon
        {
            get
            {
                if ((UnityEngine.Object)pauseIcon == (UnityEngine.Object)null)
                    pauseIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "pause.png", typeof(Texture2D)) as Texture2D;

                return pauseIcon;
            }
        }

        private Texture2D forwardIcon;
        public Texture2D ForwardIcon
        {
            get
            {
                if ((UnityEngine.Object)forwardIcon == (UnityEngine.Object)null)
                    forwardIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "forward.png", typeof(Texture2D)) as Texture2D;

                return forwardIcon;
            }
        }

        private Texture2D crossIcon;
        public Texture2D CrossIcon
        {
            get
            {
                if ((UnityEngine.Object)crossIcon == (UnityEngine.Object)null)
                    crossIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "cross.png", typeof(Texture2D)) as Texture2D;

                return crossIcon;
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

    public enum PlayModeState
    {
        Stopped,
        Playing,
        Paused
    }

    [InitializeOnLoad]
    public class EditorPlayMode
    {
        private PlayModeState _currentState = PlayModeState.Stopped;

        EditorPlayMode()
        {
            EditorApplication.playmodeStateChanged = OnUnityPlayModeChanged;
        }

        public event System.Action<PlayModeState, PlayModeState> PlayModeChanged;

        public void Play()
        {
            EditorApplication.isPlaying = true;
        }

        public void Pause()
        {
            EditorApplication.isPaused = true;
        }

        public void Stop()
        {
            EditorApplication.isPlaying = false;
        }


        private void OnPlayModeChanged(PlayModeState currentState, PlayModeState changedState)
        {
            if (PlayModeChanged != null)
                PlayModeChanged(currentState, changedState);
        }

        private void OnUnityPlayModeChanged()
        {
            var changedState = PlayModeState.Stopped;
            switch (_currentState)
            {
                case PlayModeState.Stopped:
                    if (EditorApplication.isPlayingOrWillChangePlaymode)
                    {
                        changedState = PlayModeState.Playing;
                    }
                    break;
                case PlayModeState.Playing:
                    if (EditorApplication.isPaused)
                    {
                        changedState = PlayModeState.Paused;
                    }
                    else
                    {
                        changedState = PlayModeState.Stopped;
                    }
                    break;
                case PlayModeState.Paused:
                    if (EditorApplication.isPlayingOrWillChangePlaymode)
                    {
                        changedState = PlayModeState.Playing;
                    }
                    else
                    {
                        changedState = PlayModeState.Stopped;
                    }
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }

            // Fire PlayModeChanged event.
            OnPlayModeChanged(_currentState, changedState);

            // Set current state.
            _currentState = changedState;
        }

    }
}