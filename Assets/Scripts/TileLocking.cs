using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Locks user interction with tiles while the ball is inside them
public class TileLocking : MonoBehaviour {
	public bool unlocked;

	void OnTriggerEnter2D (Collider2D other) {
		unlocked = false;
	}

	void OnTriggerExit2D (Collider2D other) {
		unlocked = true;
	}
}
