using UnityEngine;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour
{
    public PressurePlateController parent;
    public float distToSink;

    private List<GameObject> localColliders;
    private float upperY;
    private float lowerY;
    private bool isInAnimation;
    private Vector2 movementDirection;

    // Use this for initialization
    void Start()
    {
        localColliders = new List<GameObject>();
        upperY = getYPos();
        lowerY = upperY - distToSink;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(name + " y:" + transform.position.y + " colliders:" + localColliders.Count);
        if (!isInAnimation) return;
        transform.Translate(movementDirection * distToSink);
        localColliders.ForEach(gameObject => gameObject.transform.Translate(movementDirection * distToSink));

        isInAnimation = getYPos() < upperY && getYPos() > lowerY;
    }

    private float getYPos()
    {
        return transform.position.y;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player") || col.gameObject.name.Equals("Box"))
        {
            localColliders.Add(col.gameObject);
            parent.colliders.Add(col.gameObject);
        }
        isInAnimation = getYPos() > lowerY;
        if (isInAnimation) movementDirection = Vector2.down;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player") || col.gameObject.name.Equals("Box"))
        {
            localColliders.Remove(col.gameObject);
            parent.colliders.Remove(col.gameObject);
        }
        isInAnimation = localColliders.Count == 0 && getYPos() < upperY;
        if (isInAnimation) movementDirection = Vector2.up;
    }
}
