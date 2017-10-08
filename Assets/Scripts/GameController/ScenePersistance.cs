using UnityEngine;
using System.Collections;

public class ScenePersistance : MonoBehaviour {

    public int ofScene;
    private bool condition;
    private GameObject p1, p2;
    private Transform loc;
    void Awake()
    {
        Debug.Log("THE THING AWAKENS!");
        Debug.Log("LAST SCENCE ID " + GameController.GetInstance().GetLastScene());
        if (GameController.GetInstance().GetLastScene() > ofScene)
        {
            Debug.Log("TRIHARD TRIHARD");
            p1 = GameObject.Find("Astronaut");
            p2 = GameObject.Find("Astronaut_2");
            loc = GameObject.Find("PositionBack").transform;
            p1.transform.position = loc.position;
            p2.transform.position = loc.position;
        }

        GameController.GetInstance().SetLastScene(ofScene);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
