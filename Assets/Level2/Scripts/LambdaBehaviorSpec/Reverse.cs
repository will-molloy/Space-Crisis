using UnityEngine;
using System.Collections;

public class Reverse : LambdaItemScript {

	// Use this for initialization
	void Start () {
		lambdaBehavior = new LambdaBehavior(i => i.Reverse());
		lambdaBehavior.desc = "Reverse";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
