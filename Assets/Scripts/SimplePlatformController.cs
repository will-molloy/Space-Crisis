using UnityEngine;
using System.Collections;

public class SimplePlatformController : MonoBehaviour {

	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;

	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform groundCheck;

	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator> ();		//Reference to animator
		rb2d = GetComponent<Rigidbody2D> ();	//Reference to the asset
	}
	
	// Update is called once per frame
	void Update () {
		grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground")); //Check if grounded. Linecast checks if it hits anything and returns a boolean
		//1 << LayerMask.NameToLayer ("Ground") is saying that the only layer we are casting against here is the ground layer
		if (Input.GetButtonDown ("Jump") && grounded) {
			jump = true;
		}
	}

	void FixedUpdate(){
		float h = Input.GetAxis ("Horizontal");
		anim.SetFloat ("Speed", Mathf.Abs (h));

		if(h * rb2d.velocity.x < maxSpeed){ //Checks the speed limit
			rb2d.AddForce(Vector2.right * h * moveForce); //Only using right but because h could be a positive or negative number. If we are getting a negative 1 for h then Vector2.right is going to become left

		}

		if(Mathf.Abs (rb2d.velocity.x) > maxSpeed){ //If the speed is greater than the max speed
			rb2d.velocity = new Vector2 (Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y); //Setting the speed directly to the max speed
		}

		if(h > 0 && !facingRight){ //Moving to the right, but not facing right
			Flip (); //Then flip
		} else if (h < 0 && facingRight) {
			Flip ();
		}

		if(jump){
			anim.SetTrigger ("Jump");
			rb2d.AddForce (new Vector2 (0f, jumpForce));
			jump = false; //Can't double jump
		}
	}

	void Flip(){ //Flip the sprite around if we are trying to move to the left or right
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale; //Store ourlocal scale into theScale
		theScale.x *= -1; //Scaling the x axis by negative 1 to flip the sprite around
		transform.localScale = theScale;
	}
}
