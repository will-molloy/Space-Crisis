using UnityEngine;
using System.Collections;

public class MapRedToCyan : LambdaItemScript {

	// Use this for initialization
	void Start () {
		lambdaBehavior = new SimpleMapBehavior(LambdaGrid.LambdaCube.RED, LambdaGrid.LambdaCube.CYAN);
		lambdaBehavior.desc = description;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

