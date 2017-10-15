using UnityEngine;
using System.Collections;

public class MapCyanToGreen : LambdaItemScript {

	// Use this for initialization
	void Start () {
		lambdaBehavior = new SimpleMapBehavior(LambdaGrid.LambdaCube.CYAN, LambdaGrid.LambdaCube.GREEN);
		lambdaBehavior.desc = description;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
