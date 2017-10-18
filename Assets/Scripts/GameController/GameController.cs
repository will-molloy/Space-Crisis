using UnityEngine;
using System;
using System.Collections.Generic;

public class GameController
{
    // Scene.name :: Object.name :: Position, For persisting given scene objects
    private static Dictionary<string, Dictionary<string, Vector3>> savedScenePositions;

    // For resseting scene e.g. level restart
    private static Dictionary<string, Dictionary<string, Vector3>> initialScenePositions; 

    private static readonly string[] playableScenes = new string[] { "level1room1", "level1room2", "level1room3" };
    private static GameController instance;

    private GameController()
    {
        initialScenePositions = new Dictionary<string, Dictionary<string, Vector3>>();
        savedScenePositions = new Dictionary<string, Dictionary<string, Vector3>>();
        foreach (string playableScene in playableScenes)
        {
            Dictionary<String, Vector3> scenePos = new Dictionary<string, Vector3>();
            savedScenePositions[playableScene] = scenePos;
            initialScenePositions[playableScene] = scenePos;
        }
    }

    public static GameController getInstance()
    {
        if (instance == null)
            instance = new GameController();
        return instance;
    }

    public void SaveObjsPosFor(string sceneName, List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            savedScenePositions[sceneName][obj.name] = obj.transform.position;
            if (!initialScenePositions[sceneName].ContainsKey(obj.name)) // write once
                initialScenePositions[sceneName][obj.name] = obj.transform.position;
        }
    }

    public Dictionary<String, Vector3> getSavedObjsPosFor(string sceneName)
    {
        return savedScenePositions[sceneName];
    }

    public Dictionary<String, Vector3> getInitialObjsPosFor(string sceneName)
    {
        return initialScenePositions[sceneName];
    }

    internal void AddItem(Item item)
    {
        Debug.Log("Not implemeneted");
    }
}
