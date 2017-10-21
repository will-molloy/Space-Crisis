using UnityEngine;
using System.Collections;

public class KeepStead : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 eulers = transform.eulerAngles;
        eulers.x = 0;
        transform.eulerAngles = eulers;
    }
}
