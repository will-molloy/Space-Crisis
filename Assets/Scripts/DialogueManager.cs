using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject dBox;
    public Text sText;
    public Text dText;

    public bool diaglogActive;

    public string[] dialogLines;
    public int currentLine;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (diaglogActive && Input.GetKeyDown(KeyCode.Space)) {
            //dBox.SetActive(false);
            //diaglogActive = false;

            currentLine++;

        }

        if (currentLine >= dialogLines.Length) {
            dBox.SetActive(false);
            diaglogActive = false;
            currentLine = 0;
        }
        dText.text = dialogLines[currentLine];
    }

    public void showBox(string source, string dialogue) {
        diaglogActive = true;
        dBox.SetActive(true);
        sText.text = source;
        dText.text = dialogue;
    }

    public void showDialogue(string source)
    {
        diaglogActive = true;
        dBox.SetActive(true);
        sText.text = source;
    }

}
