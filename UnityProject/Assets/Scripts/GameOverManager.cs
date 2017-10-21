using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

	LifeSystem life;
	Animator anim;
	GameObject container;
	GameObject team;
	Timer timer;


	void Awake () {
		anim = GetComponent <Animator> ();
		team = GameObject.FindGameObjectWithTag("Team");
		life = team.GetComponent <LifeSystem> ();
		timer = GameObject.Find("TimerText").GetComponent<Timer>();
	}

	void Update () {
		if (life.isDead) {
			anim.SetTrigger ("GameOver");
			timer.Pause ();
			timer.Reset ();
		}
	}
}
