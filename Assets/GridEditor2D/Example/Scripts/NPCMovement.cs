using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class NPCMovement : MonoBehaviour
{

    private Rigidbody2D rBody;
    private BoxCollider2D col;
    private int direction = -1;
    public float speed = 2f;
    public bool groundCheck = true;

    bool IsGrounded()
    {
        if (!groundCheck)
            return true;

        return Physics2D.Raycast(transform.position, -Vector2.up, col.bounds.extents.y + 0.1f, ~(1 << 9));
    }

    // Use this for initialization
    void Start()
    {
        gameObject.layer = 9;
        rBody = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

    }

    void FixedUpdate()
    {

        if (IsGrounded())
        {
            rBody.velocity = new Vector2(speed * direction, rBody.velocity.y);

            if (Physics2D.Raycast(transform.position, Vector2.right * direction, col.bounds.extents.x + 0.1f, ~(1 << 9)))
            {
                direction *= -1;
            }
            if (rBody.velocity.x < 0)
            {
                transform.localEulerAngles = new Vector3(transform.rotation.x, 0, 0);
            }

            if (0 < rBody.velocity.x)
            {
                transform.localEulerAngles = new Vector3(transform.rotation.x, 180, 0);
            }
        }
        if (!groundCheck)
            rBody.velocity = new Vector2(rBody.velocity.x, 0);


    }

}
