using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using System.IO;
using System.Linq;

namespace GridEditor2D
{
    public class ColliderBuilder : Editor
    {
        static Grid grid;
        private GameObject layer;

        static void Init()
        {
            grid = (Grid)FindObjectOfType(typeof(Grid));
        }

        static void HideLayerHelper(Transform currentLayerTransform, bool hide)
        {

            grid.hideOtherLayer = hide;
            foreach (Transform ly in grid.layerList)
            {
                if (ly == currentLayerTransform)
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

        public static void OptimizeAll()
        {
            if (!grid)
                Init();

            foreach (Transform t in grid.layerList)
            {
                if (t.GetComponent<LevelEditorLayer>().dirty)
                    Optimize(t);
            }

            HideLayerHelper(grid.currentLayerTransform, false);
        }


        public static void RestoreAllCollider()
        {
            if (!grid)
                Init();

            foreach (Transform t in grid.layerList)
                t.GetComponent<LevelEditorLayer>().dirty = true;


            Transform[] allChildren = grid.editor.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (!child.GetComponent<TileManager>())
                    continue;
                if (child.GetComponent<BoxCollider2D>())
                    DestroyImmediate(child.GetComponent<BoxCollider2D>());
                child.gameObject.AddComponent<BoxCollider2D>();
            }
        }

        static RaycastHit2D DoRayCheck(Vector2 pos, float distance, Vector2 direction)
        {
            return Physics2D.Raycast(pos, direction, distance);
        }

        static void Optimize(Transform layerTransform)
        {

            HideLayerHelper(layerTransform, true);
            layerTransform.GetComponent<LevelEditorLayer>().dirty = false;

            List<Tile> tiles = new List<Tile>();

            //Store all Tiles with the managerscript in a List
            foreach (Transform child in layerTransform.transform)
            {
                if (!child.GetComponent<TileManager>())
                    continue;

                Tile curTile = new Tile();

                if (child.GetComponent<BoxCollider2D>())
                    DestroyImmediate(child.GetComponent<BoxCollider2D>());


                child.gameObject.AddComponent<BoxCollider2D>();

                curTile.boxCollider = child.GetComponent<BoxCollider2D>();
                curTile.go = child.gameObject;

                child.GetComponent<TileManager>().dirty = false;
                curTile.manager = child.GetComponent<TileManager>();
                curTile.position = child.transform.position;
                tiles.Add(curTile);
            }

            //Sort tiles by x position
            tiles = tiles.OrderBy(x => x.position.x).ToList();

            //First batchingpass X
            foreach (Tile ff in tiles)
            {

                if (ff.manager.dirty)
                    continue;
                int brk = 1;
                Vector2 pos = ff.position;
                float size = 0.1f;
                float colliderSizeX = 0;

                float boundX = ff.boxCollider.size.x/2;
                colliderSizeX = 0;

                //Do a raycast
                RaycastHit2D hit;
                GameObject temp = ff.go;

                List<GameObject> catched = new List<GameObject>();

                //Loop through object direction right
                for (int i = 0; i < grid.ColliderBatchingSize.x; i++)
                {

                    //Exclude prefabs mark tile as dirty
                    if (!temp.GetComponent<TileManager>() || temp.GetComponent<TileManager>().dirty)
                        break;

                    temp.GetComponent<TileManager>().dirty = true;
                    colliderSizeX += temp.GetComponent<BoxCollider2D>().size.x;
                    //Position for the next ray
                    pos = temp.transform.position;
                    pos.x += temp.GetComponent<BoxCollider2D>().bounds.size.x;

                    if (i != 0)
                        catched.Add(temp);

                    //disable collider for raycast
                    temp.GetComponent<BoxCollider2D>().enabled = false;

                    hit = DoRayCheck(pos, size, Vector2.right);

                    //No object on the right side
                    if (!hit)
                    {
                        //Reenable the collider and abord          
                        temp.GetComponent<BoxCollider2D>().enabled = true;
                        if (i == 0)
                            ff.manager.dirty = false;
                        break;
                    }
                    else
                    {

                        temp.GetComponent<BoxCollider2D>().enabled = true;
                        if (!hit.collider.gameObject.GetComponent<TileManager>())
                        {
                            if (i == 0)
                                ff.manager.dirty = false;

                            break;
                        }
                        //Already used object? abord
                        if (hit.collider.gameObject.GetComponent<TileManager>().dirty)
                        {
                            if (i == 0)
                                ff.manager.dirty = false;
                            break;
                        }

                        if (!Mathf.Approximately(ff.boxCollider.bounds.size.x, hit.collider.bounds.size.x))
                        {
                            if (i == 0)
                                ff.manager.dirty = false;
                            break;
                        }

                    }

                    temp = hit.collider.gameObject;

                    brk++;
                }


                //Line found
                if (1 < brk)
                {
                    //remove Boxcolliders
                    foreach (GameObject go in catched)
                        DestroyImmediate(go.GetComponent<BoxCollider2D>());


                    ff.boxCollider.size = new Vector2(colliderSizeX, ff.boxCollider.size.y);
                    if (ff.go.transform.localEulerAngles.y != 180)
                        ff.boxCollider.offset = new Vector2(((ff.boxCollider.size.x/2) - boundX), ff.boxCollider.offset.y);
                    else
                        ff.boxCollider.offset = new Vector2((((ff.boxCollider.size.x/2) - boundX) * -1), ff.boxCollider.offset.y);

                }
            }

            //Sort tiles by y position
            tiles = tiles.OrderBy(x => x.position.y).ToList();

            //First batchingpass X
            foreach (Tile ff in tiles)
            {

                if (ff.manager.dirty)
                    continue;
                int brk = 1;
                Vector2 pos = ff.position;
                float size = 0.1f;
                float colliderSizeY = 0;

                float boundY = ff.boxCollider.size.y/2;
                colliderSizeY = 0;

                //Do a raycast
                RaycastHit2D hit;
                GameObject temp = ff.go;

                List<GameObject> catched = new List<GameObject>();

                //Loop through object direction right
                for (int i = 0; i < grid.ColliderBatchingSize.y; i++)
                {

                    //Exclude prefabs mark tile as dirty
                    if (!temp.GetComponent<TileManager>() || temp.GetComponent<TileManager>().dirty)
                        break;

                    temp.GetComponent<TileManager>().dirty = true;
                    colliderSizeY += temp.GetComponent<BoxCollider2D>().size.y;
                    //Position for the next ray
                    pos = temp.transform.position;
                    pos.y += temp.GetComponent<BoxCollider2D>().bounds.size.y;

                    if (i != 0)
                        catched.Add(temp);

                    //disable collider for raycast
                    temp.GetComponent<BoxCollider2D>().enabled = false;

                    hit = DoRayCheck(pos, size, Vector2.right);

                    //No object on the right side
                    if (!hit)
                    {
                        //Reenable the collider and abord          
                        temp.GetComponent<BoxCollider2D>().enabled = true;
                        if (i == 0)
                            ff.manager.dirty = false;
                        break;
                    }
                    else
                    {

                        temp.GetComponent<BoxCollider2D>().enabled = true;
                        if (!hit.collider.gameObject.GetComponent<TileManager>())
                        {
                            if (i == 0)
                                ff.manager.dirty = false;

                            break;
                        }
                        //Already used object? abord
                        if (hit.collider.gameObject.GetComponent<TileManager>().dirty)
                        {
                            if (i == 0)
                                ff.manager.dirty = false;
                            break;
                        }

                        if (!Mathf.Approximately(ff.boxCollider.bounds.size.x, hit.collider.bounds.size.x))
                        {
                            if (i == 0)
                                ff.manager.dirty = false;
                            break;
                        }

                    }

                    temp = hit.collider.gameObject;

                    brk++;
                }

                //Line found
                if (1 < brk)
                {
                    //remove Boxcolliders
                    foreach (GameObject go in catched)
                        DestroyImmediate(go.GetComponent<BoxCollider2D>());

                    ff.boxCollider.size = new Vector2(ff.boxCollider.size.x, colliderSizeY);
                    if (ff.go.transform.localEulerAngles.y != 180)
                        ff.boxCollider.offset = new Vector2(ff.boxCollider.offset.x, ((ff.boxCollider.size.y/2) - boundY));
                    else
                        ff.boxCollider.offset = new Vector2(ff.boxCollider.offset.x, (((ff.boxCollider.size.y/2) - boundY) * -1));
                }
            }
        }


        public class Tile
        {
            public Vector2 position = Vector2.zero;
            public GameObject go;
            public bool dirty = false;
            public BoxCollider2D boxCollider;
            public TileManager manager;
        }


        public static Texture2D textureFromSprite(Sprite t)
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
                return t.texture;
        }

    }
}
