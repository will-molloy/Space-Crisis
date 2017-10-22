using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Usage: Create Empty component with this script and set appropriate variables.
/// Set all components that should be persisted as children.
/// Depending on the GameController state this script will restore or reset the scene on load.
/// </summary>
/// <author>Will Molloy</author>
public class ScenePersistence : MonoBehaviour
{
    public GameController.PlayableScene thisScene;
    private static Inventory inventory;
    private GameObject player;

    void Start()
    {

    }

    /// <summary>
    /// Restore or reset the scene.
    /// </summary>
    void Awake()
    {
        // Determine if scene should be restored or reset
        if (GameController.GetShouldBeReset(thisScene))
            ResetScene();
        RestoreScene();

        foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Obj.name == "Astronaut")
                player = Obj;
        }
        if (player != null)
            inventory = player.GetComponent<PlayerInventory>().inventory.GetComponent<Inventory>();

        // Add any inventory items (from other scenes) to this scenes inventory
        GameController.GetInventoryItems().ForEach(item =>
        {
            Debug.Log("persisting item in inventory: " + item.itemName);
            inventory.addItemToInventory(item.itemID, item.itemValue);
            inventory.updateItemList();
            inventory.stackableSettings();
        });
        
    }

    private void ResetScene()
    {
        MoveObjects(GameController.GetInitialObjectPositions(thisScene)); // move objects to initial, default positions
		AudioManager.loadAudio();
        GameController.SetShouldBeReset(thisScene, false); // scene has now been reset, set back to false
    }

    private void RestoreScene()
    {
		AudioManager.loadAudio();
        MoveObjects(GameController.GetSavedObjectPositons(thisScene)); // move objects to saved, persisted positions
    }

    private void MoveObjects(Dictionary<string, Vector3> objPositions)
    {
        foreach (KeyValuePair<string, Vector3> objPos in objPositions)
        {
            GameObject obj = GameObject.Find(objPos.Key);
            obj.transform.position = objPos.Value;
        }
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        SaveScene();
    }

    private void SaveScene()
    {
        GameController.SaveObjectPositions(thisScene, getChildObjs(transform));
    }

    private List<GameObject> getChildObjs(Transform transform)
    {
        List<GameObject> objs = new List<GameObject>();
        foreach (Transform t in transform)
        {
            objs.Add(t.gameObject);
            objs.AddRange(getChildObjs(t)); // recursively add child components
        }
        return objs;
    }
}
