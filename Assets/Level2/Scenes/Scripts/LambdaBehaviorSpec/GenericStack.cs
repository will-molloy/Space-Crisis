using UnityEngine;
using System.Collections;
using System;

public class GenericStack : LambdaItemScript {

	public string what;
	// Use this for initialization
	private static string templateString = "Stack\n" +
											"   <sprite=\"{0}\" index=0>";

	void Start () {
		var lwhat = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), what);
		lambdaBehavior = new LambdaBehavior(i => i.Stack(lwhat));
		// Override desc in GUI
		if(String.IsNullOrEmpty(description)) {
			description = String.Format(templateString, LambdaGrid.GetAssetStringFromCube(lwhat));
		}
		lambdaBehavior.desc = description;
		lambdaBehavior.extraAction = new LambdaBehavior.ExtraActionOnTMProUI(ui => ui.alignment = TMPro.TextAlignmentOptions.Center);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}

