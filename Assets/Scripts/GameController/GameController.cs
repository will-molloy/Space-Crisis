using UnityEngine;
using System;
using System.Collections.Generic;


public class GameController
{

    private static GameController instance;
    private Dictionary<String, GameObject[]> savedSceneObjects = new Dictionary<string, GameObject[]>(); // Scene :: Objects, for persisting scene

    private int lastScene;

    private Dictionary<int, object> savedObjects = new Dictionary<int, object>();

    private List<Item> savedItems = new List<Item>();
   
    private GameController()
    {
        lastScene = 0;
    }

    public void SaveListOfItems(List<Item> list)
    {
        savedItems = list.ConvertAll(i => i.getCopy());
        Debug.Log("SAVED ITEMS" + savedItems);
    }

    public void AddItem(Item i)
    {
        Debug.Log("SAVED ITEM " + i);
        savedItems.Add(i.getCopy());
    }

    public List<Item> GetListOfItems()
    {
        return savedItems;
    }

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
