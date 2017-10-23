using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneUtility : MonoBehaviour {

    public GameController.PlayableScene SceneToLoad;

    public void LoadScene()
    {
        if (SceneToLoad == GameController.PlayableScene.None)
            throw new System.Exception("Please set ThisScene");
        SceneManager.LoadScene(GameController.GetFileName(SceneToLoad));
    }
}
