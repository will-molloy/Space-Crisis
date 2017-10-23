using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameController.PlayableScene sceneToLoad;
    public Sprite[] sprites = new Sprite[3];
    private List<GameObject> colliders;
    public float gracePeriod = 2f;

    private DialogueManager dMan;
    public GameObject warningBox;
    private bool warned = false;
    
    // Use this for initialization
    void Start()
    {
        dMan = FindObjectOfType<DialogueManager>();
        colliders = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGracePeriod();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = sprites[2 - colliders.Count];

        bool allDone = dMan.getAllDone();
        if (colliders.Count == 2 && gracePeriod < 1)
        {
            if (dMan.allDone || (warningBox == null))
            {
                SceneManager.LoadScene(GameController.GetFileName(sceneToLoad));
            }
            else {
                // player has not correctly interacted with NPC
                if (!warned)
                {
                    dMan.getActiveNPC().GetComponent<DialogHolder>().setAndShowDialogue(warningBox);
                    warned = true;
                }
            }
        }
    }

    private void UpdateGracePeriod()
    {
        if (gracePeriod < 1)
        {
            gracePeriod = 0;
        }
        else
        {
            gracePeriod -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        warned = false;
        if (IsValidCollider(other))
        {
            colliders.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (IsValidCollider(other))
        {
            colliders.Remove(other.gameObject);
        }
    }

    private bool IsValidCollider(Collider2D col)
    {
        return col.gameObject.CompareTag("Player");
    }
}
