using UnityEngine;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour
{
    public PressurePlateController parent;
    public float distToSink = 0.2f;
    public int animationTimeInFrames = 10;

    private List<GameObject> localColliders;
    private float upperY;
    private float lowerY;
    private bool isInAnimation;
    private Vector2 movementDirection;
    private float translation;

    // Use this for initialization
    void Start()
    {
        localColliders = new List<GameObject>();
        upperY = GetYPos();
        lowerY = upperY - distToSink;
        translation = distToSink / animationTimeInFrames;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInAnimation) return;

        transform.Translate(movementDirection * translation);
        localColliders.ForEach(gameObject => gameObject.transform.Translate(movementDirection * translation));

        isInAnimation = GetYPos() < upperY && GetYPos() > lowerY;
    }

    private float GetYPos()
    {
        return transform.position.y;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (IsValidCollider(col))
        {
            localColliders.Add(col.gameObject);
            parent.colliders.Add(col.gameObject);
        }
        isInAnimation = GetYPos() > lowerY;
        if (isInAnimation) movementDirection = Vector2.down;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (IsValidCollider(col))
        {
            localColliders.Remove(col.gameObject);
            parent.colliders.Remove(col.gameObject);
        }
        isInAnimation = localColliders.Count == 0 && GetYPos() < upperY;
        if (isInAnimation) movementDirection = Vector2.up;
    }

    private bool IsValidCollider(Collision2D col)
    {
        return col.gameObject.tag.Equals("Player") || col.gameObject.tag.Equals("Box");
    }
}
