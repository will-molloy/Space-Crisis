using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class LeverEventTest
{
    private GameObject leverObj = new GameObject();
    private GameObject plateObj = new GameObject();

    [SetUp]
    public void Init()
    {
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


        ActivateLever();

        // ensure plate moved back to original position
        Assert.AreEqual(new Vector3(0, 0, 0), plateObj.transform.position);
    }
}
