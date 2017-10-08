using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    public string sceneToLoad;
    private List<GameObject> colliders;

    // Use this for initialization
    void Start()
    {
        colliders = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
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
