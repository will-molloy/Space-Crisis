using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	LifeSystem life;

	void OnTriggerEnter2D (Collider2D other){
        

        if (other.gameObject.tag.Equals("Player")) { 
			life = other.transform.parent.gameObject.GetComponent<LifeSystem>(); 
			life.TakeDamage (); 
		}
	}
}
