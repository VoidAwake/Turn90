using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapChild : MonoBehaviour {

	public int childID;
	public bool checking = true;

	private Wrap wrapScript;

	// Use this for initialization
	void Start () {
		wrapScript = transform.parent.GetComponent <Wrap> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (checking) {
			wrapScript.OnChildTriggerEnter2D (other, childID);
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		wrapScript.OnChildTriggerExit2D (other, childID);
	}
}
