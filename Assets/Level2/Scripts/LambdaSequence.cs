using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LambdaSequence : MonoBehaviour {
	
	// Would be nice if we have union types for this.
	[SerializeField]
	public List<GameObject> seq;

	public LambdaGrid start, end;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void Awake() {
	}

	public bool DiscriminatedAdd(GameObject obj) {
		if(IsValidSequenceObject(obj)) {
			seq.Add(obj);
			return true;
		}
		return false;
	}

	protected bool CheckValidity() {
		if(CheckIntegrity()) {
			if(seq.Count < 2) {
				return false;
			}
			return IsA<DisplayScreen>(seq[0]) && IsA<DisplayScreen>(seq[seq.Count-1]);
		}
		return false;
	}

	protected bool CheckIntegrity() {
		foreach(var i in seq) {
			if(!IsValidSequenceObject(i)) {
				return false;
			}
		}
		return true;
	}

	public static bool IsValidSequenceObject(GameObject obj) {
		// The object must be either a display or a slot
		return IsAorB<LambdaSlot, DisplayScreen>(obj);
	}

	public static bool IsA<T>(GameObject obj) {
		var comp = obj.GetComponent<T>();
		if(comp != null)
			return true;
		return false;
	}

	public static bool IsAorB<A,B>(GameObject obj) {
		return IsA<A>(obj) || IsA<B>(obj);
	}

	protected void StartCascadeUpdate() {
        Cascade(0, seq[0].GetComponent<DisplayScreen>().GetLambdaGrid());
	}

	protected void Cascade(int n, LambdaGrid nextGrid) {
		if(n >= seq.Count) return;
		var dp = seq[n].GetComponent<DisplayScreen>();
		if(dp == null) {
			// Not a display, apply the fuction
			var slot = seq[n].GetComponent<LambdaSlot>();
			var newGrid = nextGrid.Deepcopy();
			newGrid.Apply(slot.behavior);
			Cascade(n+1, newGrid);
		}
		else {
			// Is a display, display and don't alter it
			if(nextGrid != null) {
				dp.UpdateLambdaGrid(nextGrid);
			}
			else {
				dp.UpdateLambdaGrid(new LambdaGrid());
			}
			Cascade(n+1, dp.GetLambdaGrid());
		}
	}

	protected void DoCascade() {

	}


}
