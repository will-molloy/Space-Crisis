using UnityEngine;
using System.Collections;

public class room1_pressure_plate_activate : MonoBehaviour {

    private static ArrayList colliders;

	// Use this for initialization
	void Start () {

   colliders  = new ArrayList();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.tag.Equals("pressure_activator"))
        {
            colliders.Add(col.gameObject);
            GameObject pivit = GameObject.Find("bush-log-rotation-point");
            BushRotation bush = (BushRotation)pivit.GetComponent(typeof(BushRotation));
            bush.setActivation(true);
        }
       
    }

    void OnCollisionExit2D(Collision2D col)
    {


        if (col.gameObject.tag.Equals("pressure_activator"))
        {
            colliders.Remove(col.gameObject);
            if (colliders.Count == 0)
            {
                GameObject pivit = GameObject.Find("bush-log-rotation-point");
                BushRotation bush = (BushRotation)pivit.GetComponent(typeof(BushRotation));
                bush.setActivation(false);
            }
        }
    }

}
