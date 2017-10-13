using UnityEngine;
using System.Collections;
using System;

public class LambdaGrid {

	public const short MAX_LAMBDA_GRID_WIDTH = 8;
	public const short MAX_LAMBDA_GRID_HEIGHT = 8;

	private LambdaCube[,] lambdaActualLines {
		get;
		set;
	}

	public LambdaGrid() {
        lambdaActualLines = new LambdaCube[MAX_LAMBDA_GRID_HEIGHT, MAX_LAMBDA_GRID_WIDTH];
        for (int i = 0; i < MAX_LAMBDA_GRID_HEIGHT; i++)
        {
            for (int j = 0; j < MAX_LAMBDA_GRID_WIDTH; j++)
            {
				lambdaActualLines[i, j] = LambdaCube.NONE;
            }
        }
    }

	private void FallDown() {
		bool changed = false;
        for (int i = MAX_LAMBDA_GRID_HEIGHT - 1; i >= 0; i--)
        {
            for (int j = 0; j < MAX_LAMBDA_GRID_WIDTH; j++)
            {
				if(lambdaActualLines[i, j] != LambdaCube.NONE
					&& i > 0
					&& lambdaActualLines[i-1, j] == LambdaCube.NONE) {
					changed = true;
					lambdaActualLines[i-1, j] = lambdaActualLines[i,j];
					lambdaActualLines[i, j] = LambdaCube.NONE;
				}
            }
        }
		if(changed) {
			FallDown();
		}
	}


	public enum LambdaCube {
		RED, ORANGE, YELLOW, GREEN, CYAN, BLUE, PURPLE, NONE, RAINBOW, ALPHA, BETA, GAMMA, YETA
	}

	public void SimpleMap(LambdaCube from, LambdaCube to) {
		throw new NotImplementedException();
	}

	public void Stack(LambdaCube what) {
		throw new NotImplementedException();

	}

	public void StackMap(LambdaCube from, LambdaCube[] tos) {
		throw new NotImplementedException();

	}

	public void Filter(LambdaCube what) {
		throw new NotImplementedException();

	}

	public void FilterContains(LambdaCube what) {
		throw new NotImplementedException();

	}

	public void Reverse() {
		throw new NotImplementedException();

	}

	public void StackEquals() {
		throw new NotImplementedException();

	}

}
