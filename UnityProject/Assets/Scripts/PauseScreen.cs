using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour {
	Timer timer;
	public bool paused = false;
	public GameObject pauseScreen;
	GameObject[] players;

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
			for (int i = 0; i < players.Length; i++) {
				PlatformerCharacter2D move = players [i].GetComponent<PlatformerCharacter2D> ();
				move.frozen = true;
			}
			paused = true;
		} else if ((Input.GetKeyDown (KeyCode.Escape) && paused)) {
			timer.Resume ();
			pauseScreen.SetActive (false);
			for (int i = 0; i < players.Length; i++) {
				PlatformerCharacter2D move = players [i].GetComponent<PlatformerCharacter2D> ();
				move.frozen = false;
			}
			paused = false;
		}
	}
}
