﻿using UnityEngine; 
using System.Collections; 
using UnityEngine.UI; 

public class Timer : MonoBehaviour { 

	public float timer = 0; 
	public Text timerText; 

	// Use this for initialization 
	void Start () { 
		timerText = GetComponent<Text> (); 
	} 

	// Update is called once per frame 
	void Update () { 
		timer += Time.deltaTime; 

		int seconds = (int)(timer % 60); 
		int minutes = (int)(timer / 60); 

		string timerString = string.Format ("{0:0}:{1:00}", minutes, seconds); 

		timerText.text = timerString; 
	} 
} 