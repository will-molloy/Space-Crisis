using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class LeverEventTest
{
    private GameObject leverObj;
    private GameObject plateObj;
    private GameController.PlayableScene testScene = GameController.PlayableScene.Level1Room1;

    [SetUp]
    public void Init()
    {
        leverObj = new GameObject();
        plateObj = new GameObject();
        Lever lever = leverObj.AddComponent<Lever>();
        PlateScript plate = plateObj.AddComponent<PlateScript>();

        leverObj.GetComponent<Lever>().thingsToControl = new List<GameObject>
        {
            plateObj,
        };

        // setup
        plate.translationAmount = 10;
        lever.timeInFrames = 1;
        plate.translationDirection = Vector3.down;
        plateObj.transform.position = new Vector3(0, 0, 0);
    }

    private void ActivateLever()
    {
        leverObj.GetComponent<Lever>().activate();
        plateObj.GetComponent<PlateScript>().Update();
        leverObj.GetComponent<Lever>().Update();
    }

    /// <summary>
    /// Ensure lever moves plate 
    /// </summary>
    [Test]
    public void TestLeverActivateOnce()
    {
        ActivateLever();

        // ensure plate moved down 10
        Assert.AreEqual(new Vector3(0, -10, 0), plateObj.transform.position);
    }

    /// <summary>
    /// Ensure lever moves plate back to original position if activated twice
    /// </summary>
    [Test]
    public void TestLeverActivateTwice()
    {
        ActivateLever();
        ActivateLever();

        // ensure plate moved back to original position
        Assert.AreEqual(new Vector3(0, 0, 0), plateObj.transform.position);
    }

    /// <summary>
    /// Ensure lever and plate state is preserved so it moves the plate back to original position
    /// when activated again when reentering the scene.
    /// </summary>
    [Test]
    public void TestLeverActivateTwiceAfterSceneChange()
    {
        ActivateLever();

        // Change scene and persist lever components

        // instantiate persistence
        leverObj.name = "lever";
        GameObject persistenceObj = new GameObject();
        ScenePersistence persistence = persistenceObj.AddComponent<ScenePersistence>();
        persistence.thisScene = testScene;

        // add objects to persist 
        plateObj.transform.parent = persistenceObj.transform;
        leverObj.transform.parent = persistenceObj.transform;

        // attempt to save obj positions
        persistence.FixedUpdate();
        persistence.Start();

        // ensure plate is still in moved position
        Vector3 persistedPlate = GameController.GetSavedObjectPositons(testScene)[plateObj.name];
        Assert.AreEqual(new Vector3(0, -10, 0), persistedPlate);

        ActivateLever();

        // ensure plate moved back to original position
        Assert.AreEqual(new Vector3(0, 0, 0), plateObj.transform.position);
    }
}
