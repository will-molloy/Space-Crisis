using UnityEngine;
using System.Collections;

public class Leve2Controller : MonoBehaviour {
    public const int GRID_SIZE = 1;
    public static Sprite RED_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks1");
    public static Sprite ORANGE_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks2");
    public static Sprite YELLOW_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks3");
    public static Sprite GREEN_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks4");
    public static Sprite CYAN_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks5");
    public static Sprite BLUE_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks6");
    public static Sprite PURPLE_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks7");
    public static Sprite DISCO_1 = Resources.Load<Sprite>("Sprites/basetile2");
    public static Sprite DISCO_2 = Resources.Load<Sprite>("Sprites/basetile3");
    public static Sprite DISCO_3 = Resources.Load<Sprite>("Sprites/basetile4");

    public static Leve2Controller instance;

    public static int TOOL_BAR_SIZE = 4;
    public static int TOOL_BAR_ITEM_SIZE = 64;

    private int handLeftCurrPos, handRightCurrPos = 0;
    private GameObject handLeft, handRight;
    private float inverseMoveTime = 1000;
    private bool leftCoroutineState, rightCoroutineState = false;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(gameObject);
            return;
        }
        handLeft = GameObject.Find("HandLeft");
        handRight = GameObject.Find("HandRight");
        DontDestroyOnLoad(gameObject);
    }

    /* TODO: WARNING: Duplicated code, `eval`ing this would be good */
    public int CycleCursorLeft() {
        if(leftCoroutineState) return -1;
        handLeftCurrPos++;
        var rtf = handLeft.GetComponent<RectTransform>();
        if(handLeftCurrPos >= TOOL_BAR_SIZE) {
            leftCoroutineState = true;
            StartCoroutine(CycleCoRoutineLeftHand(rtf, rtf.transform.position + Vector3.up * TOOL_BAR_ITEM_SIZE  * (TOOL_BAR_SIZE - 1)));
            handLeftCurrPos = 0;
            return handLeftCurrPos;
        }
        else {
            leftCoroutineState = true;
            StartCoroutine(CycleCoRoutineLeftHand(rtf, rtf.transform.position + Vector3.down * (TOOL_BAR_ITEM_SIZE )));
            return handLeftCurrPos;
        }

    }

    private IEnumerator CycleCoRoutineLeftHand(Transform rtf, Vector3 end) {

        // Only one instace should be running
        float remainingDist = (rtf.position - end).sqrMagnitude;
        
        while(remainingDist > float.Epsilon) {
            Vector3 newPosition = Vector3.MoveTowards(handLeft.GetComponent<Rigidbody2D>().position, end, inverseMoveTime * Time.deltaTime);
            handLeft.GetComponent<Rigidbody2D>().MovePosition(newPosition);
            remainingDist = (rtf.position - end).sqrMagnitude;
            yield return null;
        }
        leftCoroutineState = false;
        yield return null;

    }

    public int CycleCursorRight() {
        if(rightCoroutineState) return -1;
        handRightCurrPos++;
        var rtf = handRight.GetComponent<RectTransform>();
        if(handRightCurrPos >= TOOL_BAR_SIZE) {
            rightCoroutineState = true;
            StartCoroutine(CycleCoRoutineRightHand(rtf, rtf.transform.position + Vector3.up * TOOL_BAR_ITEM_SIZE  * (TOOL_BAR_SIZE - 1)));
            handRightCurrPos = 0;
            return handRightCurrPos;
        }
        else {
            rightCoroutineState = true;
            StartCoroutine(CycleCoRoutineRightHand(rtf, rtf.transform.position + Vector3.down * (TOOL_BAR_ITEM_SIZE )));
            return handRightCurrPos;
        }

    }

    private IEnumerator CycleCoRoutineRightHand(Transform rtf, Vector3 end) {

        // Only one instace should be running
        float remainingDist = (rtf.position - end).sqrMagnitude;
        
        while(remainingDist > float.Epsilon) {
            Vector3 newPosition = Vector3.MoveTowards(handRight.GetComponent<Rigidbody2D>().position, end, inverseMoveTime * Time.deltaTime);
            handRight.GetComponent<Rigidbody2D>().MovePosition(newPosition);
            remainingDist = (rtf.position - end).sqrMagnitude;
            yield return null;
        }
        rightCoroutineState = false;
        yield return null;

    }



}
