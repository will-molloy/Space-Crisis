using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
	 
	void OnCollisionEnter(Collision col){
		if (col.gameObject.name == "Team") {
			Destroy (col.gameObject);
		}
	}
}
