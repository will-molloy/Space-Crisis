using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeSlotBackground : MonoBehaviour {

    public Sprite redsSlot;
    public Sprite originalSlot;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        Debug.Log("Clicked");
        GetComponent<Image>().sprite = redsSlot;
    }

    void OnMouseExit()
    { Debug.Log("Clicked");
        GetComponent<Image>().sprite = originalSlot;
    }

    void OnMouseClick() {
        GetComponent<Image>().sprite = redsSlot;
    }
}
