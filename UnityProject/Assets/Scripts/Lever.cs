using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using System.Collections.Generic;

public class Lever : MonoBehaviour
{
    protected int remainingFrames = int.MaxValue;
    protected bool isRunning = false;
	public AudioClip pulledFX;
    public int timeInFrames;
    public List<GameObject> thingsToControl = new List<GameObject>();

    private static Vector3 InitialScale, FinalScale;
    private Dictionary<string, Vector3> persistedPlatePositions;

    public void Start()
    {
        persistedPlatePositions = GameController.GetPlatePositons();

        if (InitialScale == null)
        {
            InitialScale = transform.localScale;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            FinalScale = theScale;
        }

        foreach(GameObject obj in thingsToControl)
        {
            PlateScript ps = obj.GetComponent<PlateScript>();
            Vector3 currentObjPos = obj.transform.position;
            if (!persistedPlatePositions.ContainsKey(obj.name)) // Write once
            {
                GameController.AddPlatePosition(obj.name, currentObjPos);
                persistedPlatePositions = GameController.GetPlatePositons();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Platformer2DUserControl p1 = other.gameObject.GetComponent<Platformer2DUserControl>();
            if (p1 != null)
            {
                p1.lever = this;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Platformer2DUserControl p1 = other.gameObject.GetComponent<Platformer2DUserControl>();
            if (p1 != null)
            {
                p1.lever = null;
            }
        }
    }

    // Update is called once per frame
    virtual public void Update()
    {
        //if (persistedPlatePositions.Count > 0 && persistedPlatePositions[thingsToControl[0].name] == thingsToControl[0].transform.position)
        //{
        //    // Initial position
        //    transform.localScale = InitialScale; 
        //} else
        //{
        //    transform.localScale = FinalScale;
        //}

        if (isRunning) remainingFrames--;
        if (isRunning && remainingFrames < 1)
        {
            foreach (GameObject obj in thingsToControl)
            {
                PlateScript ps = obj.GetComponent<PlateScript>();
                ps.stop();
            }
            isRunning = false;
        }

    }

    public virtual void activate()
    {
        if (isRunning) return;
        isRunning = true;

        Flip();
        remainingFrames = timeInFrames;

		if (pulledFX != null){
			AudioSource.PlayClipAtPoint(pulledFX, transform.position);
		}

        foreach (GameObject obj in thingsToControl)
        {
            PlateScript ps = obj.GetComponent<PlateScript>();
            Vector3 currentObjPos = obj.transform.position;
            if (currentObjPos != persistedPlatePositions[obj.name])
            {
                Debug.Log("Reversing");
                ps.reverseDirection();
            }
            ps.setAnimationTime(this.timeInFrames);
            ps.start();
        }

    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	public void assignSoundFX(AudioClip clip){
		pulledFX = clip;
	}
}
