using UnityEngine;
using System.Collections;
using UnityEditor;

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

		leverFX = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Audio/CasualGameSounds/DM-CGS-39.wav", typeof(AudioClip));
		if(leverFX == null){
			Debug.LogError ("No lever audio");
		}

		pickUpFX = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Audio/CasualGameSounds/DM-CGS-22.wav", typeof(AudioClip));

		jumpClips[0] = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Audio/Player/Jumps/Jump1.wav", typeof(AudioClip));
		jumpClips[1] = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Audio/Player/Jumps/Jump2.wav", typeof(AudioClip));

		damagedAudioClips = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Audio/Player/Ouch/Damaged.wav", typeof(AudioClip));

		pressureFX[0] = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Audio/CasualGameSounds/DM-CGS-26.wav", typeof(AudioClip));
		pressureFX[1] = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Audio/CasualGameSounds/DM-CGS-27.wav", typeof(AudioClip));

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
			script.pickUpFX = pickUpFX;
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
