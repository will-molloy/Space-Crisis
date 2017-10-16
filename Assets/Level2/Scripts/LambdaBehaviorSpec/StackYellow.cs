using UnityEngine;
using System.Collections;

public class StackYellow : LambdaItemScript {

	// Use this for initialization
	void Start () {
		lambdaBehavior = new LambdaBehavior(i => i.Stack(LambdaGrid.LambdaCube.YELLOW));
		lambdaBehavior.desc = "Stack Yellow";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
