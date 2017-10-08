using UnityEngine;
using System.Collections;
using System;

public class GameController {

    private static GameController instance;

    private int lastScene;

    private System.Object[] savedObjects = new System.Object[100];
   
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

    public void SetSavedObjectFor(int of, System.Object obj)
    {
        savedObjects[of] = obj;
    }

    public System.Object GetSavedObjectFor(int of)
    {
        return savedObjects[of];
    }

}
