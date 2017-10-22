using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneUtility : MonoBehaviour {

    public GameController.PlayableScene SceneToLoad;

    public void LoadScene()
    {
        SceneManager.LoadScene(GameController.GetFileName(SceneToLoad));
    }
}
