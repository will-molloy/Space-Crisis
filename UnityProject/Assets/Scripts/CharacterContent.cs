using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections.Specialized;

public class CharacterContent : MonoBehaviour
{
    public OrderedDictionary characterProfile = new OrderedDictionary();
    public GameObject contentPane;
    public int index;
    // Use this for initialization
    void Start()
    {
        index = 0;
        showPage(index);
    }

    // Update is called once per frame
    void Update()
    {
        showPage(index);
    }

    public void addStatement(GameObject NPC, string statement)
    {
        if (!characterProfile.Contains(NPC.name))
        {
            // create new page for this NPC
            Image npcImage = NPC.GetComponent<Image>();
            CharacterPage newPage = new CharacterPage(NPC.name, npcImage);
            updatePage(newPage, statement);

            characterProfile.Add(NPC.name, newPage);
        }
        else {
            // page already exists
            CharacterPage existingPage = (CharacterPage)characterProfile[NPC.name];

            // add statement if does not yet exist
            if (!existingPage.statementExists(statement)) {
                updatePage(existingPage, statement);
            }
        }
    }

    private void updatePage(CharacterPage page, string btnText) {
        page.addStatement(btnText);
        
        GameObject btnPrefab = (GameObject)Instantiate(Resources.Load("ButtonPrefab"), new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        //btnPrefab.transform.SetParent(contentPane.transform);
        Button statementBtn = btnPrefab.GetComponent<Button>();

        statementBtn.tag = "StatementButton";
        statementBtn.GetComponentInChildren<Text>().text = btnText;
        
        page.addStatementBtn(statementBtn);
    }

    public void showPage(int pageNumber) {

        if (characterProfile.Count >= (pageNumber + 1)) {
            // show the first character in dictionary
            CharacterPage page = (CharacterPage)characterProfile[pageNumber];
            List<Button> statementBtns = page.getStatementButtons();

            foreach (Button b in statementBtns) {
                b.transform.SetParent(contentPane.transform);
            }
        }
    }
}
