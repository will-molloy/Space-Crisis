using UnityEngine;
using System.Collections;

public class PlateScript : MonoBehaviour {

    public double translationAmount;
    public Vector3 translationDirection;
    private int animationTimeInFrames;
    private bool isRunning = false;

    public void setAnimationTime(int t)
    {
        animationTimeInFrames = t;
    }

    public void reverseDirection()
    {
        translationDirection.x = -translationDirection.x;
        translationDirection.y = -translationDirection.y;
    }

    public void start()
    {
        isRunning = true;
    }

    public void stop()
    {
        isRunning = false;
    }
	
	// Update is called once per frame
	public void Update() {
        if (!isRunning) return;
        transform.Translate(translationDirection * ((float)translationAmount / animationTimeInFrames));
    }
}
