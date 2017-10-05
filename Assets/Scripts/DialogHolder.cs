using UnityEngine;
using System.Collections;

public class DialogHolder : MonoBehaviour {

    public string dialogue;
    private DialogueManager dMan;
    public string[] dialogLines;

    // Use this for initialization
    void Start () {
        dMan = FindObjectOfType<DialogueManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (Input.GetKeyUp(KeyCode.Space)) {

                //dMan.showBox(this.gameObject.name , dialogue);

                if (!dMan.diaglogActive) {
                    dMan.dialogLines = dialogLines;
                    dMan.currentLine = 0;
                    dMan.showDialogue(this.gameObject.name);
                }

            }
        }
    }

}
