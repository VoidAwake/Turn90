using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

	public GameObject tutorialContainer;
	public GameObject overlay;
	public float lerpTime;

	private bool overlayActive;

	void Update () {
		overlay.GetComponent<CanvasGroup> ().alpha = Mathf.Lerp(overlay.GetComponent<CanvasGroup> ().alpha, overlayActive ? 1 : 0, Time.fixedDeltaTime / lerpTime);
	}

	void OnTriggerEnter2D (Collider2D other) {
		overlayActive = true;

		Time.timeScale = 0;
	}

	public void closeTutorial () {
		overlayActive = false;

		Time.timeScale = 1;

		Destroy (overlay);

		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

		hit.collider.gameObject.transform.eulerAngles += new Vector3 (0, 0, -90);

		Destroy (tutorialContainer);
	}
}
