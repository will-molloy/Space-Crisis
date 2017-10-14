using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class LambdaSlot : MonoBehaviour {

	public LambdaBehavior behavior;
	// Use this for initialization
	private Animator animator;
	void Start () {
		animator = GetComponent<Animator>();
		behavior = new LambdaBehavior(i => i.Identity());
	}
	
	// Update is called once per frame
	void Update () {
		if(behavior == null) {
			animator.SetBool("Active", false);
		}
		else {
			animator.SetBool("Active", true);
		}
	
	}

	public void InsertLambda(LambdaBehavior beh) {
		behavior = beh;
	}

	// Return an item here instead?
	public void RemoveLambda() {
		behavior = null;
	}
}
