﻿using UnityEngine;
using System.Collections;

public class Room1Sequence : LambdaSequence {

	// Use this for initialization
	protected void Start () {
		start = new LambdaGrid();
		start.SetAt(0, 3, LambdaGrid.LambdaCube.RED);
		start.SetAt(0, 4, LambdaGrid.LambdaCube.RED);
		start.SetAt(0, 5, LambdaGrid.LambdaCube.RED);

		answer = new LambdaGrid();
		answer.SetAt(0, 3, LambdaGrid.LambdaCube.CYAN);
		answer.SetAt(0, 4, LambdaGrid.LambdaCube.CYAN);
		answer.SetAt(0, 5, LambdaGrid.LambdaCube.CYAN);
		
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