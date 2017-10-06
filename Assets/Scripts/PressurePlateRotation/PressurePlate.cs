using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{

    public PressurePlateController parent;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player") || col.gameObject.name.Equals("Box"))
        {
            parent.colliders.Add(col.gameObject);
        }

    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player") || col.gameObject.name.Equals("Box"))
        {
            parent.colliders.Remove(col.gameObject);
        }
    }

}
