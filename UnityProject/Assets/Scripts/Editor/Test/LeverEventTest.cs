using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class LeverEventTest
{
    private GameObject leverObj = new GameObject();
    private GameObject plateObj = new GameObject();
    private Lever lever;
    private PlateScript plate;

    [SetUp]
    public void Init()
    {
        lever = leverObj.AddComponent<Lever>();
        leverObj.AddComponent<Transform>();
        plate = plateObj.AddComponent<PlateScript>();
        plateObj.AddComponent<Transform>();

        leverObj.GetComponent<Lever>().thingsToControl = new List<GameObject>
        {
            plateObj,
        };
    }

    [Test]
    public void TestLeverSingleActivate()
    {
        // setup
        plate.translationAmount = 10;
        lever.timeInFrames = 1;
        plate.translationDirection = Vector3.down;
        plateObj.transform.position = new Vector3(0, 0, 0);
        
        // activate lever
        leverObj.GetComponent<Lever>().activate();
        plateObj.GetComponent<PlateScript>().Update();

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

        // activate lever
        leverObj.GetComponent<Lever>().activate();
        plateObj.GetComponent<PlateScript>().Update();

        Assert.AreEqual(new Vector3(0, -10, 0), plateObj.transform.position);

        leverObj.GetComponent<Lever>().activate();
        plateObj.GetComponent<PlateScript>().Update();

        // ensure plate moved back to original position
        Assert.AreEqual(new Vector3(0, 0, 0), plateObj.transform.position);
    }
}
