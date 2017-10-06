using UnityEngine;
using System.Collections;
using System;

public class PressurePlateController : MonoBehaviour {

    internal ArrayList colliders;
    public GameObject theThingToActivate;
    private RotatableSprite rotatableSprite;

    // Use this for initialization
    void Start () {
        colliders = new ArrayList();
        rotatableSprite = (RotatableSprite)theThingToActivate.GetComponent(typeof(RotatableSprite));
        Debug.Log(rotatableSprite);
    }

    // Update is called once per frame
    void Update () {
        Debug.Log("Colliders " + colliders.Count);
        if (colliders.Count == 0)
        {
            rotatableSprite.setActivation(false);
        }
        else
        {
            rotatableSprite.setActivation(true);
        }
    }
}
