using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using mattmc3.Common.Collections.Generic;

public class ItemManagerAndPersistenceTest
{

    private const GameController.PlayableScene testScene = GameController.PlayableScene.Level1Room1;
    private GameObject itemSpawner;
    private ItemSpawnManager itemSpawnManager;

    [SetUp]
    public void Init()
    {
        itemSpawner = new GameObject();
        itemSpawnManager = itemSpawner.AddComponent<ItemSpawnManager>();
        itemSpawnManager.ThisScene = testScene;
    }

    private Vector3 createItemSpawnPosition(Vector3 vector)
    {
        GameObject gameObject = new GameObject();
        gameObject.tag = "item-spawn";
        gameObject.transform.parent = itemSpawnManager.transform; // Link with itemSpawnManager
        gameObject.transform.position = vector;
        return vector;
    }

    [Test]
    public void TestSpawnItemOnlyOnce()
    {
        // Add item Ids to spawn -- 1 is duplicated and should only be spawned once
        itemSpawnManager.UniqueItemSpawnIds = new int[] { 1, 1, 2, 3, 4 };
        // Add spawn positions
        createItemSpawnPosition(new Vector3(0, 0, 0));
        createItemSpawnPosition(new Vector3(1, 1, 1));
        createItemSpawnPosition(new Vector3(2, 2, 2));
        createItemSpawnPosition(new Vector3(3, 3, 3));
        // Spawn generate the item spawn order
        itemSpawnManager.Start();

        // Retrive the items 
        OrderedDictionary<int, bool> itemsInScene = GameController.GetItemsInScene(testScene);

        Assert.AreEqual(4, itemsInScene.Count);
    }

    [Test]
    public void SpawItemsInsufficientItemsTest()
    {
        itemSpawnManager.UniqueItemSpawnIds = new int[] { 1, 1, 4 }; // only two items to spawn
        createItemSpawnPosition(new Vector3(0, 0, 0));
        createItemSpawnPosition(new Vector3(1, 1, 1));
        createItemSpawnPosition(new Vector3(2, 2, 2));
        try
        {
            itemSpawnManager.Start();
            Assert.Fail();
        }
        catch (System.Exception e)
        {
            Assert.AreEqual("Need more items to spawn in " + testScene.GetFileName(), e.Message);
        }
    }
}
