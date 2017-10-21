using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CharacterContent : MonoBehaviour
{
    public Dictionary<string, CharacterPage> characterProfile;

    // Use this for initialization
    void Start()
    {
        characterProfile = new Dictionary<string, CharacterPage>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addStatement(GameObject NPC, string statement)
    {
        if (!characterProfile.ContainsKey(NPC.name))
        {
            // create new page for this NPC
            Image npcImage = NPC.GetComponent<Image>();
            CharacterPage newPage = new CharacterPage(NPC.name, npcImage);
            updatePage(newPage, statement);

            characterProfile.Add(NPC.name, newPage);
        }
        else {
            // page already exists
            CharacterPage existingPage = characterProfile[NPC.name];

            // add statement if does not yet exist
            if (!existingPage.statementExists(statement)) {
                updatePage(existingPage, statement);
            }
        }
    }

    private void updatePage(CharacterPage page, string btnText) {
        page.addStatement(btnText);
        
        GameObject btnPrefab = (GameObject)Instantiate(Resources.Load("ButtonPrefab"), new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        btnPrefab.transform.SetParent(this.gameObject.transform);
        Button statementBtn = btnPrefab.GetComponent<Button>();

        statementBtn.tag = "StatementButton";
        statementBtn.GetComponentInChildren<Text>().text = btnText;
        
        page.addStatementBtn(statementBtn);
    }
}
