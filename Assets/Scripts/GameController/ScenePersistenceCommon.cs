using UnityEngine;
using System.Collections.Generic;

abstract public class ScenePersistenceCommon : MonoBehaviour {

    protected int ofScene;
    private bool condition;
    private GameObject p1, p2;
    private Transform loc;

    abstract public void RestoreScene();
    abstract public void SaveScene();
    public void Awake()
    {
        if (GameController.GetInstance().GetLastScene() > ofScene)
        {
            p1 = GameObject.Find("Astronaut");
            p2 = GameObject.Find("Astronaut_2");
            loc = GameObject.Find("PositionBack").transform;
            p1.transform.position = loc.position;
            p2.transform.position = loc.position;
            Inventory inv = p1.GetComponent<PlayerInventory>().inventory.GetComponent<Inventory>();
            List<Item> l = GameController.GetInstance().GetListOfItems();
            l.ForEach(i => {
                Debug.Log("HAS ITEM " + i);
                inv.addItemToInventory(i.itemID, i.itemValue);
                inv.updateItemList();
                inv.stackableSettings();
            });


            RestoreScene();
        }

        GameController.GetInstance().SetLastScene(ofScene);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SaveScene();
    }


}
