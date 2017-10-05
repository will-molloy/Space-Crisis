using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour {

	public int startHearts = 3;
	public int currentHearts = 3;
	public int maxHeart = 3;
	public int healthPerHeart = 1;

	public Image[] imgs;
	public Sprite[] healthSprites;


	void Start () {
		CheckHealthAmount();
		UpdateHearts ();
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

	void UpdateHearts(){
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

	public void TakeDamage(int amount){
		currentHearts += amount;
		currentHearts = Mathf.Clamp (currentHearts, 0, startHearts * healthPerHeart);
		UpdateHearts ();

	}
}
