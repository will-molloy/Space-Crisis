using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour {
	Timer timer;
	public bool paused = false;
	public GameObject pauseScreen;

	// Use this for initialization
	void Awake () {
		timer = GameObject.Find("TimerText").GetComponent<Timer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && !paused) {
			timer.Pause ();
			pauseScreen.SetActive (true);
			paused = true;
		} else if (Input.GetKeyDown (KeyCode.Escape) && paused) {
			timer.Resume ();
			pauseScreen.SetActive (false);
			paused = false;
		}
	}
}
