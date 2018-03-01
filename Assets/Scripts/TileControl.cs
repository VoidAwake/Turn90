using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines how user interects with the tiles
public class TileControl : MonoBehaviour {

	private Ray ray;
	private RaycastHit2D hit;

	void Update() {
		if (!PauseMenuController.paused) {
			// Tap to rotate 90 degrees clockwise
			hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

			if (hit.collider != null) {
				if (Input.GetMouseButtonDown (0)) {
					if (hit.transform.gameObject.tag == "Tile") {
						if (hit.collider.gameObject.GetComponentsInChildren<TileLocking> () [0].unlocked) {
						
							hit.collider.gameObject.transform.eulerAngles += new Vector3 (0, 0, -90);
						}
					} else if (hit.transform.gameObject.tag == "Close Tutorial") {
						GameObject.FindGameObjectWithTag ("Tutorial").GetComponent <TutorialController> ().closeTutorial ();
					}
				}
			}
		}
	}
}
