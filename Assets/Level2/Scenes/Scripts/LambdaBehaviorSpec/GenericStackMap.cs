
using UnityEngine;
using System.Collections;
using System;

public class GenericStackMap : LambdaItemScript {

	public string from, toTop, toBtm;
	// Use this for initialization
	private static string templateString = "<sprite=\"{1}\" index=0>\n" +
                                           "<sprite=\"{0}\" index=0> <sprite=\"simplearrow\" index=0> <sprite=\"{2}\" index=0>";

	void Start () {
		var lfrom = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), from);
		var ltoTop = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), toTop);
		var ltoBtm = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), toBtm);
		lambdaBehavior = new LambdaBehavior(i => i.StackMap(lfrom, new LambdaGrid.LambdaCube[2]{ltoTop, ltoBtm}));
		// Override desc in GUI
		if(String.IsNullOrEmpty(description)) {
            description = String.Format(templateString, LambdaGrid.GetAssetStringFromCube(lfrom), LambdaGrid.GetAssetStringFromCube(ltoTop), LambdaGrid.GetAssetStringFromCube(ltoBtm));
		}
		lambdaBehavior.desc = description;
	}

	// Update is called once per frame
	void Update () {
	
	}
}

