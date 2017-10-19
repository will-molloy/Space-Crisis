using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class LambdaSlot : MonoBehaviour {

	public LambdaBehavior behavior;
	// Use this for initialization
	private Animator animator;

	public float interactionDelay = 0.5f;
	void Start () {
		animator = GetComponent<Animator>();
		behavior = new LambdaBehavior(i => i.Identity());
		behavior.desc = "identity";
		behavior.extraAction = new LambdaBehavior.ExtraActionOnTMProUI(ui => {ui.fontSize = 16; ui.alignment = TMPro.TextAlignmentOptions.Center;});
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
	/** @NULLABLE
	 */
	public LambdaBehavior RemoveLambda() {
		var ret = behavior;
		behavior = null;
		return ret;
	}

	public bool HasLambdaInSlot() {
		return behavior != null;
	}

}
