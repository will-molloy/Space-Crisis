using UnityEngine;
using System.Collections;

public class Room1Sequence : LambdaSequence {

	// Use this for initialization
	protected void Start () {
		start = LambdaGrid.FromString
        ("NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE RED NONE NONE NONE NONE RED NONE\n" +
		"NONE YELLOW RED NONE NONE RED YELLOW NONE\n" +
		"NONE YELLOW YELLOW RED RED YELLOW YELLOW NONE");

		answer = LambdaGrid.FromString
        ("NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE NONE NONE NONE NONE NONE NONE NONE\n" +
		"NONE RED RED RED RED RED RED NONE\n" +
		"NONE YELLOW YELLOW YELLOW YELLOW YELLOW YELLOW NONE\n" +
		"NONE RED RED RED RED RED RED NONE");
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
