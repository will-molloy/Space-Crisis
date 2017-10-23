using UnityEngine;
using System.Collections;

public class EndCutscene : MonoBehaviour {

	private float time = 60.0f;
	
	// Update is called once per frame
	void Update () {

		if (time <= 0) {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		} else if(time > 0){
			time -= Time.deltaTime;
		}
	
	}
}
