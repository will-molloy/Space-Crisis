using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class GameController 
{
    private static Dictionary<String, GameObject[]> savedSceneObjects = new Dictionary<string, GameObject[]>(); // Scene :: Objects, for persisting scene
    private static readonly string[] playableScenes = new string[] { "level1room1", "level1room2", "level1room3" };

    public static GameObject[] GetObjectsFor(string scene)
    {
        return savedSceneObjects[scene];
    }

    private static void SaveScenes() // DO NOT USE (yet)
    {
        Debug.Log("Saving scenes");
        foreach (string playableScene in playableScenes)
        {
            SaveScene(playableScene);
        }
    }

    public static void SaveScene(string sceneName) // Only works for LOADED scenes
    {
        Debug.Log("Saving: " + sceneName);
        Scene scene = SceneManager.GetSceneByName(sceneName);
        GameObject[] objects = scene.GetRootGameObjects();
        SaveObjectsFor(sceneName, scene.GetRootGameObjects());
    }

    public static void SaveObjectsFor(string scene, GameObject[] objects)
    {
        foreach(GameObject obj in objects)
        {
            MonoBehaviour.DontDestroyOnLoad(obj);
        }
        savedSceneObjects[scene] = objects;
    }

    public static void RestoreScene(string scene)
    {
        Debug.Log("Restoring: " + scene);
        GameObject[] savedObjects = GetObjectsFor(scene);
        GameObject[] currentObjects = SceneManager.GetSceneByName(scene).GetRootGameObjects();

        for (int i = 0; i < currentObjects.Length; i++)
        {
            currentObjects[i].transform.position = savedObjects[i].transform.position;
        }
    }

    internal static void AddItem(Item item)
    {
        Debug.Log("Not implemeneted");
    }
}
