using UnityEngine;
using System.Collections;

public class AnimalWalking : MonoBehaviour
{

    public float leftWall; // Left x value to turn around
    public float rightWall; // Right x value to turn around
    public float speed; //Movement speed

    private int direction = -1; //Positive for walking right, negative for left
    private float x;

    // Use this for initialization
    void Start()
    {
     
    }

    void FixedUpdate()
    {
        x = transform.position.x + speed * direction;

        if (x <= leftWall)
        {
            direction = 1; //Turn right
            transform.Rotate(0, 180 * direction, 0); //Rotate sprite
        }
        else if (x >= rightWall)
        {
            direction = -1; //Turn right
            transform.Rotate(0, 180 * direction, 0); //Rotate sprite
        }
        
        transform.position = new Vector3(x, transform.position.y, 0); //Update position
    }

}
