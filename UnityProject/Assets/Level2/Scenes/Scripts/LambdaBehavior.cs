using UnityEngine;
using System.Collections;
using TMPro;

public class LambdaBehavior {

	// XXX: Maybe just return void since we are mutating the state?
	public delegate void Fn(LambdaGrid gridA);

	public delegate void ExtraActionOnTMProUI(TMPro.TextMeshProUGUI ui);
	public string desc = "";
	public Fn function;
	public ExtraActionOnTMProUI extraAction;
	public LambdaBehavior(Fn f) {
		function = f;
	}


}
