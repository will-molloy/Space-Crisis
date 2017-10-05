using UnityEngine;
using System.Collections;

public class BushRotation : MonoBehaviour
{

    private static bool activated = false;

    private float translation;
    private const int TOLERANCE = 5;
    private const int ROTATION_SPEED = 300;
    private const int UPPER_ANGLE = 230;
    private const int LOWER_ANGLE = 140;

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        translation = Time.deltaTime * ROTATION_SPEED;

        GameObject pivit = GameObject.Find("bush-log-rotation-point");
        HingeJoint2D bush = (HingeJoint2D)pivit.GetComponent(typeof(HingeJoint2D));

        if (activated && transform.rotation.eulerAngles.z <= UPPER_ANGLE)
        {
            transform.Rotate(0, 0, translation);
        }
        else if (!activated && getRotation() >= LOWER_ANGLE && transform.rotation.eulerAngles.z <= UPPER_ANGLE + TOLERANCE)
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
