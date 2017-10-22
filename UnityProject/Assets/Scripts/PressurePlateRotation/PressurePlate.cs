using UnityEngine;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour
{
    public PressurePlateController parent;
    public float distToSink = 0.2f;
    public int animationTimeInFrames = 10;
	public AudioClip[] pressuredAudioClips;

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
        localColliders.ForEach(gameObject => {
            gameObject.transform.Translate(movementDirection * translation); // make objects go down with pad
            gameObject.transform.Rotate(new Vector2(0, 0)); // make objects flat on pad 
            });

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
		if (isInAnimation) {
			movementDirection = Vector2.down;
			AudioSource.PlayClipAtPoint(pressuredAudioClips[0], transform.position);
		}

    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (IsValidCollider(col))
        {
            localColliders.Remove(col.gameObject);
            parent.colliders.Remove(col.gameObject);
        }
        isInAnimation = localColliders.Count == 0 && GetYPos() < upperY;
		if (isInAnimation) {
			movementDirection = Vector2.up;
			AudioSource.PlayClipAtPoint(pressuredAudioClips[1], transform.position);
		}

    }

    private bool IsValidCollider(Collision2D col)
    {
        return col.gameObject.tag.Equals("Player") || col.gameObject.tag.Equals("Box");
    }
}
