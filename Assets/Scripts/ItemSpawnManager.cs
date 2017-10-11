using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemSpawnManager : MonoBehaviour {

    public Transform[] _spawns;
    static ItemDataBaseList inventoryItemList;
    public int[] _itemRange;
    public List<int> _spawnedItems;

    // Use this for initialization
    void Start () {
		System.Random r = new System.Random();
		_itemRange = _itemRange.OrderBy(x => r.Next()).ToArray();
        _spawnedItems = new List<int>();
        Spawn();

}
	
	// Update is called once per frame
	void Spawn () {

        inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");

        for (int i = 0; i < _spawns.Length; i++)
        {

			if (!(inventoryItemList.itemList[_itemRange[i]].itemModel == null))
            {

				Debug.Log("Item " + _itemRange[i] + " picked!");
				GameObject randomLootItem = (GameObject)Instantiate(inventoryItemList.itemList[_itemRange[i]].itemModel);
                PickUpItem item = randomLootItem.AddComponent<PickUpItem>();
				item.item = inventoryItemList.itemList[_itemRange[i]];

                randomLootItem.transform.localPosition = _spawns[i].position;
            }

        }

    }
}
