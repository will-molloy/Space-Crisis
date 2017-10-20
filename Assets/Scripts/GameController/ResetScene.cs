using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Reset the given scene.
/// </summary>
/// <author>Will Molloy</author>
public class ResetScene : MonoBehaviour {

    public GameController.PlayableScene ThisScene;

    public void ResetCurrentScene()
    {
        // Set scenes persistence component to reset the scene when it awakes.
        GameController.SetShouldBeReset(ThisScene, true);
        GameController.ClearPersistedDataForScene(ThisScene);
        // Reload the scene.
        SceneManager.LoadScene(ThisScene.ToString());
    }
}
