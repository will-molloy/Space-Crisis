using UnityEngine; 
using System.Collections; 
using UnityEngine.UI; 

public class Timer : MonoBehaviour { 

	static public float timer = 0; 
	public Text timerText; 
	public bool paused = false;

	// Use this for initialization 
	void Start () { 
		timerText = GetComponent<Text> ();
	} 

	// Update is called once per frame 
	void Update () { 
		if (!paused) {
			timer += Time.deltaTime; 
		
		int seconds = (int)(timer % 60); 
		int minutes = (int)(timer / 60); 

		string timerString = string.Format ("{0:0}:{1:00}", minutes, seconds); 

		timerText.text = timerString; 
		}
	} 

	public void Pause(){
		paused = true;
	}

	public void Resume(){
		paused = false;
	}

	public void Reset(){
		timer = 0;
	}
} 