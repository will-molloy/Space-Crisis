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
			Leve2Controller.instance.AddPlayerEnterPortal();
		}
	}

	void OnTriggerExit2D(Collider2D hit) {
		if(hit.gameObject.tag == "Player") {
			Leve2Controller.instance.DecreasePlayerEnterPortal();
		}
	}

	void OnCollisionEnter2D(Collision2D hit) {
		if(hit.transform.tag == "Player") {
			Leve2Controller.instance.AddPlayerEnterPortal();
		}
	}

	void OnCollisionExit2D(Collision2D hit) {
		if(hit.transform.tag == "Player") {
			Leve2Controller.instance.DecreasePlayerEnterPortal();
		}
	}

	void OnCollisionStay2D(Collision2D hit) {
		Debug.Log("ASDf");
	}


}
