using UnityEngine;
using System.Collections;

namespace GridEditor2D
{
    public class TileManager : MonoBehaviour
    {
        [HideInInspector]
        public bool addCollider = false;
        [HideInInspector]
        public bool dirty = false;
    }
}