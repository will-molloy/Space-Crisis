using UnityEngine;
using System.Collections;
using System;

public class PressurePlateController : MonoBehaviour {

    internal ArrayList colliders;
    public GameObject theThingToActivate;
    private BushRotation bush;

    // Use this for initialization
    void Start () {
        colliders = new ArrayList();
        bush = (BushRotation)theThingToActivate.GetComponent(typeof(BushRotation));
        Debug.Log(bush);
    }

    // Update is called once per frame
    void Update () {
        Debug.Log("Colliders " + colliders.Count);
        if (colliders.Count == 0)
        {
            bush.setActivation(false);
        }
        else
        {
            bush.setActivation(true);
        }
    }
}
