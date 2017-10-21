using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerTest
{

    /// <summary>
    /// Ensures scenes in level are retrieved with no extra or missing scenes and in correct order.
    /// </summary>
    [Test]
    public void SceneLevelRetrievalTest()
    {
        List<GameController.PlayableScene> expectedScenesNoLevel = new List<GameController.PlayableScene>
        {
            GameController.PlayableScene.WelcomeScreen,
            GameController.PlayableScene.ExitScene,
        };

        List<GameController.PlayableScene> expectedScenesLevel1 = new List<GameController.PlayableScene>
        {
            GameController.PlayableScene.Level1Room1,
            GameController.PlayableScene.Level1Room2,
            GameController.PlayableScene.Level1Room3,
        };

        List<GameController.PlayableScene> expectedScenesLevel2 = new List<GameController.PlayableScene>
        {
            GameController.PlayableScene.Level2Room1,
            GameController.PlayableScene.Level2Room2,
        };

        Assert.AreEqual(expectedScenesLevel1, GameController.getScenesForLevel(GameController.Level.Level1));
        Assert.AreEqual(expectedScenesLevel2, GameController.getScenesForLevel(GameController.Level.Level2));
        Assert.AreEqual(expectedScenesNoLevel, GameController.getScenesForLevel(GameController.Level.None));
    }

    /// <summary>
    /// Ensures the persistence saves the position of any child objects within both the
    /// saved and initial object position dictionaries in the gamecontroller.
    /// </summary>
    [Test]
    public void PersistenceSavedAndInitialTest()
    {
        // instantiate persistence
        var persistence = new GameObject();
        persistence.AddComponent<ScenePersistence>();
        persistence.AddComponent<Transform>();
        GameController.PlayableScene testScene = GameController.PlayableScene.Level1Room1;
        persistence.GetComponent<ScenePersistence>().thisScene = testScene;

        // add some children objects to persist
        var gameObject = new GameObject();
        gameObject.name = "persisted-object";
        gameObject.AddComponent<Transform>();
        gameObject.transform.parent = persistence.transform;
        gameObject.transform.position = new Vector3(0, 0, 0);

        // attempt to save obj positions
        persistence.GetComponent<ScenePersistence>().FixedUpdate();
        
        // ensure gamecontroller has the objects position in both initial and saved dictionaries
        Assert.AreEqual(
            new Vector3(0, 0, 0),
            GameController.GetInitialObjectPositions(testScene)[gameObject.name]
            );
        Assert.AreEqual(
            new Vector3(0,0,0),
            GameController.GetSavedObjectPositons(testScene)[gameObject.name]
            );
    }

    /// <summary>
    /// Ensures the initial object position dictionary within the gamecontroller
    /// is not updated when an object moves but the saved object dictionary is.
    /// </summary>
    [Test]
    public void PersistenceMoveObjectTest()
    {
        // instantiate persistence
        var persistence = new GameObject();
        persistence.AddComponent<ScenePersistence>();
        persistence.AddComponent<Transform>();
        GameController.PlayableScene testScene = GameController.PlayableScene.Level1Room1;
        persistence.GetComponent<ScenePersistence>().thisScene = testScene;

        // add some children objects to persist
        var gameObject = new GameObject();
        gameObject.name = "persisted-object";
        gameObject.AddComponent<Transform>();
        gameObject.transform.parent = persistence.transform;
        gameObject.transform.position = new Vector3(0, 0, 0);

        // attempt to save obj positions
        persistence.GetComponent<ScenePersistence>().FixedUpdate();

        // Move the object
        gameObject.transform.position = new Vector3(1, 1, 1);

        // save positions again
        persistence.GetComponent<ScenePersistence>().FixedUpdate();

        // ensure initial dictionary has only initial position
        Assert.AreEqual(
            new Vector3(0,0,0),
            GameController.GetInitialObjectPositions(testScene)[gameObject.name]
            );

        // ensure saved dictionary has latest position
        Assert.AreEqual(
            new Vector3(1, 1, 1),
            GameController.GetSavedObjectPositons(testScene)[gameObject.name]
            );
    }
}
