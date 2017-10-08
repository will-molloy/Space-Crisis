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
        if (colliders.Count == 2 && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            colliders.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            colliders.Remove(other.gameObject);
        }
    }
}
