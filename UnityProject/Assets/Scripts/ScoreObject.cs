using UnityEngine;
using System.Collections;

public class ScoreObject : MonoBehaviour {

    public string _name { get; set; }
    public int _level { get; set; }
    public string _score { get; set; }
    public int totalSeconds { get; set; }

    public ScoreObject(string name, int lvl, string scr)
    {
        _name = name;
        _level = lvl;
        _score = scr;

        string[] sstring = _score.Split(':');

        int minutes = int.Parse(sstring[0]);
        int seconds = int.Parse(sstring[1]);

        totalSeconds = minutes + seconds;

  



    }

}
