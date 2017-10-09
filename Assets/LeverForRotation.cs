using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using System.Collections.Generic;

public class LeverForRotation : Lever
{

    private bool isTrihard = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    override public void Update()
    {
        if (isRunning) remainingFrames--;
        if (isRunning && remainingFrames < 1)
        {
            isRunning = false;
        }

    }

    override public void activate()
    {
        Debug.Log("ACTIVATE");
        Debug.Log("IS RUNNING " + isRunning);
        if (isRunning) return;
        isRunning = true;

        Flip();
        remainingFrames = timeInFrames;

        foreach (GameObject obj in thingsToControl)
        {
            RotatableSprite rs = obj.GetComponent<RotatableSprite>();
            rs.setActivation(!isTrihard);
            isTrihard = !isTrihard;
        }

    }

    private void Flip()
    {
        Debug.Log("FLIP");
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
