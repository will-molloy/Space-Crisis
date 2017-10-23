using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerTest
{
    private GameController.PlayableScene testScene = GameController.PlayableScene.Level1Room1;
    private ScenePersistence persistence;
    private GameObject persistedObj;

    [SetUp]
    public void Init()
    {
        // instantiate persistence
        persistence = new GameObject().AddComponent<ScenePersistence>();
        persistence.thisScene = testScene;

        // add some children objects to persist
        persistedObj = new GameObject();
        persistedObj.name = "persisted-object";
        persistedObj.transform.parent = persistence.transform;
        persistedObj.transform.position = new Vector3(0, 0, 0);
    }

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
            GameController.PlayableScene.Level2Room3,
        };

        Assert.AreEqual(expectedScenesLevel1, GameController.GetScenesForLevel(GameController.Level.Level1));
        Assert.AreEqual(expectedScenesLevel2, GameController.GetScenesForLevel(GameController.Level.Level2));
        Assert.AreEqual(expectedScenesNoLevel, GameController.GetScenesForLevel(GameController.Level.None));
    }

    /// <summary>
    /// Ensures the persistence saves the position of any child objects within both the
    /// saved and initial object position dictionaries in the gamecontroller.
    /// </summary>
    [Test]
    public void PersistenceSavedAndInitialTest()
    {
        // attempt to save obj positions
        persistence.FixedUpdate();

        // ensure gamecontroller has the objects position in both initial and saved dictionaries
        Assert.AreEqual(
            new Vector3(0, 0, 0),
            GameController.GetInitialObjectPositions(testScene)[persistedObj.name]
            );
        Assert.AreEqual(
            new Vector3(0, 0, 0),
            GameController.GetSavedObjectPositons(testScene)[persistedObj.name]
            );
    }

    /// <summary>
    /// Ensures the initial object position dictionary within the gamecontroller
    /// is not updated when an object moves but the saved object dictionary is.
    /// </summary>
    [Test]
    public void PersistenceMoveObjectTest()
    {
        // attempt to save obj positions
        persistence.FixedUpdate();

        // Move the object
        persistedObj.transform.position = new Vector3(1, 1, 1);

        // save positions again
        persistence.FixedUpdate();

        // ensure initial dictionary has only initial position
        Assert.AreEqual(
            new Vector3(0, 0, 0),
            GameController.GetInitialObjectPositions(testScene)[persistedObj.name]
            );

        // ensure saved dictionary has latest position
        Assert.AreEqual(
            new Vector3(1, 1, 1),
            GameController.GetSavedObjectPositons(testScene)[persistedObj.name]
            );
    }
}
