using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class DisplayScreen : MonoBehaviour {

	public Texture2D baseTexture;
	private LambdaGrid lambdaGrid {get; set;}
	// Use this for initialization
	private SpriteRenderer renderer;
	private const float DIVISION = 2f / LambdaGrid.MAX_LAMBDA_GRID_HEIGHT;
	private const float LOCAL_SCALING = 1.64f;
	private const float OFFSET = DIVISION / 2f;
	private bool changed = true;
	private GameObject cubeCanvas;
	void Start () {
		renderer = GetComponent<SpriteRenderer>();
		renderer.sprite =
		Sprite.Create(baseTexture
				  ,new Rect(0, 0, baseTexture.width, baseTexture.height)
				  ,new Vector2(0.5f, 0.5f));
		GameObject parent = gameObject.transform.parent.gameObject;
		cubeCanvas = new GameObject();
		cubeCanvas.transform.parent = parent.transform;
		// move to bottom left corner to the lambda dispaly
		cubeCanvas.transform.position = new Vector3(transform.position.x - 1 , transform.position.y - 1 , 0);
		cubeCanvas.name = "Cube Canvas of " + name;

	}

    Vector3[] SpriteLocalToWorld(Sprite sp)
    {
        Vector3 pos = transform.position;
        Vector3[] array = new Vector3[2];
        //top left
        array[0] = pos + sp.bounds.min;
        // Bottom right
        array[1] = pos + sp.bounds.max;
        return array;
    }

    // Update is called once per frame
    void Update () {
		if(changed) {
			ApplyLambdaGrid();
			changed = false;
		}
	}

	public void UpdateLambdaGrid(LambdaGrid grid) {
	/* 
		if(grid.Equals(lambdaGrid)) {
			return;
		}
		*/
		lambdaGrid = grid;
		changed = true;
	}

	public LambdaGrid GetLambdaGrid() {
		return lambdaGrid;
	}

    private void ApplyLambdaGrid()
    {
		Debug.Log("REDRAW");
		if(lambdaGrid == null) return;
		//OR YOU KNOW, JUSt RENDER ON TOP OF IT
		/* ^
		   |
		   |
		   |
		   |_________> 
		 (0,0) */
		foreach(Transform child in cubeCanvas.gameObject.GetComponentInChildren<Transform>()) {
			Destroy(child.gameObject);
		}
        for (int i = 0; i < LambdaGrid.MAX_LAMBDA_GRID_HEIGHT; i++)
        {
            for (int j = 0; j < LambdaGrid.MAX_LAMBDA_GRID_WIDTH; j++)
            {
                Sprite next = LambdaGrid.LambdaGridItemToSprite(lambdaGrid.GetAt(i, j));
                if (next != null)
                {
					// Rendering on top of it
                    GameObject rdSubObj = new GameObject();
                    rdSubObj.name = "A Cube";
                    rdSubObj.transform.parent = cubeCanvas.transform;
                    rdSubObj.transform.localScale = new Vector3(LOCAL_SCALING, LOCAL_SCALING, 1);
                    // Realitve position to parent
                    rdSubObj.transform.localPosition = new Vector3(j * DIVISION + OFFSET, i * DIVISION + OFFSET, 0);
                    SpriteRenderer rdr = rdSubObj.AddComponent<SpriteRenderer>();
                    // Set relative position
                    rdr.sprite = next;
                    rdr.sortingOrder = 2;
				}

            }
        }
    }


}
