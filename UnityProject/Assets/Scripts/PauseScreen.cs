using UnityEngine;
using System.Collections.Generic;
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
            FreezePlayers();
			paused = true;
		} else if ((Input.GetKeyDown (KeyCode.Escape) && paused)) {
			timer.Resume ();
			pauseScreen.SetActive (false);
            UnFreezePlayers();
			paused = false;
		}
	}

    public void FreezePlayers()
    {
        List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        players.ForEach(player => {
            PlatformerCharacter2D move = player.GetComponent<PlatformerCharacter2D>();
            move.frozen = false;
        });
    }

    public void UnFreezePlayers()
    {
        List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        players.ForEach(player => {
            PlatformerCharacter2D move = player.GetComponent<PlatformerCharacter2D>();
            move.frozen = true;
        });
    }
}
