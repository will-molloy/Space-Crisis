using UnityEngine;
using System.Collections;
using System;

public class GenericFilterContains : LambdaItemScript {

	public string what;
	// Use this for initialization
	private static string templateString = "Filter\nContains\n" +
											"   <sprite=\"{0}\" index=0>";
	void Start () {
		var lwhat = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), what);
		lambdaBehavior = new LambdaBehavior(i => i.FilterContains(lwhat));
		// Override desc in GUI
		if(String.IsNullOrEmpty(description)) {
			description = String.Format(templateString, LambdaGrid.GetAssetStringFromCube(lwhat));
            lambdaBehavior.extraAction = new LambdaBehavior.ExtraActionOnTMProUI(ui => { ui.alignment = TMPro.TextAlignmentOptions.Center; ui.fontSize = 16; });
		}
		lambdaBehavior.desc = description;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
