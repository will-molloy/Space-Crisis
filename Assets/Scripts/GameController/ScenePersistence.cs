using UnityEngine;
using System.Collections.Generic;

public class ScenePersistence : MonoBehaviour
{

    public string thisScene;

    public void Awake()
    {
        Debug.Log("Restoring: " + thisScene);

        foreach (KeyValuePair<string, Vector3> objPos in GameController.getInstance().getSavedObjPosFor(thisScene))
        {
            Debug.Log("Restoring: Key :" + objPos.Key + ", Value: " + objPos.Value);

            GameObject obj = GameObject.Find(objPos.Key);
            obj.transform.position = objPos.Value;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SaveScene();
    }

    public void SaveScene()
    {
        Debug.Log("Saving: " + thisScene);
        GameController.getInstance().SaveObjsPosFor(thisScene, getChildObjs(transform));
    }

    private List<GameObject> getChildObjs(Transform transform)
    {
        List<GameObject> objs = new List<GameObject>();
        foreach (Transform t in transform)
        {
            objs.Add(t.gameObject);
            objs.AddRange(getChildObjs(t)); // recursively add child components
        }
        return objs;
    }
}
