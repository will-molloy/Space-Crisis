using UnityEngine;
using System.Collections;

public class DialogHolder : MonoBehaviour
{

    public string dialogue;
    private DialogueManager dMan;

    // public Item item to check
    //needs to link to inventory 

    public DialogDetail triggeredDialog;
    public DialogDetail norminalDialog;


    // Use this for initialization
    void Start()
    {
        dMan = FindObjectOfType<DialogueManager>();
        triggeredDialog = new DialogDetail();
        triggeredDialog.source = "AAAAAAAAAAAAAAAAAAA";
        triggeredDialog.lines = new string[] { "A", " B", "c" };
        norminalDialog = new DialogDetail();
        norminalDialog.lines = new string[] { "z", " x", "y" };
        norminalDialog.source = "BBBBBBBBBBB";
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                dMan.startDialogueOf(norminalDialog);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && norminalDialog != null)
        {
                dMan.startDialogueOf(triggeredDialog);
        }
    }

}
