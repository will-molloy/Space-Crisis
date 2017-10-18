using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GridEditor2D
{
    public class Grid : MonoBehaviour
    {

        public enum DRAWOPTION { select, paint, paintover, erase };

        [Header("Settings")]
        public bool OptimizeColliderAsDefault = true;
        public bool ScaleToGridAsDefault = false;

        [HideInInspector]
        public bool DrawEditorInSceneGUI = false;

        [HideInInspector]
        public DRAWOPTION selected = DRAWOPTION.paint;
        [HideInInspector]
        public float width = 1.2f;
        [HideInInspector]
        public float height = 1.2f;
        [HideInInspector]
        public Color color = Color.white;
        [HideInInspector]
        public bool showPreview = false;
        [HideInInspector]
        public bool optimized = true;
        [HideInInspector]
        public bool snapToGrid = true;
        [HideInInspector]
        public int currentPrefabList = 0;
        [HideInInspector]
        public Transform editor;
        [HideInInspector]
        public GameObject goFocus;
        [HideInInspector]
        public Object selectedObj;
        [HideInInspector]
        public int itemsPerRow = 4;
        [HideInInspector]
        public bool locked = false;
        [HideInInspector]
        public bool gridEnabled = false;
        [HideInInspector]
        public bool showGrid = true;
        [HideInInspector]
        public bool hideOtherLayer = false;
        [HideInInspector]
        public bool complexView = true;
        [HideInInspector]
        public int mode = 0;
        [HideInInspector]
        public int currentLayer = 0;
        [HideInInspector]
        public bool tileMap = false;
        [HideInInspector]
        public int spriteMapIndex = 0;
        [HideInInspector]
        public int spriteMapZoomFactor = 1;
        public Vector2 ColliderBatchingSize = new Vector2(20, 20);
        [HideInInspector]
        public List<Transform> layerList = new List<Transform>();
        [HideInInspector]
        public List<Transform> levelLayerList = new List<Transform>();
        [HideInInspector]
        public Transform currentLayerTransform;
        [HideInInspector]
        public List<Object> prefabs = new List<Object>();
        [HideInInspector]
        public List<Object> prefabs0 = new List<Object>();
        [HideInInspector]
        public List<Object> prefabs1 = new List<Object>();
        [HideInInspector]
        public List<Object> prefabs2 = new List<Object>();
        [HideInInspector]
        public List<Object> prefabs3 = new List<Object>();
        [HideInInspector]
        public List<Object> prefabs4 = new List<Object>();
        [HideInInspector]
        public List<Object> prefabs5 = new List<Object>();
        [HideInInspector]
        public List<Object> prefabs6 = new List<Object>();
        [HideInInspector]
        public List<Object> prefabs7 = new List<Object>();
        [HideInInspector]
        public List<Object> prefabs8 = new List<Object>();
        [HideInInspector]
        public List<Object> prefabs9 = new List<Object>();
        [HideInInspector]
        public Transform levelTransform;

        public void Lock()
        {
            LockObject(gameObject, true);
        }

        public void UnLock()
        {
            UnlockObject(gameObject, true);
        }

        void LockObject(GameObject targetObject, bool recursive)
        {

            targetObject.hideFlags = targetObject.hideFlags | HideFlags.NotEditable;

            if (recursive)
            {
                foreach (Transform child in targetObject.transform)
                    LockObject(child.gameObject, true);
            }
        }

        void UnlockObject(GameObject targetObject, bool recursive)
        {
            targetObject.hideFlags = targetObject.hideFlags & ~HideFlags.NotEditable;

            if (recursive)
            {
                foreach (Transform child in targetObject.transform)
                    UnlockObject(child.gameObject, true);
            }
        }

        void OnDrawGizmos()
        {

            if (!showGrid)
                return;

            if (width < 0.1f) width = 0.1f;
            if (height < 0.1f) height = 0.1f;

            Vector3 pos = Camera.current.transform.position;
            Gizmos.color = color;

            for (float y = pos.y - 800.0f; y < pos.y + 800.0f; y += height)
            {
                Gizmos.DrawLine(new Vector3(-1000000.0f, Mathf.Floor(y / height) * height, 0.0f),
                                new Vector3(1000000.0f, Mathf.Floor(y / height) * height, 0.0f));
            }

            for (float x = pos.x - 1200.0f; x < pos.x + 1200.0f; x += width)
            {

                Gizmos.DrawLine(new Vector3(Mathf.Floor(x / width) * width, -1000000.0f, 0.0f),
                                new Vector3(Mathf.Floor(x / width) * width, 1000000.0f, 0.0f));
            }
        }
    }
}
