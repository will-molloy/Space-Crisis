using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LambdaItemScript : MonoBehaviour {

	public LambdaBehavior lambdaBehavior;
	public string description;

	public static LambdaItemScript Of(LambdaBehavior beh) {
		var ret = new LambdaItemScript();
		ret.lambdaBehavior = beh;
		ret.description = beh.desc;
		return ret;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D () {

	}
}
