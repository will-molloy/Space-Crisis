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
        this.ReversePlateDirection();
    }

    public void start()
    {
        this.SetLeverPlateDirection(translationDirection);
        translationDirection = this.GetLeverPlateDirection();
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
