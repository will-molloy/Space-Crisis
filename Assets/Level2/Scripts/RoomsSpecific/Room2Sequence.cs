
using UnityEngine;
using System.Collections;

public class Room2Sequence : LambdaSequence {

	// Use this for initialization
	protected void Start () {
		start = LambdaGrid.FromString
        ("NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE RED YELLOW YELLOW CYAN NONE NONE\n" +
		"NONE NONE RED RED CYAN CYAN NONE NONE");

		answer = LambdaGrid.FromString
        ("NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE RED RED RED RED NONE NONE\n" +
		"NONE NONE RED RED RED RED NONE NONE");
		if(CheckValidity()) {
			var first = seq[0].GetComponent<DisplayScreen>();
			first.UpdateLambdaGrid(start);
			var last = seq[seq.Count - 1].GetComponent<DisplayScreen>();
			last.UpdateLambdaGrid(answer);
		}
		else {
			throw new System.Exception("Invalid sequence, Check it again!");
		}
	}

	void Update() {
		if(CheckValidity()) {
			StartCascadeUpdate();
		}
		else {
			throw new System.Exception("Invalid sequence, Check it again!");
		}
	}
	
}
