using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemSpawnManager : MonoBehaviour
{
    public GameController.PlayableScene ThisScene;
    static ItemDataBaseList inventoryItemList;
    public int[] ItemKeyRange;
    public AudioClip pickUpFX;
    private List<Vector3> ItemSpawnsPositions;

    // Use this for initialization
    void Start()
    {
        ItemSpawnsPositions = new List<Vector3>();
        foreach (Transform t in transform)
        {
            if (t.CompareTag("item-spawn"))
                ItemSpawnsPositions.Add(t.position);
        }
        System.Random r = new System.Random();
        ItemKeyRange = ItemKeyRange.OrderBy(x => r.Next()).ToArray(); // RNG
        Spawn();
    }

    // Update is called once per frame
    void Spawn()
    {
        inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");

        for (int i = 0; i < ItemSpawnsPositions.Count; i++)
        {
            if (!(inventoryItemList.itemList[ItemKeyRange[i]].itemModel == null))
            {
                Debug.Log("Item " + ItemKeyRange[i] + " picked!");
                GameObject randomLootItem = (GameObject)Instantiate(inventoryItemList.itemList[ItemKeyRange[i]].itemModel);
                PickUpItem item = randomLootItem.AddComponent<PickUpItem>();
                item.item = inventoryItemList.itemList[ItemKeyRange[i]];
                item.pickUpFX = pickUpFX;
                randomLootItem.transform.localPosition = ItemSpawnsPositions[i];
                GameController.AddItemForScene(ThisScene, item); // Add item to GameController database
            }

        }

    }
}
