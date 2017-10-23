using UnityEngine;
using System.Collections;

public class BackgroundMusicManager : MonoBehaviour {
    private static BackgroundMusicManager instance = null;
    public static BackgroundMusicManager Instance
    {
        get { return instance; }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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
