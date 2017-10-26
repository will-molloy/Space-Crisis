using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour {

	public Button button;
	Timer timer;

	void Start () {
		Button btn = GameObject.Find("Resume").GetComponent<Button> (); 
		btn.onClick.AddListener (TaskOnClick);
		timer = GameObject.Find("TimerText").GetComponent<Timer>();
	}
	
	//Stops the button is clicked the timer starts to run again.
	void TaskOnClick () {
		timer.Resume ();
	}
}
