using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class ResetScene : MonoBehaviour {

    public GameController.LevelAttribute.Level LevelToReset;

    void ResetLevel()
    {
        // Set all scenes in the level to not reset when scene persistance wakes
        foreach(GameController.PlayableScene scene in GameController.getScenesForLevel(LevelToReset))
        {
            foreach (GameObject obj in SceneManager.GetSceneByName(scene.ToString()).GetRootGameObjects().Where(x => x.CompareTag("Persistence")).ToList())
            {
                ScenePersistence persistence = obj.GetComponent<ScenePersistence>();
                persistence.resetSceneOnLoad = true;
            }
        }
    }
}
