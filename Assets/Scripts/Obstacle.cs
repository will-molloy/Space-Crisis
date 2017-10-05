using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	GameObject player;
	LifeSystem life;
	bool attacked = false;

	void Awake(){
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update(){
		if(attacked){
			life.TakeDamage ();
		}
		attacked = false;
	}

	void OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			life = other.gameObject.GetComponent<LifeSystem>();

		}
	}
}
