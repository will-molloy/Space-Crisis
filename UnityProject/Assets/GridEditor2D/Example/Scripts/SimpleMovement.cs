using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class SimpleMovement : MonoBehaviour
{

    public float playerSpeed = 4.0f;
    public float jumpForce = 7f;
  
    private Rigidbody2D rBody;
    private BoxCollider2D col;
    private Animator ani;
    private Camera playerCamera;
    public bool followPlayer = false;


    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector2.up, col.bounds.extents.y + 0.1f, ~(1 << 8));
    }

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        rBody.gravityScale = 0;
        col = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        playerCamera = Camera.main;
    }



    void FixedUpdate()
    {
        if (IsGrounded())
        {
            ani.SetBool("Jump", false);
            if (Input.GetKey("w"))
            {
                rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
                ani.SetBool("Jump", true);
            }
        }

        if (Input.GetKey("a"))
        {
            rBody.velocity = new Vector2(-playerSpeed, rBody.velocity.y);
            ani.SetBool("IsWalking", true);
        }
        if (Input.GetKey("d"))
        {
            rBody.velocity = new Vector2(playerSpeed, rBody.velocity.y);
            ani.SetBool("IsWalking", true);
        }

        if (rBody.velocity.x < 0)
        {
            transform.localEulerAngles = new Vector3(transform.rotation.x, 180, 0);
        }

        if (0 < rBody.velocity.x)
        {
            transform.localEulerAngles = new Vector3(transform.rotation.x, 0, 0);
        }

        if (rBody.velocity.x == 0)
        {
            ani.SetBool("IsWalking", false);
            ani.SetTrigger("Stand");
        }

        rBody.AddForce(new Vector2(0, -9.81f));
        if (followPlayer)
            playerCamera.transform.position = new Vector3(transform.position.x, playerCamera.transform.position.y, playerCamera.transform.position.z);


    }

   
}
