using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	public int maxPlatforms = 20;
	public GameObject platform;
	public float horizontalMin = 6.5f; 
	public float horizontalMax = 14f;
	public float verticalMin = -6f;
	public float verticalMax = 6f;

	private Vector2 originPosition;

	// Use this for initialization
	void Start () {
		originPosition = transform.position;
		Spawn ();
	}
	
	void Spawn(){
		for (int i = 0; i < maxPlatforms; i++) {
			Vector2 randomPosition = originPosition + new Vector2 (Random.Range (horizontalMin, horizontalMax), Random.Range (verticalMin, verticalMax));
			Instantiate (platform, randomPosition, Quaternion.identity); //Quaternion.identity means no rotation
			originPosition = randomPosition; //Next time when we run the loop we will use the last random position and run the offset there, drawing a chain of randomly spaced platforms.
		}
	}

}
