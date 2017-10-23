using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnColliderEnter2D() {


	}

	void OnTriggerEnter2D(Collider2D hit) {
		if(hit.gameObject.tag == "Player") {
			Leve2Controller.instance.AddPlayerEnterPortal(this);
		}
	}

	void OnTriggerExit2D(Collider2D hit) {
		if(hit.gameObject.tag == "Player") {
			Leve2Controller.instance.DecreasePlayerEnterPortal(this);
		}
	}



}
