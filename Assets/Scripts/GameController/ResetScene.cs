﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour {

    public GameController.PlayableScene SceneToReset;
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