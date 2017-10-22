using UnityEngine;
using System.Collections;

public class SpawnItem : MonoBehaviour {

	public Transform[] _spawns;
	static ItemDataBaseList inventoryItemList;
	public int[] _itemRange;
	public AudioClip pickUpFX;

	// Use this for initialization
	void Start () {
	
		Spawn ();
	}

	void Spawn() {

		inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");

		for (int i = 0; i < _spawns.Length; i++) {
			int randomNumber = Random.Range (0,_itemRange.Length);

			if (!(inventoryItemList.itemList[_itemRange[randomNumber]].itemModel == null)) {

				Debug.Log ("Item " + _itemRange[randomNumber] + " picked!");
				GameObject randomLootItem = (GameObject)Instantiate(inventoryItemList.itemList[_itemRange[randomNumber]].itemModel);
				PickUpItem item = randomLootItem.AddComponent<PickUpItem>();
				item.item = inventoryItemList.itemList[_itemRange[randomNumber]];
				item.pickUpFX = pickUpFX;
				randomLootItem.transform.localPosition = _spawns [i].position;
			}
				
		}
	}

}
