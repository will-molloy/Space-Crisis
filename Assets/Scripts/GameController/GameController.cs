using UnityEngine;
using System;
using System.Collections.Generic;


public class GameController
{

    private static GameController instance;
    private Dictionary<String, GameObject[]> savedSceneObjects = new Dictionary<string, GameObject[]>(); // Scene :: Objects, for persisting scene

    private GameController() { }

    public static GameController GetInstance()
    {
        if (instance == null)
            instance = new GameController();
        return instance;
    }


    public void SaveObjectsFor(string scene, GameObject[] objects)
    {
        savedSceneObjects[scene] = objects;
    }

    public GameObject[] GetObjectsFor(string scene)
    {
        return savedSceneObjects[scene];
    }

}
