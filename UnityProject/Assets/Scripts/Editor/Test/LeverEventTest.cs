using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class LeverEventTest
{
    private GameObject lever = new GameObject();
    private GameObject plate = new GameObject();

    [SetUp]
    public void Init()
    {
        lever.AddComponent<Lever>();
        lever.AddComponent<Transform>();
        plate.AddComponent<PlateScript>();
        plate.AddComponent<Transform>();

        lever.GetComponent<Lever>().thingsToControl = new List<GameObject>
        {
            plate,
        };
    }

    [Test]
    public void TestLeverSingleMovement()
    {
        // setup
        plate.GetComponent<PlateScript>().translationAmount = 10;
        lever.GetComponent<Lever>().timeInFrames = 1;
        plate.GetComponent<PlateScript>().translationDirection = Vector3.down;
        plate.transform.position = new Vector3(0, 0, 0);
        
        // activate lever
        lever.GetComponent<Lever>().activate();
        plate.GetComponent<PlateScript>().Update();

        // ensure plate moved down 10
        Assert.AreEqual(new Vector3(0,-10,0), plate.transform.position);
    }
}
