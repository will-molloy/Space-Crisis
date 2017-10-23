using UnityEngine;
using System.Collections.Generic;
using System;
using mattmc3.Common.Collections.Generic;

/// <summary>
/// Usage: Create Empty component with this script and set appropriate variables.
/// Set all components that should be persisted as children.
/// Depending on the GameController state this script will restore or reset the scene on load.
/// </summary>
/// <author>Will Molloy</author>
public class ScenePersistence : MonoBehaviour
{
    public GameController.PlayableScene thisScene;

    /// <summary>
    /// Restore or reset the scene.
    /// </summary>
    public void Start()
    {
        if (thisScene == GameController.PlayableScene.None)
            throw new System.Exception("Please set ThisScene in inventory panel");
        // Determine if scene should be restored or reset
        if (thisScene.GetShouldBeReset())
            ResetScene();
        RestoreScene();

        // Retrieve player
        Vector3 playerPosition = new Vector3();
        foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Obj.name == "Astronaut")
                playerPosition = Obj.transform.position;
        }

        // Retreive all items the players have picked up
        foreach (int itemId in GameController.GetItemsPickedUp())
        {
            ItemSpawnManager.spawnItem(itemId, null, playerPosition);
        }
        
    }

    private void ResetScene()
    {
        MoveObjects(thisScene.GetInitialObjectPositions()); // move objects to initial, default positions
		AudioManager.loadAudio();
        thisScene.SetShouldBeReset(false); // scene has now been reset, set back to false
    }

    private void RestoreScene()
    {
		AudioManager.loadAudio();
        MoveObjects(thisScene.GetSavedObjectPositons()); // move objects to saved, persisted positions
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
        thisScene.SaveObjectPositions(getChildObjs(transform));
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
