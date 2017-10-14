using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GridEditor2D
{
    public class LevelEditorLayer : MonoBehaviour
    {
        [HideInInspector]
        public bool locked = false;
        [HideInInspector]
        public bool dirty = true;
        public int sortingLayer = 0;

        public void Lock()
        {
            locked = true;
            LockObject(gameObject, true);
        }

        public void Unlock()
        {
            locked = false;
            UnlockObject(gameObject, true);
        }

        void LockObject(GameObject targetObject, bool recursive)
        {
            if(targetObject != gameObject)
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

        public void SetSortingLayer()
        {
            if (GetComponent<Renderer>())
                GetComponent<Renderer>().sortingOrder = sortingLayer;

            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.gameObject.GetComponent<Renderer>())
                    child.gameObject.GetComponent<Renderer>().sortingOrder = sortingLayer;
            }
        }

        public void ClearLayer()
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
