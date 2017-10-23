using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Reset all scenes in given level and put players at first scene in level
/// </summary>
/// <author>Will Molloy</author>
public class ResetLevel : MonoBehaviour {

    public GameController.Level ThisLevel;

    public void ResetScenesInLevel()
    {
        GameController.PlayableScene? firstSceneInLevel = null;

        // Set all scenes in the level to reset when their scene persistance component awakes
        foreach (GameController.PlayableScene scene in ThisLevel.GetScenes())
        {
            if (!firstSceneInLevel.HasValue)
                firstSceneInLevel = scene;
            Debug.Log("Resseting: " + ThisLevel.ToString() + " ," + scene.ToString());
            scene.SetShouldBeReset(true);
        }
        ThisLevel.ClearScenesIncludingItems();
        // Load the first scene in the level
        if (firstSceneInLevel.HasValue)
        {
            SceneManager.LoadScene(GameController.GetFileName(firstSceneInLevel.Value));
        }
        else
        {
            throw new System.Exception("Could not retrieve first scene for: " + ThisLevel.ToString());
        }
    }
}
