using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawnManager : MonoBehaviour {

    public Transform[] _spawns;
    static ItemDataBaseList inventoryItemList;
    public int[] _itemRange;
    public List<int> _spawnedItems;

    // Use this for initialization
    void Start () {
        _spawnedItems = new List<int>();
        Spawn();

}
	
	// Update is called once per frame
	void Spawn () {

        inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");

        for (int i = 0; i < _spawns.Length; i++)
        {
            int randomNumber = Random.Range(0, _itemRange.Length);

            while (_spawnedItems.Contains(randomNumber)) {
                randomNumber = Random.Range(0, _itemRange.Length);
            }

            _spawnedItems.Add(randomNumber);

            if (!(inventoryItemList.itemList[_itemRange[randomNumber]].itemModel == null))
            {

                Debug.Log("Item " + _itemRange[randomNumber] + " picked!");
                GameObject randomLootItem = (GameObject)Instantiate(inventoryItemList.itemList[_itemRange[randomNumber]].itemModel);
                PickUpItem item = randomLootItem.AddComponent<PickUpItem>();
                item.item = inventoryItemList.itemList[_itemRange[randomNumber]];

                randomLootItem.transform.localPosition = _spawns[i].position;
            }

        }

    }
}
