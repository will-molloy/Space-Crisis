using UnityEngine;

public class ConsumeItem : MonoBehaviour
{
    public Item item;
    private static Tooltip tooltip;
    public ItemType[] itemTypeOfSlot;
    public static EquipmentSystem eS;
    public GameObject duplication;
    public static GameObject mainInventory;

    void Start()
    {
        item = GetComponent<ItemOnObject>().item;
        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
            tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
        if (GameObject.FindGameObjectWithTag("EquipmentSystem") != null)
        {
            foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (Obj.name == "Astronaut")
                {
                    eS = Obj.GetComponent<PlayerInventory>().characterSystem.GetComponent<EquipmentSystem>();
                }
            }
        }
        if (GameObject.FindGameObjectWithTag("MainInventory") != null)
            mainInventory = GameObject.FindGameObjectWithTag("MainInventory");
    }


    public void consumeIt()
    {
        Inventory inventory = transform.parent.parent.parent.GetComponent<Inventory>();

        bool gearable = false;

        if (GameObject.FindGameObjectWithTag("EquipmentSystem") != null)
            eS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().characterSystem.GetComponent<EquipmentSystem>();

        if (eS != null)
            itemTypeOfSlot = eS.itemTypeOfSlots;

        Item itemFromDup = null;
        if (duplication != null)
            itemFromDup = duplication.GetComponent<ItemOnObject>().item;

        bool stop = false;
        if (eS != null)
        {

            for (int i = 0; i < eS.slotsInTotal; i++)
            {
                if (itemTypeOfSlot[i].Equals(item.itemType))
                {
                    if (eS.transform.GetChild(1).GetChild(i).childCount == 0)
                    {
                        stop = true;
                        this.gameObject.transform.SetParent(eS.transform.GetChild(1).GetChild(i));
                        this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        eS.gameObject.GetComponent<Inventory>().updateItemList();
                        inventory.updateItemList();
                        inventory.EquiptItem(item);
                        gearable = true;
                        if (duplication != null)
                            Destroy(duplication.gameObject);
                        break;
                    }
                }
            }

            if (!stop)
            {
                for (int i = 0; i < eS.slotsInTotal; i++)
                {
                    if (itemTypeOfSlot[i].Equals(item.itemType))
                    {
                        if (eS.transform.GetChild(1).GetChild(i).childCount != 0)
                        {
                            GameObject otherItemFromCharacterSystem = eS.transform.GetChild(1).GetChild(i).GetChild(0).gameObject;
                            Item otherSlotItem = otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item;
                            if (item.itemType == ItemType.UFPS_Weapon)
                            {
                                inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item);
                                inventory.EquiptItem(item);
                            }
                            else
                            {
                                inventory.EquiptItem(item);
                                if (item.itemType != ItemType.Backpack)
                                    inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item);
                            }
                            if (this == null)
                            {
                                GameObject dropItem = (GameObject)Instantiate(otherSlotItem.itemModel);
                                dropItem.AddComponent<PickUpItem>();
                                dropItem.GetComponent<PickUpItem>().item = otherSlotItem;
                                dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
                                inventory.OnUpdateItemList();
                            }
                            else
                            {
                                otherItemFromCharacterSystem.transform.SetParent(this.transform.parent);
                                otherItemFromCharacterSystem.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                if (this.gameObject.transform.parent.parent.parent.GetComponent<Hotbar>() != null)
                                    createDuplication(otherItemFromCharacterSystem);

                                this.gameObject.transform.SetParent(eS.transform.GetChild(1).GetChild(i));
                                this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                            }

                            gearable = true;
                            if (duplication != null)
                                Destroy(duplication.gameObject);
                            eS.gameObject.GetComponent<Inventory>().updateItemList();
                            inventory.OnUpdateItemList();
                            break;
                        }
                    }
                }
            }


        }
        if (!gearable && item.itemType != ItemType.UFPS_Ammo && item.itemType != ItemType.UFPS_Grenade)
        {

            if (duplication != null)
                itemFromDup = duplication.GetComponent<ItemOnObject>().item;

            inventory.ConsumeItem(item);

            item.itemValue--;
            if (itemFromDup != null)
            {
                duplication.GetComponent<ItemOnObject>().item.itemValue--;
                if (itemFromDup.itemValue <= 0)
                {
                    if (tooltip != null)
                        tooltip.deactivateTooltip();
                    inventory.deleteItemFromInventory(item);
                    Destroy(duplication.gameObject);

                }
            }
            if (item.itemValue <= 0)
            {
                if (tooltip != null)
                    tooltip.deactivateTooltip();
                inventory.deleteItemFromInventory(item);
                Destroy(this.gameObject);
            }
        }
    }

    public void createDuplication(GameObject Item)
    {
        Item item = Item.GetComponent<ItemOnObject>().item;
        GameObject dup = mainInventory.GetComponent<Inventory>().addItemToInventory(item.itemID, item.itemValue);
        Item.GetComponent<ConsumeItem>().duplication = dup;
        dup.GetComponent<ConsumeItem>().duplication = Item;
    }
}
