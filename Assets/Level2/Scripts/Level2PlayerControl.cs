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
    public float moveTime = 100;
    public KeyCode inventoryCycleKey;
    public KeyCode actionKey;

    private BoxCollider2D selfCollider;
    private Rigidbody2D rigidbody;
    private float inverseMoveTime;
    private bool coroutineState;
    private Face facing;

    // Use this for initialization
    void Start()
    {
        selfCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
        coroutineState = false;
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
        if(Input.GetKey(actionKey)) {
            TryPickup();
        }
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

    bool Move(int x, int y, out RaycastHit2D hit)
    {
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
        hit = Physics2D.Linecast(currPos, end, wallLayer);
        // Re enable it
        selfCollider.enabled = true;
        if(hit.transform != null) {
            return false;
        }
        if(!coroutineState) {
            coroutineState = true;
            StartCoroutine(DoMove(end));
        }
        // Return true on able to move?
        return true;
    }

    void TryPickup() {
        var hit = Physics2D.Linecast(transform.position, transform.position + (Vector3)facing.GetVector(), pickupLayer);

        if(hit.transform != null) {
            GameObject.Destroy(hit.transform.gameObject);
        }

    }

    void TryMove(int x, int y)
    {
        RaycastHit2D hit;
        if (Move(x, y, out hit))
        {
            if(hit.transform == null)
                return;
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

