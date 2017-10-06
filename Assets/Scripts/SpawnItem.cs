using UnityEngine;
using System.Collections;

public class SpawnItem : MonoBehaviour {

	public Transform[] Spawns;

	public GameObject[] items;

	// Use this for initialization
	void Start () {
	
		Spawn ();
	}

	void Spawn() {
		for (int i = 0; i < Spawns.Length; i++) {
			int item = Random.Range (0,items.Length);
			Instantiate (items[item], Spawns [i].position, Quaternion.identity);
		}
	}

}
