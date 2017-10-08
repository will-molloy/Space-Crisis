using UnityEngine;
using System.Collections;

public class GameController {

    private static GameController instance;

    private int lastScene;
   
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

}
