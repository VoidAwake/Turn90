using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines how user interects with the tiles
public class TileControl : MonoBehaviour {

	public int tileControlMethod = 0;

	// Swipe control variables
	private Vector2 touchOrigin = -Vector2.one;
	private Ray ray;
	private RaycastHit2D hit;

	void Update() {
		if (tileControlMethod == 0) {
			// Tap to rotate 90 degrees clockwise
			hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

			if (hit.collider != null) {
				if (hit.transform.gameObject.tag == "Tile") {
					if (hit.collider.gameObject.GetComponentsInChildren<TileLocking> () [0].unlocked) {
						if (Input.GetMouseButtonDown (0)) {
							hit.collider.gameObject.transform.eulerAngles += new Vector3 (0, 0, -90);
						}
					}
				}
			}
		} else if (tileControlMethod == 1) {
			//Swipe to rotate 90 degrees
			if (Input.touchCount > 0) {
				Debug.Log ("yes");
				Touch myTouch = Input.touches [0];

				if (myTouch.phase == TouchPhase.Began) {
					touchOrigin = myTouch.position;

					ray = Camera.main.ScreenPointToRay (myTouch.position);
					hit = Physics2D.Raycast (ray.origin, ray.direction);
				} else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
					Vector2 touchEnd = myTouch.position;
					float x = touchEnd.x - touchOrigin.x;
					float y = touchEnd.y - touchOrigin.y;
					touchOrigin.x = -1;
					if (Mathf.Abs (x) > Mathf.Abs (y)) {
						if (hit != null) {
							hit.collider.gameObject.transform.eulerAngles += new Vector3 (0, 0, x > 0 ? -90 : 90);
						}
					}
				}
			}
		} else if (tileControlMethod == 2) {
			// Swipe to flip
			if (Input.touchCount > 0) {
				Debug.Log ("yes");
				Touch myTouch = Input.touches [0];

				if (myTouch.phase == TouchPhase.Began) {
					touchOrigin = myTouch.position;

					ray = Camera.main.ScreenPointToRay (myTouch.position);
					hit = Physics2D.Raycast (ray.origin, ray.direction);
				} else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
					Vector2 touchEnd = myTouch.position;
					float x = touchEnd.x - touchOrigin.x;
					float y = touchEnd.y - touchOrigin.y;
					touchOrigin.x = -1;
					int direction = 0;
					if (hit != null) {
						if (hit.collider.gameObject.GetComponentsInChildren<TileLocking> () [0].unlocked) {
							if (Mathf.Abs (x) > Mathf.Abs (y)) {
								direction = x > 0 ? 2 : 4;
							} else {
								direction = y > 0 ? 1 : 3;
							}
						}
					}
					int rotation = Mathf.RoundToInt(hit.collider.gameObject.transform.eulerAngles.z);
					switch (rotation < 0 ? rotation + 360 : rotation) {
					case 0:
						if (direction == 3) {
							hit.collider.gameObject.transform.eulerAngles = new Vector3 (0, 0, -90);
						} else if (direction == 4) {
							hit.collider.gameObject.transform.eulerAngles = new Vector3 (0, 0, 90);
						}
						break;
					case 90:
						if (direction == 3) {
							hit.collider.gameObject.transform.eulerAngles = new Vector3 (0, 0, 180);
						} else if (direction == 2) {
							hit.collider.gameObject.transform.eulerAngles = new Vector3 (0, 0, 0);
						}
						break;
					case 180:
						if (direction == 1) {
							hit.collider.gameObject.transform.eulerAngles = new Vector3 (0, 0, 90);
						} else if (direction == 2) {
							hit.collider.gameObject.transform.eulerAngles = new Vector3 (0, 0, -90);
						}
						break;
					case 270:
						if (direction == 4) {
							hit.collider.gameObject.transform.eulerAngles = new Vector3 (0, 0, 180);
						} else if (direction == 1) {
							hit.collider.gameObject.transform.eulerAngles = new Vector3 (0, 0, 0);
						}
						break;
					}
				}
			}
		}
	}

	public void ChangeTileControlMethod (int newControlMethod) {
		tileControlMethod = newControlMethod;
	}
}
