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
        leverObj.name = "lever";
        plateObj = new GameObject();
        plateObj.name = "plate";
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
        plateObj.transform.position = new Vector3(0, 0, 0);
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
        plateObj.transform.position = new Vector3(0, 0, 0);
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
        plateObj.transform.position = new Vector3(0, 0, 0);
        ActivateLever();

        // Change scene and persist lever components

        // instantiate persistence
        ScenePersistence persistence = new GameObject().AddComponent<ScenePersistence>();
        persistence.thisScene = testScene;

        // add plate to persist 
        plateObj.transform.parent = persistence.transform;

        // attempt to save obj positions
        persistence.FixedUpdate();

        // Reload the plate and lever
        Init();
        plateObj.transform.position = GameController.GetSavedObjectPositons(testScene)[plateObj.name];

        // ensure plate is still in moved position
        Assert.AreEqual(new Vector3(0, -10, 0), plateObj.transform.position);

        ActivateLever();

        // ensure plate moved back to original position
        Assert.AreEqual(new Vector3(0, 0, 0), plateObj.transform.position);
    }
}
