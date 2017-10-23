using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndCutscene : MonoBehaviour {

	private float time = 60.0f;
	
	// Update is called once per frame
	void Update () {

		if (time <= 0) {
			SceneManager.LoadScene("ExitScene2");
		} else if(time > 0){
			time -= Time.deltaTime;
		}
	
	}
}
