using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * The code is referenced from https://www.youtube.com/watch?v=CBjpiNYDecQ.
 */

public class LifeSystem : MonoBehaviour {


	int startHearts = 7;
	public static int currentHearts = 7;

	GameObject[] players;
	GameObject[] spawnPoints;
	public AudioClip damagedAudioClips;	
	public bool isDead = false;


	public Image[] imgs;
	public Sprite[] healthSprites;

	Animator anim;  

	/*
	 * Initialising 
	 */
	void Start () {
		//CheckHealthAmount();
		//Check players health at the start of scene and load the hearts depending on the currentHearts amount.
		UpdateHearts ();

		//Initialise fields
		players = GameObject.FindGameObjectsWithTag ("Player");
		spawnPoints = GameObject.FindGameObjectsWithTag ("Position");
	}
		
	/*void CheckHealthAmount () {
		for (int i = 0; i < startHearts; i++) {
			if (startHearts <= i) {
				imgs [i].enabled = false;
			} else {
				imgs [i].enabled = true; 
			}
		}
	}*/

	/**
	 * Function to assign correct images to  
	 * correct lifebar image.
	 *
	 */
	public void UpdateHearts(){
		//checking whether there is life left for players
		bool empty = false;
		int i = 0;

		//ONLY when the player is alive
		if (!isDead) {
			//Selecting hearts inside the hearts system.
			foreach (Image img in imgs) {
				//If empty, change image to be empty heart
				if (empty) {
					img.sprite = healthSprites [0];
				} else {
					//Check the image cells starting from position 0.
					i++;
					//If the current health amount larger than the heart position, change the image with filled heart.
					if (currentHearts > i) {
						img.sprite = healthSprites [1];
					} else {
						int currentHealth = (int)(1 - (1 * i - currentHearts));
						int healthPerImage = 1 / (healthSprites.Length - 1);
						int imageIndex = currentHealth / healthPerImage;
						img.sprite = healthSprites [imageIndex];
						empty = true;
					}
				}
			}
		}
	}

	/**
	 * Decrease the current health amount when the function is called.
	 */
	public void TakeDamage(){
			currentHearts -= 1;
			currentHearts = Mathf.Clamp (currentHearts, 0, startHearts * 1);
			UpdateHearts ();

		if(damagedAudioClips != null){
			// Play a audio clip of the player getting hurt.
			AudioSource.PlayClipAtPoint(damagedAudioClips, transform.position);
		}

			//checks whether the players died or not.
			if (currentHearts == 0) {
				isDead = true;
			} else {
			//Send playsers to starting position if player looses a life.
				for (int i = 0; i < 2; i++) {
					players [i].transform.position = spawnPoints [i].transform.position;
				}
			}
	}

	/**
	 * Resets the persisted current hearts ammount
	 */
	public void ResetHearts(){
		currentHearts = 7;
	}
		
	/**
	 * Make players lose life without moving them back to the start position.
	 */
    public void TakeDamageWithoutTransform()
    {
        currentHearts -= 1;
        currentHearts = Mathf.Clamp(currentHearts, 0, startHearts * 1);
        UpdateHearts();

        if (damagedAudioClips != null)
        {
            // Play a audio clip of the player getting hurt.
            AudioSource.PlayClipAtPoint(damagedAudioClips, transform.position);
        }


        if (currentHearts == 0)
        {
            isDead = true;
        }
        // do not transform
    }
}
