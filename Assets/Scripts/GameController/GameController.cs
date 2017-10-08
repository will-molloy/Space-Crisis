using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class GameController {

    private static GameController instance;

    private int lastScene;

    private Dictionary<int, object> savedObjects = new Dictionary<int, object>();
   
    private GameController()
    {
        lastScene = 0;
    }

    public static GameController GetInstance()
    {
        if(instance == null)
        {
            instance = new GameController();
        }
        return instance;
    }

    public void SetLastScene(int a)
    {
        lastScene = a;
    }

    public int GetLastScene()
    {   
        return lastScene;
    }

    public void SetSavedObjectFor(int of, object obj)
    {
        
        savedObjects[of] = obj;
    }

    public object GetSavedObjectFor(int of)
    {
        return savedObjects[of];
    }

}
