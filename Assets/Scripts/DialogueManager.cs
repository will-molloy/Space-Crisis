using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public GameObject dialogBox;

    public Text sText;
    public Text dText;
    public bool isFrozen = false;

    public bool diaglogActive;

    public string[] dialogLines;
    public int currentLine;

    private Rigidbody2D[] playerBody;
    private Vector2[] linearBackups;
    private RigidbodyConstraints2D[] constraintsBackup = new RigidbodyConstraints2D[2];
    private DialogDetail theDialog;
    private bool isInDialog = false;
    public const int FRAME_DELAY = 30;
    private int delayRemaining;
    private bool delaying = false;



    // Use this for initialization
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        playerBody = new Rigidbody2D[2];
        linearBackups = new Vector2[2];

        playerBody[0] = players[0].GetComponent<Rigidbody2D>();
        playerBody[1] = players[1].GetComponent<Rigidbody2D>();
        SetDialogBoxState(false);
    }

    private void TryAdvanceDialog()
    {
        if (isInDialog && Input.GetKeyDown(KeyCode.Space) && theDialog != null)
        {
            // Try to advance the dialog now
            string nextLine = theDialog.GetNextLine();
            if (nextLine != null) dText.text = nextLine;
            else /* we have no more lines, close */
                closeDialogue();
        }
    }

    private void SetDelayedClose()
    {
        /* Wont accept any command while delaying is SET */
        delaying = true;
        delayRemaining = FRAME_DELAY;
    }

    private void CheckDelayedClose()
    {
        if (delaying)
        {
            delayRemaining--;
            if(delayRemaining < 1)
            {
                delaying = false;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        TryAdvanceDialog();
        CheckDelayedClose();
    }

    public void showBox(string source, string dialogue)
    {
        diaglogActive = true;
        dialogBox.SetActive(true);
        sText.text = source;
        dText.text = dialogue;
    }

    public void closeDialogue()
    {
        SetDelayedClose();
        SetDialogBoxState(false);
        isInDialog = false;

        if (isFrozen)
        {
            isFrozen = false;
            unfreezePlayer();
        }

    }

    public void startDialogueOf(DialogDetail theDialog)
    {
        Debug.Log("START THE DIALOG OF " + theDialog.source);
        if (!isInDialog && theDialog.PeekNextLine() != null && !delaying)
        {
            this.theDialog = theDialog;
            isInDialog = true;
            sText.text = theDialog.source;
            dText.text = theDialog.GetNextLine();
            freezePlayer();
            SetDialogBoxState(true);
        }
    }

    private void SetDialogBoxState(bool state)
    {
        diaglogActive = state;
        dialogBox.SetActive(state);
    }

    private void freezePlayer()
    {
        isFrozen = true;
        linearBackups[0] = playerBody[0].velocity;
        linearBackups[1] = playerBody[1].velocity;
        
        playerBody[0].velocity = Vector2.zero;
        playerBody[1].velocity = Vector2.zero;

        constraintsBackup[0] = playerBody[0].constraints;
        constraintsBackup[1] = playerBody[1].constraints;

        playerBody[0].constraints = RigidbodyConstraints2D.FreezeAll;
        playerBody[1].constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void unfreezePlayer()
    {
        isFrozen = false;
        playerBody[0].constraints = constraintsBackup[0];
        playerBody[0].velocity = linearBackups[0];

        playerBody[1].constraints = constraintsBackup[1];
        playerBody[1].velocity = linearBackups[1];
    }

}
