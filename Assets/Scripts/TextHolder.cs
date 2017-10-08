using UnityEngine;
using System.Collections;

public class TextHolder : MonoBehaviour
{
    public TextAsset textFile;
    public string[] textLines;
    public int lineToBreak;
    // public Item item to check
    public bool autoDialog;

    // Use this for initialization
    void Start()
    {

        if (textFile != null)
        {
            textLines = textFile.text.Split('\n');
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
