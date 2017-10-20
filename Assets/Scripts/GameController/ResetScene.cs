using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetScene : MonoBehaviour {

    public GameController.Level LevelToReset;
    public GameController.PlayableScene SceneToReset;

    /**
     * Reset all scenes in given level and put players at first scene in level
     */
    public void ResetScenesInLevel()
    {
        GameController.PlayableScene? firstSceneInLevel = null;

        // Set all scenes in the level to reset when scene persistance awakes
        foreach(GameController.PlayableScene scene in GameController.getScenesForLevel(LevelToReset))
        {
            if (firstSceneInLevel == null)
                firstSceneInLevel = scene;          
            Debug.Log("Resseting: " + LevelToReset.ToString() + " ," + scene.ToString());
            GameController.setResetSceneAttributeFor(scene, true);
        }
        GameController.clearScenesForLevel(LevelToReset);
        SceneManager.LoadScene(firstSceneInLevel.ToString());
    }
    /**
     * Reset the given scene only 
     */
    public void ResetCurrentScene()
    {
        GameController.setResetSceneAttributeFor(SceneToReset, true);
        GameController.ClearPersistedDataForScene(SceneToReset);
        SceneManager.LoadScene(SceneToReset.ToString());
    }
}
