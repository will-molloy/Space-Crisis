
using UnityEngine;
using System.Collections;
using System;

public class GenericStackMap : LambdaItemScript {

	public string from, toTop, toBtm;
	// Use this for initialization
	private static string templateStr = "Map {0} to {1} and {2}";
	void Start () {
		var lfrom = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), from);
		var ltoTop = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), toTop);
		var ltoBtm = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), toBtm);
		lambdaBehavior = new LambdaBehavior(i => i.StackMap(lfrom, new LambdaGrid.LambdaCube[2]{ltoTop, ltoBtm}));
		// Override desc in GUI
		if(String.IsNullOrEmpty(description)) {
			OverrideString();
		}
		lambdaBehavior.desc = description;
	}

	void OverrideString() {
        description = String.Format(templateStr, from, toTop, toBtm);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

