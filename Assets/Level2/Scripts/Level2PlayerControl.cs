using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Level2PlayerControl : MonoBehaviour
{
    public string horizontalKey;
    public string verticalKey;
    public LayerMask wallLayer;
    public LayerMask pickupLayer;
    public float moveTime = 0.1f;
    public KeyCode inventoryCycleKeyLeft;
    public KeyCode inventoryCycleKeyRight;
    public KeyCode actionKey;
    public KeyCode dropKey;
    public GameObject lambdaItemPrefab;

    private BoxCollider2D selfCollider;
    private Rigidbody2D rigidbody;
    private float inverseMoveTime;
    private bool coroutineState;
    private Face facing;
    private float interactionDelay = 0.33f;
    private float timeStamp;

    // Use this for initialization
    void Start()
    {
        selfCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
        coroutineState = false;
        if(inventoryCycleKeyLeft == KeyCode.None && inventoryCycleKeyRight == KeyCode.None) {
            throw new System.Exception("One of the key must be null");
        }
        timeStamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        int horizontal = (int)Input.GetAxisRaw(horizontalKey);
        int vertical = (int)Input.GetAxisRaw(verticalKey);

        if (horizontal != 0 || vertical != 0)
        {
            TryMove(horizontal, vertical);
        }
        else if(Input.GetKey(actionKey)) {
            TryInteract();
        }
        else if(Input.GetKey(inventoryCycleKeyLeft)) {
            Leve2Controller.instance.CycleCursorLeft();
        }
        else if(Input.GetKey(inventoryCycleKeyRight)) {
            Leve2Controller.instance.CycleCursorRight();
        }
        else if(Input.GetKey(dropKey)) {
            TryDrop();
        }
    }

    private Leve2Controller.PlayerSide DetermineSide() {
        if (inventoryCycleKeyLeft != KeyCode.None)
        {
            // must be the player on left
            return Leve2Controller.PlayerSide.LEFT;
        }
        else if (inventoryCycleKeyRight != KeyCode.None)
        {
            return Leve2Controller.PlayerSide.RIGHT;
        }
        else throw new System.Exception("Both keys are null on this player");

    }

    private IEnumerator DoMove(Vector3 end)
    {
        // Only one instace should be running
        float remainingDist = (transform.position - end).sqrMagnitude;
        
        while(remainingDist > float.Epsilon) {
            Vector3 newPosition = Vector3.MoveTowards(rigidbody.position, end, inverseMoveTime * Time.deltaTime);
            rigidbody.MovePosition(newPosition);
            remainingDist = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        coroutineState = false;
        yield return null;

    }

    void SetFacing(Face newFacing) {
        if(facing == newFacing)
            return;

        float zRot = transform.rotation.z;
        //transform.Rotate(Quaternion.identity);
        /* 
         0 -> UP
         90 -> LEFT
         180 -> DOWN
         270 -> RIGHT
         */
        
        facing = newFacing;
        switch(facing) {
            case Face.UP: 
                transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case Face.LEFT:
                transform.localEulerAngles = new Vector3(0, 0, 90);
                break;
            case Face.DOWN:
                transform.localEulerAngles = new Vector3(0, 0, 180);
                break;
            case Face.RIGHT:
                //transform.Rotate(new Vector3(0, 0, 90));
                transform.localEulerAngles = new Vector3(0, 0, 270);
                break;
        }

    }

    bool Move(int x, int y)
    {
        if(coroutineState) { 
            return false;
        }
        Vector2 currPos = transform.position;
        Vector2 end;
        // Vertical (y) takes priority
        if (y != 0)
        {
            if (y < 0) {
                end = currPos + Vector2.down;
                SetFacing(Face.DOWN);
            }
            else
            {
                end = currPos + Vector2.up;
                SetFacing(Face.UP);
            }
        }
        else {
            if(x < 0) {
                end = currPos + Vector2.left;
                SetFacing(Face.LEFT);
            }
            else{
                end = currPos + Vector2.right;
                SetFacing(Face.RIGHT);
            }
        }
        // Disable self collider
        selfCollider.enabled = false;
        var hit = Physics2D.Linecast(currPos, end, wallLayer);
        // Re enable it
        selfCollider.enabled = true;
        if(hit.transform != null) {
            return false;
        }
        coroutineState = true;
        StartCoroutine(DoMove(end));
        return true;
    }


    private void TryDrop() {
        // First we check if there is anything in the front
        var hits = Physics2D.LinecastAll(transform.position, transform.position + (Vector3)facing.GetVector(), wallLayer | pickupLayer);
        if(hits.Length > 0) {
            return;
        }
        var side = DetermineSide();
        var beh = Leve2Controller.instance.GetInventoryNForPlayer(side);
        if(beh != null) {
            var go = Instantiate(lambdaItemPrefab);
            var parent = GameObject.Find("Items");
            go.transform.SetParent(parent.transform);
            go.transform.localPosition = transform.position + (Vector3)facing.GetVector();
            go.transform.localScale = new Vector3(3.125f, 3.125f, 0);
            go.layer = pickupLayer;
            var itemScript = go.AddComponent<LambdaItemScript>();
            itemScript.lambdaBehavior = beh;
            itemScript.description = beh.desc;
        }
    }

    void TryInteract() {
        // Try pickup first
        var hit = Physics2D.Linecast(transform.position, transform.position + (Vector3)facing.GetVector(), pickupLayer);

        if(hit && hit.transform != null) {
            var script = hit.transform.gameObject.GetComponent<LambdaItemScript>();
            if(script != null && script.lambdaBehavior != null) {
                // We check if any of the key is null
                var side = DetermineSide();
                if(Leve2Controller.instance.PutInInventory(side, script.lambdaBehavior)) {
                    GameObject.Destroy(hit.transform.gameObject);
                }
            }
            return;
        }


        // Check if we are allowed to interact with a slot
        if(timeStamp > Time.time) {
            return;
        }
        else {
            timeStamp = Time.time + interactionDelay;
        }


        // Then, try interract with a slot 
        var hits = Physics2D.LinecastAll(transform.position, transform.position + (Vector3)facing.GetVector());
        foreach(var h in hits) {
            if(h.transform.gameObject.tag == "LambdaSlot") {
                var slot = h.transform.GetComponent<LambdaSlot>();
                var side = DetermineSide();
                // First we check if there is something in the slot and we have a free place in inventory
                if (slot.HasLambdaInSlot())
                {
                    if (!Leve2Controller.instance.IsCurrentInventorySlotTaken(side))
                    {
                        // The slot is not taken
                        Leve2Controller.instance.PutInInventory(Leve2Controller.PlayerSide.LEFT, slot.RemoveLambda());
                    }
                    return;
                }
                else {
                    LambdaBehavior beh;
                    beh = Leve2Controller.instance.GetInventoryNForPlayer(side);
                    if (beh != null)
                        slot.InsertLambda(beh);
                    return;
                }
            }
        }
    }

    void TryMove(int x, int y)
    {
        RaycastHit2D hit;
        if (Move(x, y))
        {
        }
    }
}

/*
case class Face {}
object Face {
    def getVector(Face f) : Vector2 = ???
}
 */
enum Face
{
    UP, DOWN, LEFT, RIGHT
}
static class FaceMethods
{
    public static Vector2 GetVector(this Face face)
    {
        switch (face)
        {
            case Face.UP:
                return Vector2.up;
            case Face.LEFT:
                return Vector2.left;
            case Face.DOWN:
                return Vector2.down;
            case Face.RIGHT:
                return Vector2.right;
            default: return Vector2.zero;
        }
    }
}


