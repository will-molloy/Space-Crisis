using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour {
	Timer timer;
	public bool paused = false;
	public GameObject pauseScreen;
	GameObject[] players;
	bool frozen = false;
	public GameObject pauseMenu;
	public GameObject controlPanel;

	// Use this for initialization
	void Awake () {
		timer = GameObject.Find("TimerText").GetComponent<Timer>();
		players = GameObject.FindGameObjectsWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && !paused) {
			timer.Pause ();
			pauseScreen.SetActive (true);
			PlayerUtility.FreezePlayers();
			paused = true;
			frozen = true;
		} else if (((Input.GetKeyDown (KeyCode.Escape) && paused))||(pauseScreen.activeSelf == false)&&frozen) {
			timer.Resume ();
			pauseMenu.SetActive (true);
			controlPanel.SetActive (false);
			pauseScreen.SetActive (false);
			PlayerUtility.UnFreezePlayers();
			paused = false;
			frozen = false;
		}
	}
}
