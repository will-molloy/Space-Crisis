using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundMusicManager : MonoBehaviour {
    private static BackgroundMusicManager instance = null;

    public List<string> Scenes = new List<string>();
    public static BackgroundMusicManager Instance
    {
        get { return instance; }
    }
    // Use this for initialization
    void Start () {
	
	}
	// Update is called once per frame
	void Update () {
        if (!Scenes.Contains(Application.loadedLevelName))
        {
            Destroy(this.gameObject);
        }

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
