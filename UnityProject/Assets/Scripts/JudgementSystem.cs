using UnityEngine;
using System.Collections;

public class JudgementSystem : MonoBehaviour {
    public int correctItemId;
    public string correctStatement;

    public int playerItemChoice;
    public string playerStatementChoice;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setItemID(int id ) {
        playerItemChoice = id;
    }

    public void setStatement(string statement) {
        playerStatementChoice = statement;
    }

    public void judge() {

        if ((correctItemId == playerItemChoice) && (correctStatement.Trim().Equals(playerStatementChoice.Trim()))) { }
    }

    public void showButton() {
        this.gameObject.SetActive(true);
    }
}
