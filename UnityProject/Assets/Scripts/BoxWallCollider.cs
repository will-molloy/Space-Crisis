using UnityEngine;
using System.Collections;

public class BoxWallCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.tag.Equals("Box"))
        {
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<BoxCollider2D>(), GetComponent<EdgeCollider2D>());
        }
    }
}
