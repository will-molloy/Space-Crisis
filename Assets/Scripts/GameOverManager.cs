using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

	LifeSystem life;
	Animator anim;
	GameObject container;
	GameObject team;


	void Awake () {
		anim = GetComponent <Animator> ();
		team = GameObject.FindGameObjectWithTag("Team");
		life = team.GetComponent <LifeSystem> ();
	}

	void Update () {
		if (life.isDead) {
			Debug.Log ("death");
			anim.SetTrigger ("GameOver");
		}
	}
}
