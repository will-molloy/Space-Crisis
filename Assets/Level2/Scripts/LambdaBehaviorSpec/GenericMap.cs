using UnityEngine;
using System.Collections;
using System;

public class GenericMap : LambdaItemScript {

	public string from, to;
	// Use this for initialization
	private static string templateString = "\n" +
                                           "<sprite=\"{0}\" index=0> <sprite=\"simplearrow\" index=0> <sprite=\"{1}\" index=0>";
	private static string rejectTemplateString = "Reject\n" +
											"   <sprite=\"{0}\" index=0>";

	void Start () {
		var lfrom = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), from);
		var lto = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), to);
		lambdaBehavior = new SimpleMapBehavior(lfrom, lto);
		if(lto == LambdaGrid.LambdaCube.NONE && lfrom != LambdaGrid.LambdaCube.NONE) {
            // must be rejection
            if (String.IsNullOrEmpty(description))
            {
                description = String.Format(rejectTemplateString, LambdaGrid.GetAssetStringFromCube(lfrom));
            }
            lambdaBehavior.extraAction = new LambdaBehavior.ExtraActionOnTMProUI(ui => ui.alignment = TMPro.TextAlignmentOptions.Center);
		}
		else {
            if (String.IsNullOrEmpty(description))
            {
                description = String.Format(templateString, LambdaGrid.GetAssetStringFromCube(lfrom), LambdaGrid.GetAssetStringFromCube(lto));
            }
		}
		// Override desc in GUI
		lambdaBehavior.desc = description;
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
