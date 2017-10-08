using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    public string sceneToLoad;
    private List<GameObject> colliders;

    public Sprite[] sps = new Sprite[3];

    // Use this for initialization
    void Start()
    {
        /*
        sps.Add(Resources.Load("/Sprites/Portals/portal-0.png", typeof(Sprite)) as Sprite);
        sps.Add(Resources.Load("/Sprites/Portals/portal-1.png", typeof(Sprite)) as Sprite);
        sps.Add(Resources.Load("/Sprites/Portals/portal-2.png", typeof(Sprite)) as Sprite);
        */

        colliders = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer r;
        r = GetComponent<SpriteRenderer>();
        r.sprite = sps[2 - colliders.Count];
        if (colliders.Count == 2)
        {
       
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsValidCollider(other))
        {
            colliders.Add(other.gameObject);
        }

       
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (IsValidCollider(other))
        {
            colliders.Remove(other.gameObject);
        }
    }

    private bool IsValidCollider(Collider2D col)
    {
        return col.gameObject.CompareTag("Player");
    }
}
