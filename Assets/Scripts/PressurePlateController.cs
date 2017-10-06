using UnityEngine;
using System.Collections;
using System;

public class PressurePlateController : MonoBehaviour {

    internal ArrayList colliders;
    public GameObject theThingToActivate;
    private RotatableSprite bush;

    // Use this for initialization
    void Start () {
        colliders = new ArrayList();
        bush = (RotatableSprite)theThingToActivate.GetComponent(typeof(RotatableSprite));
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
