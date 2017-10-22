using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetJudgementStatement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateStatement() {
        Debug.Log("update statement");
        GameObject judgebtn = GameObject.FindGameObjectWithTag("JudgementBtn");
        judgebtn.GetComponent<JudgementSystem>().setStatement(this.gameObject.GetComponentInChildren<Text>().text);
    }
}
