using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour {

	public int startHearts = 3;
	public static int currentHearts = 3;
	public int maxHeart = 3;
	public int healthPerHeart = 1;
	GameObject[] players;
	GameObject[] spawnPoints;
	bool isDead = false;

	public Image[] imgs;
	public Sprite[] healthSprites;

	Animator anim;  

	void Start () {
		CheckHealthAmount();
		UpdateHearts ();
		players = GameObject.FindGameObjectsWithTag ("Player");
		spawnPoints = GameObject.FindGameObjectsWithTag ("Position");
	}

	void Update(){

	}

	void CheckHealthAmount () {
		for (int i = 0; i < maxHeart; i++) {
			if (startHearts <= i) {
				imgs [i].enabled = false;
			} else {
				imgs [i].enabled = true; 
			}
		}
	}

	public void UpdateHearts(){
		bool empty = false;
		int i = 0;

		foreach (Image img in imgs) {
			if (empty) {
				img.sprite = healthSprites [0];
			} else {
				i++;
				if (currentHearts>i) {
					img.sprite = healthSprites [1];
				} else {
					int currentHealth = (int)(healthPerHeart-(healthPerHeart*i-currentHearts));
					int healthPerImage = healthPerHeart / (healthSprites.Length - 1);
					int imageIndex = currentHealth / healthPerImage;
					img.sprite = healthSprites [imageIndex];
					empty = true;
				}
			}
		}
	}

	public void TakeDamage(){
		currentHearts -=1;
		currentHearts = Mathf.Clamp (currentHearts, 0, startHearts * healthPerHeart);
		UpdateHearts ();

		if (currentHearts == 0) {
			//Game Over Screen to be Implemented...
		} else {
			for (int i = 0; i < 2; i++) {
				players[i].transform.position = spawnPoints[i].transform.position;
			}
		}
	}
}
