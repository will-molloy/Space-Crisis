using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Level2PlayerControl : MonoBehaviour {

    public string horizontal_key;
    public string vertical_key;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int horizontal = (int)Input.GetAxisRaw(horizontal_key);
        int vertical = (int)Input.GetAxisRaw(vertical_key);
	}
}
