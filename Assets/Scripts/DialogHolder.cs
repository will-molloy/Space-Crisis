using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DialogHolder : MonoBehaviour
{
    //public GameObject[] dBoxes;
    public List<GameObject> dBoxes;
    private int boxIndex = 0;
    // public TextAsset[] textAssets;

    public string dialogue;
    private DialogueManager dMan;

    private TextAsset textFile;
    public string[] textLines;
    public int lineToBreak;
    // public Item item to check
    public bool autoDialog;
    private bool moveOn;
    //needs to link to inventory 

    // Use this for initialization
    void Start()
    {
        textFile = dBoxes[boxIndex].GetComponent<TextHolder>().textFile;

        moveOn = false;
        dMan = FindObjectOfType<DialogueManager>();

        if (textFile != null)
        {
            textLines = textFile.text.Split('\n');
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (dMan.currentLine == lineToBreak && lineToBreak != 0)
        {
            moveOn = true;
            //!!! needs to check if user has the item in their inventory!!!!!
            dMan.closeDialogue();
        }

        // check if should move on to next dialogue box
        if (dMan.currentLine >= textLines.Length)
        {    // end of this box's file asset
            if (boxIndex < dBoxes.Count - 1)
            {
                boxIndex++;
                textFile = dBoxes[boxIndex].GetComponent<TextHolder>().textFile;
                if (textFile != null)
                {
                    textLines = textFile.text.Split('\n');
                }

                setDialogue();
            }
        }
        else { 
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        // dMan.setActiveNPC(this.gameObject);
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {

                //dMan.showBox(this.gameObject.name , dialogue);
                // show dialogue
                if (!dMan.diaglogActive && !moveOn)
                {
                    setDialogue();
                }

                if (!dMan.diaglogActive && moveOn)
                {
                    dMan.currentLine = lineToBreak + 1; // line5 corresponds to the text file content
                    dMan.showDialogue(this.gameObject.name);
                }

            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (autoDialog)
            {

                //dMan.showBox(this.gameObject.name , dialogue);
                // show dialogue
                if (!dMan.diaglogActive && !moveOn)
                {
                    setDialogue();
                }

            }
        }
    }

    public void addNextBox(GameObject nextBox) {
        dBoxes.Add(nextBox);
        boxIndex = dBoxes.Count - 1; // use latest added box
        setDialogue();
    }

    private void setDialogue()
    {


        textFile = dBoxes[boxIndex].GetComponent<TextHolder>().textFile;
        if (textFile != null)
        {
            textLines = textFile.text.Split('\n');
        }

        dMan.dBox = dBoxes[boxIndex];

        dMan.dialogLines = textLines;
        dMan.currentLine = 0;
       
            dMan.showDialogue(this.gameObject.name);
        
       
    }

}
