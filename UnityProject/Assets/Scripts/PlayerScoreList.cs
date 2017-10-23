using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class PlayerScoreList : MonoBehaviour {

    public GameObject playerScoreEntryPrefab;

    ScoreManager sm;

    List<ScoreObject> scoreList = new List<ScoreObject>();

	// Use this for initialization
	void Start () {
        sm = ScoreManager.Instance();

        string[] names = sm.GetPlayerNames();

        while (this.transform.childCount > 0)
        {
            Transform c = this.transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }

        foreach (string name in names)
        {

            Debug.Log("a name" + name);
            for (int i = 0; i < 2; i++)
            {
                string score = sm.GetScore(name, i);
                if (score != null)
                {
                    Debug.Log("a score" + name + i);

                    scoreList.Add(new ScoreObject(name, i + 1, sm.GetScore(name, i)));
                    /*
                    go.transform.Find("TeamName").GetComponent<Text>().text = name;
                    int level = i + 1;
                    go.transform.Find("Level").GetComponent<Text>().text = level.ToString();
                    go.transform.Find("HighScore").GetComponent<Text>().text = sm.GetScore(name, i);
                    */

                }
            }




        }

        scoreList.Sort((x, y) => x.totalSeconds.CompareTo(y.totalSeconds));
        foreach (ScoreObject sob in scoreList)
        {
            GameObject go = (GameObject)Instantiate(playerScoreEntryPrefab);
            go.transform.SetParent(this.transform);
            go.transform.Find("TeamName").GetComponent<Text>().text = sob._name;
            go.transform.Find("Level").GetComponent<Text>().text = sob._level.ToString();
            go.transform.Find("HighScore").GetComponent<Text>().text = sob._score;
        }
    }
	
	// Update is called once per frame
	void Update () { 



    }
}
