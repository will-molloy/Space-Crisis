using UnityEngine;
using System.Collections;

public class room1_pressure_plate_activate : MonoBehaviour
{

    private static ArrayList colliders;
    private BushRotation bush;

    public GameObject theThingToActivate;

    // Use this for initialization
    void Start()
    {
        colliders = new ArrayList();
        bush = (BushRotation)theThingToActivate.GetComponent(typeof(BushRotation));
        Debug.Log(bush);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Colliders " + colliders.Count);
        if (colliders.Count == 0)
        {
            bush.setActivation(false);
        } else
        {
            bush.setActivation(true);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player") || col.gameObject.name.Equals("Box"))
        {
            colliders.Add(col.gameObject);
        }

    }

    void OnCollisionExit2D(Collision2D col)
    { 
        if (col.gameObject.tag.Equals("Player") || col.gameObject.name.Equals("Box"))
        {
            colliders.Remove(col.gameObject);
        }
    }

}
