using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using System.Collections.Generic;

public class Lever : MonoBehaviour {

    private int remainingFrames = int.MaxValue;
    private bool isRunning = false;

    public int timeInFrames;
    public List<GameObject> thingsToControl = new List<GameObject>();
   
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
        if(isRunning && remainingFrames < 1)
        {
            foreach (GameObject obj in thingsToControl)
            {
                PlateScript ps = obj.GetComponent<PlateScript>();
                ps.stop();
                ps.reverseDirection();
            }
            isRunning = false;
        }
	
	}

    public void activate()
    {
        Debug.Log("ACTIVATE");
        Debug.Log("IS RUNING " + isRunning);
        if (isRunning) return;
        isRunning = true;

        Flip();
        remainingFrames = timeInFrames;
 
        foreach(GameObject obj in thingsToControl)
        {
            PlateScript ps = obj.GetComponent<PlateScript>();
            ps.setAnimationTime(this.timeInFrames);
            ps.start();
        }

    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
