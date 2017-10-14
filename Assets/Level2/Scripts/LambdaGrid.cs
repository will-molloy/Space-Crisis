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

	public LambdaCube GetAt(int x, int y) {
		if(x >= 0 && x < MAX_LAMBDA_GRID_WIDTH && y >= 0 && y < MAX_LAMBDA_GRID_HEIGHT)
			return lambdaActualLines[x, y];
        else return LambdaCube.NONE;
	}

	public void SetAt(int x, int y, LambdaCube cube) {
		if(x >= 0 && x < MAX_LAMBDA_GRID_WIDTH && y >= 0 && y < MAX_LAMBDA_GRID_HEIGHT) {
			lambdaActualLines[x, y] = cube;
		}

	}

	public void FallDown() {
		bool changed = false;
		// [Row, Col]
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

	public static Sprite LambdaGridItemToSprite(LambdaGrid.LambdaCube en) {
		switch(en) {
			case LambdaCube.RED: return Leve2Controller.RED_CUBE;
			case LambdaCube.ORANGE: return Leve2Controller.ORANGE_CUBE;
			case LambdaCube.YELLOW: return Leve2Controller.YELLOW_CUBE;
			case LambdaCube.GREEN: return Leve2Controller.GREEN_CUBE;
			case LambdaCube.CYAN: return Leve2Controller.CYAN_CUBE;
			case LambdaCube.BLUE: return Leve2Controller.BLUE_CUBE;
			case LambdaCube.PURPLE: return Leve2Controller.PURPLE_CUBE;
			default: return null;
		}
	}

	public void SimpleMap(LambdaCube from, LambdaCube to) {
        for (int i = 0; i < MAX_LAMBDA_GRID_HEIGHT; i++)
        {
            for (int j = 0; j < MAX_LAMBDA_GRID_WIDTH; j++)
            {
				if(lambdaActualLines[i, j] == from) {
					lambdaActualLines[i, j] = to;
				}
				// WE need to fall down incase map to none
				FallDown();
            }
        }
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

	public string ToString() {
		
		string build = "";
        for (int i = MAX_LAMBDA_GRID_HEIGHT - 1; i >= 0; i--)
        {
            for (int j = 0; j < MAX_LAMBDA_GRID_WIDTH; j++)
            {
				build += lambdaActualLines[i,j] + " ";
            }
			build += "\n";
        }
		return build;
	}

	public void Apply(LambdaBehavior behavior) {
		behavior.function(this);
	}

}
