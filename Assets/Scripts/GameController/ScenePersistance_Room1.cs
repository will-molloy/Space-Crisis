using UnityEngine;
using System.Collections;
using System;
public class ScenePersistance_Room1 : ScenePersistenceCommon {

    public void Awake()
    {
        ofScene = 0;
        base.Awake();
    }

	// Use this for initialization
	void Start () {
	
	}
	
    public override void RestoreScene()
    {
        object obj = GameController.GetInstance().GetSavedObjectFor(ofScene);
        if(obj != null)
        {
            GameObject.Find("Box").transform.position = ((PSV)obj).boxPos;
        }
    }

    public override void SaveScene()
    {
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
