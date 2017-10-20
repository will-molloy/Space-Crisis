using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Reset the given scene.
/// </summary>
/// <author>Will Molloy</author>
public class ResetScene : MonoBehaviour {

    public GameController.PlayableScene SceneToReset;

    public void ResetCurrentScene()
    {
        // Set scenes persistence component to reset the scene when it awakes.
        GameController.SetShouldBeReset(SceneToReset, true);
        GameController.ClearPersistedDataForScene(SceneToReset);
        // Reload the scene.
        SceneManager.LoadScene(SceneToReset.ToString());
    }
}
