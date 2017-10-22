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
        if (diaglogActive && Input.GetKeyDown(KeyCode.Space))
        {
            //dBox.SetActive(false);
            //diaglogActive = false;

            currentLine++;

        }
        if (dialogLines.Length > 0 && currentLine < dialogLines.Length)
        {
            dText.text = dialogLines[currentLine];

            if (diaglogActive && dBox.tag.Equals("NPCStatement"))
            {
                characterContent.addStatement(activeNPC, dText.text);
            }
        }

        if (currentLine >= dialogLines.Length)
        {
            closeDialogue();
            //currentLine = 0;
        }


    }

    public void showBox(string source, string dialogue)
    {


        diaglogActive = true;
        dBox.SetActive(true);
        //  sText.text = source;
        dText.text = dialogue;
        dText.enabled = true;

    }

    public void showDialogue(string source)
    {
        PlayerUtility.FreezePlayers();
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
        dText.enabled = false;
        diaglogActive = false;
        dBox.SetActive(false);
        PlayerUtility.UnFreezePlayers();
        if (isFrozen)
        {
            isFrozen = false;
           // unfreezePlayer();
            PlayerUtility.UnFreezePlayers();
        }

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


}
