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

        plate.GetComponent<PlateScript>().translationAmount = 10;
        plate.GetComponent<PlateScript>().translationDirection = Vector3.down;
        plate.transform.position = new Vector3(0, 0, 0);

        lever.GetComponent<Lever>().timeInFrames = 1;
        lever.GetComponent<Lever>().thingsToControl = new List<GameObject>
        {
            plate,
        };
    }

    [Test]
    public void TestLeverSingleMovement()
    {
        Assert.AreEqual(new Vector3(0,0,0), plate.transform.position);
        lever.GetComponent<Lever>().activate();
        Assert.AreEqual(new Vector3(0,10,0), plate.transform.position);
    }
}
