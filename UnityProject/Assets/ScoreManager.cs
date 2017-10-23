using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ScoreManager
{

    private static ScoreManager _instance;

    // Constructor is 'protected'
    protected ScoreManager()
    {
    }

    public static ScoreManager Instance()
    {
        // Uses lazy initialization.
        // Note: this is not thread safe.
        if (_instance == null)
        {
            _instance = new ScoreManager();
        }

        return _instance;
    }

    public static Dictionary<string, List<string>> playerScores = new Dictionary<string, List<string>>();

    public string GetScore(string teamName, int levelType)
    {
        if (playerScores.ContainsKey(teamName) == false)
        {
            return "Error: TeamNotFound";
        }
        Debug.Log("sm lvl" + levelType);
        Debug.Log("sm stff" + playerScores[teamName][levelType]);
        return playerScores[teamName][levelType];
    }

    public void SetScore(string teamName, int levelType, string score)
    {

        if (playerScores.ContainsKey(teamName) == false)
        {
            playerScores.Add(teamName, new List<string>(new string[2]));
        }

        if (playerScores[teamName][levelType] != null)
        {
            string scr = playerScores[teamName][levelType];

            int score1 = getTimeAsInt(score);
            int score2 = getTimeAsInt(scr);

            if (score1 < score2)
            {
                playerScores[teamName].Insert(levelType, score);
            }
        }
        else
        {
            playerScores[teamName].Insert(levelType, score);
        }


    }


    private int getTimeAsInt(string str)
    {
        string[] sstring = str.Split(':');

        int minutes = int.Parse(sstring[0]);
        int seconds = int.Parse(sstring[1]);

        int totalSeconds = minutes + seconds;

        return totalSeconds;
    }
    public string[] GetPlayerNames()
    {
        return playerScores.Keys.ToArray();
    }
}
