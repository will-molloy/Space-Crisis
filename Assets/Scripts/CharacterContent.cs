using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CharacterContent : MonoBehaviour
{
    public List<Button> statementBtns;
    public List<Text> statements;
    private List<string> statementStrings;

    // Use this for initialization
    void Start()
    {
        statements = new List<Text>();
        statementStrings = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void addStatement(string statement)
    {
        if (!statementStrings.Contains(statement)){
            statementStrings.Add(statement);
            /* GameObject UItext = new GameObject(statement);
             UItext.transform.SetParent(this.gameObject.transform);

             //RectTransform trans = UItext.AddComponent<RectTransform>();

             Text text = UItext.AddComponent<Text>();
             text.text = statement;
             text.color = Color.black;
             text.fontSize = 14;
             text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
             statements.Add(text);
        */

            /*create button from prefab
            GameObject btnPrefab = (GameObject)Instantiate(Resources.Load("ButtonPrefab"));
            Button statementBtn = btnPrefab.GetComponent<Button>();

            statementBtn.tag = "StatementButton";
            statementBtn.GetComponentInChildren<Text>().text = statement;
            //statementBtn.transform.SetParent(FindObjectOfType<CharacterContent>().transform);*/

            //-----\
          
            GameObject btnPrefab = (GameObject)Instantiate(Resources.Load("ButtonPrefab"),new Vector3(transform.position.x, transform.position.y), Quaternion.identity );
            btnPrefab.transform.SetParent(this.gameObject.transform);
            Button statementBtn = btnPrefab.GetComponent<Button>();

          
           // buttonToAdd.targetGraphic = statementBtn.targetGraphic;

            statementBtn.tag = "StatementButton";
            statementBtn.GetComponentInChildren<Text>().text = statement;

            statementBtns.Add(statementBtn);
        }
    }
}
