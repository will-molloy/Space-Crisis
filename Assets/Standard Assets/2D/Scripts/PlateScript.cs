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
        Debug.Log(translationDirection);
        isRunning = true;
    }

    public void stop()
    {
        isRunning = false;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() {
        //Debug.Log(isRunning);
        if (!isRunning) return;
        //transform.position = Vector3.Lerp(transform.position, translationDirection, 300 * Time.deltaTime);
        transform.Translate(translationDirection * ((float)translationAmount / animationTimeInFrames));
    }
}
