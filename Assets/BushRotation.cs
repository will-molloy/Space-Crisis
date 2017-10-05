using UnityEngine;
using System.Collections;

public class BushRotation : MonoBehaviour {

    private static bool activated = false;

    private float translation;
    private int tolerance = 5;

    // Use this for initialization
    void Start () {


}

// Update is called once per frame
void Update () {
        translation = Time.deltaTime * 300;

        GameObject pivit = GameObject.Find("bush-log-rotation-point");
        HingeJoint2D bush = (HingeJoint2D)pivit.GetComponent(typeof(HingeJoint2D));

        if (activated && transform.rotation.eulerAngles.z <= 230)
        {
              transform.Rotate(0, 0, translation);
        }
        else if (!activated && getRotation()  >= 140 && transform.rotation.eulerAngles.z <= 230+tolerance)
        {
             transform.Rotate(0, 0, -1 * translation);
        }
        Debug.Log("rotation: " + getRotation());
    }

    public void setActivation(bool active)
    {
        activated = active;
    }

    private int getRotation()
    {
        return (int)transform.rotation.eulerAngles.z;
    }
}
