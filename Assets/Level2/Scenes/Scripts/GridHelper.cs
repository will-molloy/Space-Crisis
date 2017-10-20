using UnityEngine;
using System.Collections;

public class GridHelper : MonoBehaviour {

	public GridHelper instance;

	[SerializeField]
	private static Vector3 origin = Vector3.zero; //0,0,0

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy(gameObject);
			return;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
