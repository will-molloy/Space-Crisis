using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class LeverEventTest
{
    private GameObject leverObj = new GameObject();
    private GameObject plateObj = new GameObject();
    // Only for init, components change in lever script
    private Lever lever;
    private PlateScript plate;

    [SetUp]
    public void Init()
    {
        lever = leverObj.AddComponent<Lever>();
        plate = plateObj.AddComponent<PlateScript>();

        leverObj.GetComponent<Lever>().thingsToControl = new List<GameObject>
        {
            plateObj,
        };
    }

    private void ActivateLever()
    {
        leverObj.GetComponent<Lever>().activate();
        plateObj.GetComponent<PlateScript>().Update();
        leverObj.GetComponent<Lever>().Update();
    }

    [Test]
    public void TestLeverSingleActivate()
    {
        // setup
        plate.translationAmount = 10;
        lever.timeInFrames = 1;
        plate.translationDirection = Vector3.down;
        plateObj.transform.position = new Vector3(0, 0, 0);

        ActivateLever();

        // ensure plate moved down 10
        Assert.AreEqual(new Vector3(0,-10,0), plateObj.transform.position);
    }

    [Test]
    public void TestLeverActivatedTwice()
    {
        // setup
        plate.translationAmount = 10;
        lever.timeInFrames = 1;
        plate.translationDirection = Vector3.down;
        plateObj.transform.position = new Vector3(0, 0, 0);

        ActivateLever();
        ActivateLever();

        // ensure plate moved back to original position
        Assert.AreEqual(new Vector3(0, 0, 0), plateObj.transform.position);
    }
}
