using UnityEngine;
using System.Collections;
public class PickUpItem : MonoBehaviour
{
    public Item item;
    private Inventory _inventory;
    private GameObject _player1;
	private GameObject _player2;
    // Use this for initialization

    void Start()
    {

		foreach(GameObject Obj in GameObject.FindGameObjectsWithTag("Player"))
		{
			if(Obj.name == "Astronaut")
			{
				_player1 = Obj;


			} else if(Obj.name == "Astronaut_2") {
				_player2 = Obj;
			}

		}
			
        if (_player1 != null)
            _inventory = _player1.GetComponent<PlayerInventory>().inventory.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inventory != null && Input.GetKeyDown(KeyCode.E))
        {
            float distance1 = Vector3.Distance(this.gameObject.transform.position, _player1.transform.position);
			float distance2 = Vector3.Distance(this.gameObject.transform.position, _player2.transform.position);

			if (distance1 <= 3 || distance2 <= 3)
            {
                bool check = _inventory.checkIfItemAllreadyExist(item.itemID, item.itemValue);
                if (check)
                    Destroy(this.gameObject);
                else if (_inventory.ItemsInInventory.Count < (_inventory.width * _inventory.height))
                {
                    _inventory.addItemToInventory(item.itemID, item.itemValue);
                    _inventory.updateItemList();
                    _inventory.stackableSettings();
                    GameController.AddItem(item);
                    Destroy(this.gameObject);
                }

            }
        }
    }

}