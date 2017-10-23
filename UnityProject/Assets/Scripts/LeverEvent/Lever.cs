using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using System.Collections.Generic;

public class Lever : MonoBehaviour
{
    public GameController.PlayableScene ThisScene;
    protected int remainingFrames = int.MaxValue;
    protected bool isRunning = false;
    public AudioClip pulledFX;
    public int timeInFrames;
    public List<GameObject> thingsToControl = new List<GameObject>();
    private LeverState state = LeverState.RIGHT;

    private enum LeverState
    {
        LEFT, RIGHT
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            var p1 = other.gameObject.GetComponent<Platformer2DUserControl>();
            if (p1 != null)
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

    void Start()
    {
        if (ThisScene == GameController.PlayableScene.None)
            throw new System.Exception("Please set ThisScene");
        if (this.GetLeverInFinalPos())
        {
            Flip();
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
                PlateScript ps = obj.GetComponent<PlateScript>();
                ps.stop();
            }
            isRunning = false;
        }

    }

    public virtual void activate()
    {
        if (isRunning) return;

        this.ActivateLever();
        isRunning = true;

        var ani = GetComponent<Animator>();
        if(state == LeverState.LEFT)
        {
            ani.SetTrigger("triggerRight");
            state = LeverState.RIGHT;
        }
        else
        {
            ani.SetTrigger("triggerLeft");
            state = LeverState.LEFT;
        }
        remainingFrames = timeInFrames;

        if (pulledFX != null)
        {
            AudioSource.PlayClipAtPoint(pulledFX, transform.position);
        }

        foreach (GameObject obj in thingsToControl)
        {
            PlateScript ps = obj.GetComponent<PlateScript>();
            Vector3 currentObjPos = obj.transform.position;

            ps.setAnimationTime(this.timeInFrames);
            ps.start();
            ps.reverseDirection();
        }

    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void assignSoundFX(AudioClip clip)
    {
        pulledFX = clip;
    }
}
