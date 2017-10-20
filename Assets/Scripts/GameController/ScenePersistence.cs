using UnityEngine;
using System.Collections.Generic;

public class ScenePersistence : MonoBehaviour
{
    public GameController.PlayableScene thisScene;
    public static bool resetScene;

    void Awake()
    {
        RestoreScene(thisScene);
    }

    public void RestoreScene(GameController.PlayableScene sceneName)
    {
        Debug.Log("Restoring: " + sceneName);

        foreach (KeyValuePair<string, Vector3> objPos in GameController.getSavedObjsPosFor(sceneName))
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
        GameController.SaveObjsPosFor(thisScene, getChildObjs(transform));
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
