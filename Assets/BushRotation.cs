using UnityEngine;
using System.Collections;

public class BushRotation : MonoBehaviour
{

    private bool activated = false;

    private float translation;
    private const int ROTATION_SPEED = 300;
    public int UPPER_ANGLE;
    public int LOWER_ANGLE;



    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        translation = Time.deltaTime * ROTATION_SPEED;


        if (activated && transform.rotation.eulerAngles.z <= UPPER_ANGLE)
        {
            transform.Rotate(0, 0, translation);
        }
        else if (!activated && getRotation() >= LOWER_ANGLE)
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
