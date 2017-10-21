using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterPage
{

    private Image NPCImage;
    private string NPCname;
    public List<Button> statementBtns;
    private List<string> statementStrings;

    // Use this for initialization
    void Start()
    {
        

    }


    // Update is called once per frame
    void Update()
    {

    }


    public CharacterPage(string NPCname, Image npcImage) {
        this.NPCname = NPCname;

        this.NPCImage = npcImage;
        statementBtns = new List<Button>();
        statementStrings = new List<string>();
    }

    public void addStatement(string statement)
    {
        if (!statementStrings.Contains(statement))
        {
            statementStrings.Add(statement);
        }
    }

    public void addStatementBtn(Button btn) {
        if (!statementBtns.Contains(btn))
        {
            statementBtns.Add(btn);
        }
    }

    public bool statementExists(string statement) {
        if (statementStrings.Contains(statement)) {
            return true;
        }
        return false;
    }
    public void setNPCnameAndImage(string NPCname, Image npcImage)
    {
        
    }

    public List<Button> getStatementButtons() {
        return statementBtns;
    }
}
