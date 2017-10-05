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
			life.UpdateHearts();
		}
		attacked = false;
	}

	void OnTriggerEnter2D (Collider2D other){
		if (other.transform.parent.gameObject.CompareTag ("Team")) {
			life = other.transform.parent.gameObject.GetComponent<LifeSystem>();
			life.TakeDamage ();
			life.UpdateHearts();
			attacked = true;
		}
	}
}
