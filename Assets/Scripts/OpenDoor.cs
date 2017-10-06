using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenDoor : MonoBehaviour {

    private static ArrayList colliders;
    // Use this for initialization
    void Start()
    {
        colliders = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Colliders " + colliders.Count);
        if (colliders.Count != 2)
        {
            //do nothing
        } else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Application.LoadLevel(2);
            }
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
