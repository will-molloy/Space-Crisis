using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour {

    public GameController.Level LevelToReset;

    /**
     * Reset all scenes in given level and put players at first scene in level
     */
    public void ResetScenesInLevel()
    {
        GameController.PlayableScene? firstSceneInLevel = null;

        // Set all scenes in the level to reset when scene persistance awakes
        foreach (GameController.PlayableScene scene in GameController.getScenesForLevel(LevelToReset))
        {
            if (firstSceneInLevel == null)
                firstSceneInLevel = scene;
            Debug.Log("Resseting: " + LevelToReset.ToString() + " ," + scene.ToString());
            GameController.SetShouldBeReset(scene, true);
        }
        GameController.clearScenesForLevel(LevelToReset);
        SceneManager.LoadScene(firstSceneInLevel.ToString());
    }
}
