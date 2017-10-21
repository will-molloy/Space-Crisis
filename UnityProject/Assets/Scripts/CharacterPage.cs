using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterPage
{

    private static Image _NPCImage;
    private static string _NPCname;
    public static List<Button> statementBtns;
    private static List<string> statementStrings;

    // Use this for initialization
    void Start()
    {


    }


    // Update is called once per frame
    void Update()
    {

    }


    public CharacterPage(string NPCname, Image npcImage)
    {
        _NPCname = NPCname;
        _NPCImage = npcImage;
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

    public void addStatementBtn(Button btn)
    {
        if (!statementBtns.Contains(btn))
        {
            statementBtns.Add(btn);
        }
    }

    public bool statementExists(string statement)
    {
        if (statementStrings.Contains(statement))
        {
            return true;
        }
        return false;
    }
    public void setNPCnameAndImage(string NPCname, Image npcImage)
    {

    }

    public List<string> getStatementStrings()
    {
        return statementStrings;
    }

    public string getNpcName()
    {
        return _NPCname;
    }

    public Image getNpcImage()
    {
        return _NPCImage;
    }
}
