using UnityEngine;
using System.Collections.Generic;
using System;

public class ScenePersistence : MonoBehaviour
{
    public GameController.PlayableScene thisScene;
    public bool resetSceneOnLoad { get; set; }

    void Awake()
    {
        if (resetSceneOnLoad)
            ResetScene(thisScene);
        RestoreScene(thisScene);
    }

    public void ResetScene(GameController.PlayableScene sceneName)
    {
        Debug.Log("Resseting: " + sceneName);
        MoveObjects(GameController.getInitialObjsPosFor(sceneName)); // initial, default positions
    }

    private void MoveObjects(Dictionary<string, Vector3> objPositions)
    {
        foreach (KeyValuePair<string, Vector3> objPos in objPositions)
        {
            Debug.Log("Moving:" + objPos.Key + ", to: " + objPos.Value);

            GameObject obj = GameObject.Find(objPos.Key);
            obj.transform.position = objPos.Value;
        }
    }

    public void RestoreScene(GameController.PlayableScene sceneName)
    {
        Debug.Log("Restoring: " + sceneName);
        MoveObjects(GameController.getSavedObjsPosFor(sceneName)); // saved, persisted positions
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

    public static implicit operator ScenePersistence(GameObject v)
    {
        throw new NotImplementedException();
    }
}
