using UnityEngine;
using System.Collections.Generic;

public class PressurePlateController : MonoBehaviour {

    internal List<GameObject> colliders;
    public GameObject theThingToActivate;
    private RotatableSprite rotatableSprite;

    // Use this for initialization
    void Start () {
        colliders = new List<GameObject>();
        rotatableSprite = (RotatableSprite)theThingToActivate.GetComponent(typeof(RotatableSprite));
        Debug.Log(rotatableSprite);
    }

    // Update is called once per frame
    void Update () {
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
