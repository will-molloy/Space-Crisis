using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class levelComplete : MonoBehaviour
{

    Timer timer;
    Text score;
    Text time;
    // Use this for initialization
    void Start()
    {
        timer = GameObject.Find("TimerText").GetComponent<Timer>();
        score = GameObject.Find("ScoreText").GetComponent<Text>();
        time = GameObject.FindGameObjectWithTag("TimerTag").GetComponent<Text>();
    }

    // Update is called once per frame, it pauses the timer and gets the score/time
	// from the timer script in string form.
	// After getting the score, the timer is reseted. 
    void Update()
    {
        timer.Pause();
        score.text = "Score: " + timer.timerString;
        time.text = timer.timerString;
        timer.Reset();
    }
}
