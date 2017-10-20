using UnityEngine;
using System.Collections;

public class Reverse : LambdaItemScript {

	// Use this for initialization
	void Start () {
		lambdaBehavior = new LambdaBehavior(i => i.Reverse());
		lambdaBehavior.desc = "Reverse";
		lambdaBehavior.extraAction = new LambdaBehavior.ExtraActionOnTMProUI(ui => {ui.fontSize = 16; ui.alignment = TMPro.TextAlignmentOptions.Center;});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
