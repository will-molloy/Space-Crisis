using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour
{

    private Rigidbody2D rBody;

    // Use this for initialization
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rBody.velocity = new Vector2(-8, rBody.velocity.y);

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
