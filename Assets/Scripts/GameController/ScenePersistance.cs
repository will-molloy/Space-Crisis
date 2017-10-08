using UnityEngine;
using System.Collections;
using System;

public class ScenePersistance : MonoBehaviour {

    public int ofScene;
    private bool condition;
    private GameObject p1, p2;
    private Transform loc;
    void Awake()
    {
        if (GameController.GetInstance().GetLastScene() > ofScene)
        {
            p1 = GameObject.Find("Astronaut");
            p2 = GameObject.Find("Astronaut_2");
            loc = GameObject.Find("PositionBack").transform;
            p1.transform.position = loc.position;
            p2.transform.position = loc.position;

            PSV p = (PSV)GameController.GetInstance().GetSavedObjectFor(ofScene);
            if ( p != null ) {
                GameObject.Find("Box").transform.position = p.boxPos;
            }
        }

        GameController.GetInstance().SetLastScene(ofScene);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        PSV p = new PSV();
        Vector3 tmp = GameObject.Find("Box").transform.position;
        p.boxPos = new Vector3(tmp.x, tmp.y, tmp.z);
        GameController.GetInstance().SetSavedObjectFor(ofScene, p);
	}

    private class PSV
    {
        public Vector3 boxPos;
    }
}
