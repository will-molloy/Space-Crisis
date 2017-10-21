using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections.Specialized;

public class CharacterContent : MonoBehaviour
{
    public static OrderedDictionary characterProfile = new OrderedDictionary();
    public GameObject contentPane;
    public int index;
    public int size = characterProfile.Count;
    public string npcFirstLine;
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
        KeyValuePair<Image, List<string>> page = (KeyValuePair<Image, List<string>>)characterProfile[index];
        npcFirstLine = page.Value[0];
    }

    public void addStatement(GameObject NPC, string statement)
    {
        if (!characterProfile.Contains(NPC.name))
        {
            // create new page for this NPC
            Image npcImage = NPC.GetComponent<Image>();
            KeyValuePair<Image, List<string>> newPage = new KeyValuePair<Image, List<string>>(npcImage, new List<string>());
            
            newPage.Value.Add(statement);
            characterProfile.Add(NPC.name, newPage);
            size = characterProfile.Count;
        }
        else
        {
            // page already exists
            KeyValuePair<Image, List<string>> existingPage = (KeyValuePair<Image, List<string>>)characterProfile[NPC.name];

            // add statement if does not yet exist
            if (!existingPage.Value.Contains(statement))
            {
                existingPage.Value.Add(statement);
            }
        }
    }

    public void showPage(int index)
    {
        if (characterProfile.Count >= (index + 1))
        {
            // show the first character in dictionary
            KeyValuePair<Image, List<string>> page = (KeyValuePair<Image, List<string>>)characterProfile[index];
            List<string> statements = page.Value;

            //show buttons -> only if the content is empty
            if (contentPane.transform.childCount == 0)
            {
                foreach (string s in statements)
                {
                    GameObject btnPrefab = (GameObject)Instantiate(Resources.Load("ButtonPrefab"), new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
                    btnPrefab.transform.SetParent(contentPane.transform);
                    Button statementBtn = btnPrefab.GetComponent<Button>();

                    statementBtn.tag = "StatementButton";
                    statementBtn.GetComponentInChildren<Text>().text = s;

                    //page.addStatementBtn(statementBtn);
                }
            }

            //show alien name
            GameObject npcNameUI = this.transform.Find("AlienName").gameObject;
            npcNameUI.GetComponent<Text>().text = findNpcNameGivenIndex(index);

            //show alien image
            Image npcImageUI = this.transform.Find("AlienImage").gameObject.GetComponent<Image>();
            npcImageUI.sprite = page.Key.sprite;

            
        }
    }

    private string findNpcNameGivenIndex(int index) {
        string[] keys = new string[characterProfile.Keys.Count];
        characterProfile.Keys.CopyTo(keys, 0);
        return keys[index];
    }

    public void nextPage()
    {

        clearScreen();
        if ((index + 1) < characterProfile.Count)
        {
            index++;
        }
        else
        {
            index = 0;
        }
        showPage(index);
    }

    public void previousPage()
    {
        clearScreen();

        if ((index - 1) >= 0)
        {
            index--;
        }
        else
        {
            index = 0;
        }
        showPage(index);
    }

    private void clearScreen()
    {
        // clear screen before showing new page
        foreach (Transform t in contentPane.transform)
        {
            if (t.tag.Equals("StatementButton"))
            {
                Destroy(t.gameObject);
                //t.gameObject.SetActive(false);
            }
        }
    }
}
