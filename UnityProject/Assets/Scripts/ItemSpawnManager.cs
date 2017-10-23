using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using mattmc3.Common.Collections.Generic;

public class ItemSpawnManager : MonoBehaviour
{
    public GameController.PlayableScene ThisScene;
    private static ItemDataBaseList ItemDataBaseList;
    public int[] UniqueItemSpawnIds;
    public AudioClip ItemPickUpSound;
    private List<Vector3> ItemSpawnPositions;

    /// <summary>
    /// Spawns the given item at the given location.
    /// </summary>
    /// <param name="itemKey"></param> Items key in the database
    /// <param name="itemPickUpSound"></param> Items audio pick up sound
    /// <param name="itemPos"></param> Item position
    public static void spawnItem(int itemKey, AudioClip itemPickUpSound, Vector3 itemPos)
    {
        GameObject randomLootItemObject = (GameObject)Instantiate(ItemDataBaseList.itemList[itemKey].itemModel);
        PickUpItem pickUpItem = randomLootItemObject.AddComponent<PickUpItem>();
        pickUpItem.item = ItemDataBaseList.itemList[itemKey];
        pickUpItem.pickUpFX = itemPickUpSound;
        randomLootItemObject.transform.localPosition = itemPos;
    }

    // Use this for initialization
    public void Start()
    {
        ThisScene = GameController.GetActivePlayableScene();
        // Load item database
        ItemDataBaseList = (ItemDataBaseList)Resources.Load("ItemDatabase");

        // Load the item spawn positions - these are the child components with the tag "item-spawn"
        ItemSpawnPositions = new List<Vector3>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("item-spawn"))
                ItemSpawnPositions.Add(child.position);
        }
        GenerateAndSpawnItems();
    }

    /// <summary>
    /// Checks the GameController to see if the items have already been spawned,
    /// if they have retrieves them to ensure consistency.
    /// If not spawns the items.
    /// </summary>
    private void GenerateAndSpawnItems()
    {
        UniqueItemSpawnIds = UniqueItemSpawnIds.Distinct().ToArray();
        if (UniqueItemSpawnIds.Length < ItemSpawnPositions.Count)
            throw new System.Exception("Need more items to spawn in " + ThisScene.GetFileName());

        // Retrieve item Ids generated for this scene
        OrderedDictionary<int, bool> itemsToSpawn = ThisScene.GetItemsInScene();
        if (itemsToSpawn.Count < ItemSpawnPositions.Count)
        {
            // Items not yet generated:
            RandomiseItemSpawns();
            ThisScene.AddGeneratedItems(UniqueItemSpawnIds.Take(ItemSpawnPositions.Count).ToList()); // persist
            itemsToSpawn = ThisScene.GetItemsInScene(); // retrieve
        }

        // Spawn items
        int itemPosIndex = 0;
        foreach (int itemId in itemsToSpawn.Keys)
        {
            // Only spawn items not picked up
            if (!itemsToSpawn.GetValue(itemId)) // [] doesn't work with custom datastructure
            {
                spawnItem(itemId, ItemPickUpSound, ItemSpawnPositions[itemPosIndex]);
            }
            itemPosIndex++;
        }
    }

    private void RandomiseItemSpawns()
    {
        Debug.Log("Randomising items");
        System.Random r = new System.Random();
        UniqueItemSpawnIds = UniqueItemSpawnIds.OrderBy(x => r.Next()).ToArray();
    }
}
