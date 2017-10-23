using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using System.Collections.Generic;

public class LeverNew : MonoBehaviour
{

    protected int remainingFrames = int.MaxValue;
    protected bool isRunning = false;
	public AudioClip pulledFX;
    public int timeInFrames;
    public List<GameObject> thingsToControl = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("COLLUSION" + other);
        var am = GetComponent<Animation>();
        am.Play("lever");
        am.Play();
        if (other.gameObject.tag == "Player")
        {
            Platformer2DUserControl p1 = other.gameObject.GetComponent<Platformer2DUserControl>();
            if (p1 != null)
            {
                //p1.lever = this;
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

    // Update is called once per frame
    virtual public void Update()
    {
        if (isRunning) remainingFrames--;
        if (isRunning && remainingFrames < 1)
        {
            foreach (GameObject obj in thingsToControl)
            {
                var ps = obj.GetComponent<PlateScript>();
                ps.stop();
                ps.reverseDirection();
            }
            isRunning = false;
        }

    }

    public virtual void activate()
    {
        Debug.Log("ACTIVATE");
        Debug.Log("IS RUNING " + isRunning);
        if (isRunning) return;
        isRunning = true;

        Flip();
        remainingFrames = timeInFrames;

		if (pulledFX != null){
			AudioSource.PlayClipAtPoint(pulledFX, transform.position);
		}


        foreach (GameObject obj in thingsToControl)
        {
            var ps = obj.GetComponent<PlateScript>();
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

	public void assignSoundFX(AudioClip clip){
		pulledFX = clip;
	}
}
