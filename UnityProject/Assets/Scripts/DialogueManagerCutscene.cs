using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManagerCutscene : MonoBehaviour
{

	public GameObject dBox;

	private Text sText;
	public Text dText;
	private bool isFrozen = false;

	public bool diaglogActive;

	public string[] dialogLines;
	public int currentLine;

	private Vector2[] linearBackups;

	public GameObject activeNPC;
	public GameObject content;
	// Use this for initialization
	void Start()
	{

		dText.enabled = false;

		linearBackups = new Vector2[2];

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space ) && activeNPC != null) {

			if (!diaglogActive)
			{
				DialogHolderCutscene dh = activeNPC.GetComponent<DialogHolderCutscene>();
				dBox = dh.dBoxes[dh.boxIndex];
				dialogLines = dBox.GetComponent<TextHolder>().getTextLines();
				currentLine = 0;
				showDialogue(this.gameObject.name);
			}
			else {
				currentLine++;
			}
		}

		if (dialogLines.Length > 0 && currentLine < dialogLines.Length)
		{
			dText.text = dialogLines[currentLine];

			if (diaglogActive && dBox.tag.Equals("NPCStatement"))
			{
				if(activeNPC != null){
				}

			}
		}

		if ((currentLine >= dialogLines.Length) && diaglogActive)
		{
			closeDialogue();
			//currentLine = 0;
		}


	}

	public void showDialogue(string source)
	{
		Debug.Log("Show dia");
		if (!isFrozen)
		{
			isFrozen = true;
			//freezePlayer();
			PlayerUtility.FreezePlayers();
		}
		dText.enabled = true;
		diaglogActive = true;
		dBox.SetActive(true);

		// enable button if any
		Button[] btns = dBox.GetComponents<Button>();
		for (int i = 0; i < btns.Length; i++)
		{
			btns[i].enabled = true;
			btns[i].interactable = true;
		}

		//  sText.text = source;

	}

	public void closeDialogue()
	{
		Debug.Log("close dia");
		dText.enabled = false;
		diaglogActive = false;
		dBox.SetActive(false);
		//PlayerUtility.UnFreezePlayers();
		if (isFrozen)
		{
			isFrozen = false;
			// unfreezePlayer();
			PlayerUtility.UnFreezePlayers();
		}
		// cleanUpDialogue();
	}

	public void setActiveNPC(GameObject NPC)
	{
		activeNPC = NPC;
		currentLine = 0;
	}

	public GameObject getActiveNPC() {
		return activeNPC;
	}

	private void resetCurrentLine() {
		DialogHolderCutscene dh = activeNPC.GetComponent<DialogHolderCutscene>();

		if (!dh.dBoxes.Contains(dBox))
		{
			dBox = dh.dBoxes[dh.dBoxes.Count - 1];
			dialogLines = dh.dBoxes[dh.dBoxes.Count - 1].GetComponent<TextHolder>().getTextLines();
			currentLine = 0;
		}
	}

}
