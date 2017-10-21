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

    // Update is called once per frame
    void FixedUpdate()
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

    /// <summary>
    /// Depending on the GameController state the current scene will be restored or reset.
    /// </summary>
    void Awake()
    {
        if (GameController.GetShouldBeReset(thisScene)) 
            ResetScene(thisScene);
        RestoreScene(thisScene);
    }

    private void ResetScene(GameController.PlayableScene sceneName)
    {
        Debug.Log("Resseting: " + sceneName);
        MoveObjects(GameController.GetInitialObjectPositions(sceneName)); // move objects to initial, default positions
        GameController.SetShouldBeReset(sceneName, false); // scene has now been reset, set back to false
    }

    private void RestoreScene(GameController.PlayableScene sceneName)
    {
        Debug.Log("Restoring: " + sceneName);
        MoveObjects(GameController.GetSavedObjectPositons(sceneName)); // move objects to saved, persisted positions
    }

    private void MoveObjects(Dictionary<string, Vector3> objPositions)
    {
        foreach (KeyValuePair<string, Vector3> objPos in objPositions)
        {
            Debug.Log("Moving:" + objPos.Key + ", to: " + objPos.Value);
            GameObject obj = GameObject.Find(objPos.Key);
            obj.transform.position = objPos.Value;
        }
    }
}
