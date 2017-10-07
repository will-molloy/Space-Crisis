using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    public string firstSceneToLoad = "level1room1";

    public void LoadByIndex(int sceneIndex) {
        SceneManager.LoadScene(firstSceneToLoad);
    }
	
}
