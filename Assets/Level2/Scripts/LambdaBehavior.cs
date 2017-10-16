using UnityEngine;
using System.Collections;

public class LambdaBehavior {

	// XXX: Maybe just return void since we are mutating the state?
	public delegate void Fn(LambdaGrid gridA);
	public string desc = "None";
	public Fn function;
	public LambdaBehavior(Fn f) {
		function = f;
	}


}
