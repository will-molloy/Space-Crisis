using UnityEngine;
using System.Collections;
using System;

public class GenericMap : LambdaItemScript {

	public string from, to;
	// Use this for initialization
	private static string templateStr = "Map <b>{0}</b> to {1}";
	void Start () {
		var lfrom = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), from);
		var lto = (LambdaGrid.LambdaCube)Enum.Parse(typeof(LambdaGrid.LambdaCube), to);
		lambdaBehavior = new SimpleMapBehavior(lfrom, lto);
		// Override desc in GUI
		if(String.IsNullOrEmpty(description)) {
			OverrideString();
		}
		lambdaBehavior.desc = description;
	}

	void OverrideString() {
        description = String.Format(templateStr, from, to);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
