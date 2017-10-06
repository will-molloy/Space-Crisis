using UnityEngine;
using System.Collections;

public class PlateScript : MonoBehaviour {

    public int translationAmount;
    public Vector3 translationDirection;
    private int animationTimeInFrames;
    private bool isAtStart = true;
    private bool isRunning = false;

    public void setAnimationTime(int t)
    {
        animationTimeInFrames = t;
    }

    public void start()
    {
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
        Debug.Log(isRunning);
        if (!isRunning) return;
        transform.Translate(translationDirection * (1f / animationTimeInFrames));
    }
}
