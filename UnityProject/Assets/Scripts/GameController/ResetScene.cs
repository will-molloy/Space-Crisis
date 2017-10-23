using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Reset the given scene.
/// </summary>
/// <author>Will Molloy</author>
public class ResetScene : MonoBehaviour {

    public GameController.PlayableScene ThisScene;
	LifeSystem life;

	void Awake () {
		GameObject team = GameObject.FindGameObjectWithTag("Team");
		life = team.GetComponent<LifeSystem> ();
	}

    public void ResetCurrentScene()
	{
		if (!life.isDead) {
			// Set scenes persistence component to reset the scene when it awakes.
			GameController.SetShouldBeReset (ThisScene, true);
			GameController.ClearPersistedDataForScene (ThisScene);
			// Reload the scene.
			SceneManager.LoadScene (ThisScene.GetFileName());
		}
	}
}
