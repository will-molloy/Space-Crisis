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

	private LambdaGrid(LambdaCube[,] newMatrix) {
		lambdaActualLines = new LambdaCube[MAX_LAMBDA_GRID_HEIGHT, MAX_LAMBDA_GRID_WIDTH];
        for (int i = 0; i < MAX_LAMBDA_GRID_HEIGHT; i++)
        {
            for (int j = 0; j < MAX_LAMBDA_GRID_WIDTH; j++)
            {
				lambdaActualLines[i, j] = newMatrix[i, j];
            }
        }
	}

	public LambdaGrid Deepcopy() {
		return new LambdaGrid(lambdaActualLines);
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
			case LambdaCube.RAINBOW: return Leve2Controller.RAINBOW_CUBE;
			case LambdaCube.ALPHA: return Leve2Controller.CUPHEAD_CUBE;
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
            }
        }
        FallDown();
	}

	public void Stack(LambdaCube what) {
        for (int j = 0; j < MAX_LAMBDA_GRID_WIDTH; j++)
        {
            for (int i = 0; i < MAX_LAMBDA_GRID_HEIGHT; i++)
            {
				if(lambdaActualLines[i, j] == LambdaCube.NONE) {
					if(i == 0) {
						// the col is empty
						break;
					}
					lambdaActualLines[i, j] = what;
					break;
				}
            }
        }
	}

	private void Bump(int i, int j) {
		if(i >= MAX_LAMBDA_GRID_HEIGHT) {
			lambdaActualLines[i-1, j] = LambdaCube.NONE;
			return;
		}
		if(lambdaActualLines[i+1, j] != LambdaCube.NONE) {
			Bump(i+1, j);
		}
			lambdaActualLines[i+1, j] = lambdaActualLines[i, j];
			lambdaActualLines[i,j] = LambdaCube.NONE;
		
	}

/* tos => |0|
	      |1|
		  ^^^ */
	public void StackMap(LambdaCube from, LambdaCube[] tos) {
		if(tos.Length != 2) {
			throw new System.NotImplementedException();
		}
        for (int i = 0; i < MAX_LAMBDA_GRID_HEIGHT; i++)
        {
            for (int j = 0; j < MAX_LAMBDA_GRID_WIDTH; j++)
            {
				if(lambdaActualLines[i, j] == from) {
					Bump(i+1, j);
					lambdaActualLines[i+1, j] = tos[0];
					lambdaActualLines[i, j] = tos[1];
				}
				// WE need to fall down incase map to none
				FallDown();
            }
        }
	}

	public void Filter(LambdaCube what) {
		SimpleMap(what, LambdaCube.NONE);
	}

	public void FilterContains(LambdaCube what) {
		throw new NotImplementedException();

	}

	public void Reverse() {
		// Inverse the column
        for (int j = 0; j < MAX_LAMBDA_GRID_WIDTH; j++)
        {
            for (int i = 0; i < MAX_LAMBDA_GRID_HEIGHT / 2; i++)
            {
                var tmp = lambdaActualLines[MAX_LAMBDA_GRID_HEIGHT - i - 1, j];
                lambdaActualLines[MAX_LAMBDA_GRID_HEIGHT -i - 1, j] = lambdaActualLines[i, j];
                lambdaActualLines[i, j] = tmp;
            }
        }
		FallDown();
	}

	public void StackEquals() {
		throw new NotImplementedException();

	}

	public void Identity() {

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

	// override object.Equals
	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		
		LambdaGrid grid = (LambdaGrid) obj;
        for (int i = 0; i < MAX_LAMBDA_GRID_HEIGHT; i++)
        {
            for (int j = 0; j < MAX_LAMBDA_GRID_WIDTH; j++)
            {
				if(lambdaActualLines[i, j] != grid.lambdaActualLines[i, j]) {
					return false;
				}
            }
        }
		return true;
	}
	
	/**
	 * Parse a string representation of the grid
	 * Lines are seped by '\n'
	 * Items are seped by a blank char
	 */
	public static LambdaGrid FromString(String str) {
		// Tokenise 
		var lines = str.Split('\n');
		var grid = new LambdaGrid();
		if(lines.Length != MAX_LAMBDA_GRID_HEIGHT) {
			throw new System.Exception("Invalid string");
		}
		for(int i = 0; i < MAX_LAMBDA_GRID_HEIGHT; i++) {
			var tokens = lines[i].Split();
            if (tokens.Length != MAX_LAMBDA_GRID_WIDTH)
            {
                throw new System.Exception(String.Format("Invalid String"));
			}
			for(int j = 0; j < MAX_LAMBDA_GRID_WIDTH; j++) {
				var eVal = Enum.Parse(typeof(LambdaCube), tokens[j]);
				grid.SetAt(MAX_LAMBDA_GRID_HEIGHT - i - 1, j, (LambdaCube)eVal);
			}
		}
		// Potential FallDown()
		return grid;
	}
	

}
