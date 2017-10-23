using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public GameObject dBox;

    private Text sText;
    public Text dText;
    private bool isFrozen = false;

    public bool diaglogActive;

    public string[] dialogLines;
    public int currentLine;

    private Rigidbody2D[] playerBody;
    private Vector2[] linearBackups;

    public GameObject activeNPC;
    public GameObject content;
    private CharacterContent characterContent;

    public bool allDone = false;

    // Use this for initialization
    void Start()
    {
        characterContent = content.GetComponent<CharacterContent>();

        dText.enabled = false;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        playerBody = new Rigidbody2D[2];
        linearBackups = new Vector2[2];

        playerBody[0] = players[0].GetComponent<Rigidbody2D>();
        playerBody[1] = players[1].GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space ) && activeNPC != null ) {
           
            if (!diaglogActive)
            {
                DialogHolder dh = activeNPC.GetComponent<DialogHolder>();
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
                characterContent.addStatement(activeNPC, dText.text);
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

    private void freezePlayer()
    {
        linearBackups[0] = playerBody[0].velocity;

        linearBackups[1] = playerBody[1].velocity;

        playerBody[0].velocity = Vector2.zero;
        playerBody[1].velocity = Vector2.zero;

        playerBody[0].constraints = RigidbodyConstraints2D.FreezeAll;
        playerBody[1].constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void unfreezePlayer()
    {
        playerBody[0].constraints = RigidbodyConstraints2D.None;
        playerBody[0].velocity = linearBackups[0];

        playerBody[1].constraints = RigidbodyConstraints2D.None;
        playerBody[1].velocity = linearBackups[1];
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
        DialogHolder dh = activeNPC.GetComponent<DialogHolder>();

        if (!dh.dBoxes.Contains(dBox))
        {
            dBox = dh.dBoxes[dh.dBoxes.Count - 1];
            dialogLines = dh.dBoxes[dh.dBoxes.Count - 1].GetComponent<TextHolder>().getTextLines();
            currentLine = 0;
        }
    }

    public void setAllDone() {
        allDone = true;
    }

    public bool getAllDone() {
        return allDone;
    }
}
