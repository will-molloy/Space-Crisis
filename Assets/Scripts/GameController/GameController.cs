using UnityEngine;
using System;
using System.Collections.Generic;

public class GameController
{
    private static Dictionary<string, Dictionary<String, Vector3>> savedScenePositions; // Scene.name :: Object.name :: Position
    private static readonly string[] playableScenes = new string[] { "level1room1", "level1room2", "level1room3" };
    private static GameController instance;

    private GameController()
    {
        savedScenePositions = new Dictionary<string, Dictionary<string, Vector3>>();
        foreach (string playableScene in playableScenes)
        {
            Dictionary<String, Vector3> sceneInitialPos = new Dictionary<string, Vector3>();
            savedScenePositions[playableScene] = sceneInitialPos;
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
        }
    }

    public Dictionary<String, Vector3> getSavedObjPosFor(string sceneName)
    {
        return savedScenePositions[sceneName];
    }

    internal void AddItem(Item item)
    {
        Debug.Log("Not implemeneted");
    }
}
