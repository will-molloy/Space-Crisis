using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using System.Collections.Generic;

public class Lever : MonoBehaviour {

    private int remainingFrames;
    private bool isRunning = false;

    public int timeInFrames;
    public List<GameObject> thingsToConrol = new List<GameObject>();

    private bool facing = false;
   

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("COLLUSION" + other);
        if(other.gameObject.tag == "Player")
        {
            Platformer2DUserControl p1 = other.gameObject.GetComponent<Platformer2DUserControl>();
            if(p1 != null)
            {
                p1.lever = this;
            }
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Platformer2DUserControl p1 = other.gameObject.GetComponent<Platformer2DUserControl>();
            if (p1 != null)
            {
                p1.lever = null;
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning) remainingFrames--;
        if(remainingFrames < 1)
        {
            isRunning = false;
        }
	
	}

    public void activate()
    {
        if (isRunning) return;

        Flip();
        isRunning = true;
        remainingFrames = timeInFrames;
 
        foreach(GameObject obj in thingsToConrol)
        {
            
        }

    }

    private void Flip()
    {
        facing = !facing;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
