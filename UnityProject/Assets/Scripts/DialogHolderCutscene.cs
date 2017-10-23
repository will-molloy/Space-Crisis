using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogHolderCutscene : MonoBehaviour
{
	public List<GameObject> dBoxes;
	public int boxIndex = 0;

	public string dialogue;
	private DialogueManagerCutscene dMan;

	public string[] textLines;
	// public Item item to check
	public bool autoDialog;
	private bool showDecisionBox;
	private float frame = 4.0f;
	private GameObject camera;
	public AudioClip audio;
	private AudioSource backgroundFX;
	private AudioSource audiosource;
	private bool endOfScene;

	//needs to link to inventory 

	// Use this for initialization
	void Start()
	{
		dMan = FindObjectOfType<DialogueManagerCutscene>();

		textLines = dBoxes[boxIndex].GetComponent<TextHolder>().getTextLines();
		endOfScene = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (frame <= 0) {
			if (autoDialog) {

				camera = GameObject.FindGameObjectWithTag ("MainCamera");
				backgroundFX = camera.GetComponent<AudioSource> ();
				backgroundFX.volume = 0.3F;

				//dMan.showBox(this.gameObject.name , dialogue);
				// show dialogue
				if (!dMan.diaglogActive) {
					setAndShowDialogue (dBoxes [boxIndex]);
					//dMan.currentLine = 0;
				}
				autoDialog = false;
			}
		} else if(frame > 0){
			frame -= Time.deltaTime;
		}
		
		if (dMan != null && this != null && dMan.activeNPC == this.gameObject) // only update dialogue if the current active NPC holds this dialogue
		{

			// check if should move on to next dialogue box
			if (dMan.currentLine >= textLines.Length)
			{    // end of this box's file asset
				if (boxIndex < dBoxes.Count - 1) {
					boxIndex++;

					setAndShowDialogue (dBoxes [boxIndex]);
				} else {

					if(endOfScene == false){


						sceneEndEvent ();

						endOfScene = true;
					}
						
				}

			}
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{

	}


	void OnTriggerEnter2D(Collider2D other)
	{

	}

	public void setAndShowDialogue(GameObject box)
	{
		if (dMan.diaglogActive)
		{
			dMan.closeDialogue();
		}

		textLines = box.GetComponent<TextHolder>().getTextLines();

		dMan.dBox = box;

		dMan.dialogLines = textLines;

		dMan.currentLine = 0;

		dMan.showDialogue(this.gameObject.name);

	}

	public void sceneEndEvent(){
		backgroundFX.volume = 0.0F;

		foreach (Transform child in camera.transform) if (child.CompareTag("Finish")) {
				child.gameObject.SetActive (true);	
			}
				

		audiosource = (AudioSource)GetComponent<AudioSource>();
		audiosource.clip = audio;
		audiosource.Play();
		StartCoroutine(WaitForSound(audio));

	}

	public IEnumerator WaitForSound(AudioClip Sound)
	{
		yield return new WaitUntil(() => audiosource.isPlaying == false);

		SceneManager.LoadScene("level1room1");
	}


}
