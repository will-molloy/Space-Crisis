using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackToMainGame : MonoBehaviour {

    public GameController.PlayableScene sceneToLoad;

    // Use this for initialization
   public void loadScene () {
        SceneManager.LoadScene(GameController.GetFileName(sceneToLoad));
    }

}
