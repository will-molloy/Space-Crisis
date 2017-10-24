using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class AudioManager{

	private static AudioClip leverFX;
	private static AudioClip pickUpFX;
	private static AudioClip damagedAudioClips;
	private static AudioClip[] jumpClips;
	private static AudioClip[] pressureFX;

	static AudioManager()
	{
		jumpClips = new AudioClip[2];
		pressureFX = new AudioClip[2];
		leverFX = Resources.Load<AudioClip>("DM-CGS-39");

		if(leverFX == null){
			Debug.LogError ("No lever audio");
		}

		pickUpFX = Resources.Load<AudioClip>("DM-CGS-22");

		jumpClips[0] = Resources.Load<AudioClip>("Jump1");
		jumpClips[1] = Resources.Load<AudioClip>("Jump2");

		damagedAudioClips = Resources.Load<AudioClip>("Damaged");

		pressureFX[0] = Resources.Load<AudioClip>("DM-CGS-26");
		pressureFX[1] = Resources.Load<AudioClip>("DM-CGS-27");
	}

	public static void loadAudio()
	{
		foreach(GameObject Obj in GameObject.FindGameObjectsWithTag("Player"))
		{
			if(Obj.name == "Astronaut")
			{
				PlatformerCharacter2D player = Obj.GetComponent<PlatformerCharacter2D> ();
				player.jumpAudio = jumpClips[0];


			} else if(Obj.name == "Astronaut_2") {
				PlatformerCharacter2D player = Obj.GetComponent<PlatformerCharacter2D> ();
				player.jumpAudio = jumpClips[1];
			}

		}

		GameObject team = GameObject.Find("Team");
		LifeSystem health = team.GetComponent<LifeSystem> ();
		health.damagedAudioClips = damagedAudioClips;
			
		GameObject manager = GameObject.Find("ItemSpawnManager");

		if(manager != null){
			ItemSpawnManager script = manager.GetComponent<ItemSpawnManager> ();
			script.ItemPickUpSound = pickUpFX;
		}


		foreach(GameObject Obj in GameObject.FindGameObjectsWithTag("Lever"))
		{
			Lever lever = Obj.GetComponent<Lever> ();
			lever.pulledFX = leverFX;
		}

		foreach(GameObject Obj in GameObject.FindGameObjectsWithTag("PressurePad"))
		{
			PressurePlate plate = Obj.GetComponent<PressurePlate> ();
			plate.pressuredAudioClips = pressureFX;
		}
			
	}
}
