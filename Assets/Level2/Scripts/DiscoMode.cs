using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscoMode : MonoBehaviour {

// DOES ABOSOLUTELY NOTHING
	public string DISCOOOOOOOOOO;
	private float time = Time.time;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(time < Time.time) {
			time = Time.time + 0.3f;
		}
		else return;
	Sprite[] DISCOS = new Sprite[3] {
		Leve2Controller.DISCO_1, 
		Leve2Controller.DISCO_2, 
		Leve2Controller.DISCO_3, 
	};
		if(!DISCOOOOOOOOOO.StartsWith("YEAH!")) return;
		var objs = GameObject.FindGameObjectsWithTag("ground");
		foreach( var o in objs) {
			var rdr = o.GetComponent<SpriteRenderer>();
			rdr.sprite = DISCOS[(int)Random.Range(0, DISCOS.Length)];
		}
	}
}
