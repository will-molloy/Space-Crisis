using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour {

	public Button button;
	Timer timer;

	// Use this for initialization
	void Start () {
		Button btn = GameObject.Find("Resume").GetComponent<Button> (); 
		btn.onClick.AddListener (TaskOnClick);
		timer = GameObject.Find("TimerText").GetComponent<Timer>();
	}
	
	// Update is called once per frame
	void TaskOnClick () {
		timer.Resume ();
	}
}
