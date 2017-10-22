using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using mattmc3.Common.Collections.Generic;

public class ItemSpawnManager : MonoBehaviour
{
    public GameController.PlayableScene ThisScene;
    static ItemDataBaseList inventoryItemList;
    public int[] ItemIdRange;
    public AudioClip pickUpFX;
    private List<Vector3> ItemSpawnsPositions;

    // Use this for initialization
    void Start()
    {
        // Load the item spawn positions
        ItemSpawnsPositions = new List<Vector3>();
        foreach (Transform t in transform)
        {
            if (t.CompareTag("item-spawn"))
                ItemSpawnsPositions.Add(t.position);
        }
        // Load the item database
        inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");

        Spawn();
    }

    /// <summary>
    /// Checks the GameController to see if the items have already been spawned,
    /// if they have retrieves them to ensure consistency.
    /// If not spawns the items.
    /// </summary>
    private void Spawn()
    {
        if (ItemIdRange.Length < ItemSpawnsPositions.Count)
            throw new System.Exception("Need more items to spawn in " + ThisScene.GetFileName());

        OrderedDictionary<int, bool> itemsToSpawn = GameController.GetItems(ThisScene);
        if (itemsToSpawn.Count < ItemSpawnsPositions.Count)
        {
            RandomiseItemSpawns();
            for (int i = 0; i < ItemSpawnsPositions.Count; i++)
            {
                itemsToSpawn.Add(ItemIdRange[i], false); // generating item; not picked up
            }
            int itemPosIndex = 0;
            foreach(int itemId in itemsToSpawn.Keys)
            {
                GameObject randomLootItemObject = (GameObject)Instantiate(inventoryItemList.itemList[itemId].itemModel);
                PickUpItem pickUpItem = randomLootItemObject.AddComponent<PickUpItem>();
                Object.DontDestroyOnLoad(pickUpItem);
                pickUpItem.item = inventoryItemList.itemList[itemId];
                pickUpItem.pickUpFX = pickUpFX;
                randomLootItemObject.transform.localPosition = ItemSpawnsPositions[itemPosIndex++];
            }
        }

        //RandomiseItemSpawns();
        //// Retrieve items already spawned in this scene
        //Dictionary<Vector3, PickUpItem> ItemsPersistedInThisScene = GameController.GetItemsFor(ThisScene);

        //for (int i = 0; i < ItemSpawnsPositions.Count; i++)
        //{
        //    if (ItemsPersistedInThisScene.ContainsKey(ItemSpawnsPositions[i]))
        //    {
        //        // Item has already spawned before 
        //        PickUpItem persistedPickUpItem = ItemsPersistedInThisScene[ItemSpawnsPositions[i]];
        //        if (!WasPickedUp(persistedPickUpItem))
        //        {
        //            // Item hasn't been picked up - respawn the exact same item
        //            GameObject randomLootItemObject = (GameObject)Instantiate(inventoryItemList.itemList[ItemKeyRange[i]].itemModel);
        //            PickUpItem newPickUpItem = randomLootItemObject.AddComponent<PickUpItem>();
        //            newPickUpItem.item = persistedPickUpItem.item;
        //            newPickUpItem.pickUpFX = pickUpFX;
        //            randomLootItemObject.transform.localPosition = persistedPickUpItem.transform.localPosition;
        //        }
        //    }
        //    else
        //    {
        //        // Item hasn't yet been spawned
        //        GameObject randomLootItemObject = (GameObject)Instantiate(inventoryItemList.itemList[ItemKeyRange[i]].itemModel);
        //        PickUpItem pickUpItem = randomLootItemObject.AddComponent<PickUpItem>();
        //        Object.DontDestroyOnLoad(pickUpItem);
        //        pickUpItem.item = inventoryItemList.itemList[ItemKeyRange[i]];
        //        pickUpItem.pickUpFX = pickUpFX;
        //        randomLootItemObject.transform.localPosition = ItemSpawnsPositions[i];
        //        GameController.AddItemToScene(ThisScene, ItemSpawnsPositions[i], pickUpItem); // Add item to GameController database
        //    }
        //}
    }

    private void RandomiseItemSpawns()
    {
        Debug.Log("Randomising items");
        System.Random r = new System.Random();
        ItemIdRange = ItemIdRange.OrderBy(x => r.Next()).ToArray();
    }

    private bool WasPickedUp(PickUpItem gameObject)
    {
        return gameObject == null && !ReferenceEquals(gameObject, null);
    }
}
