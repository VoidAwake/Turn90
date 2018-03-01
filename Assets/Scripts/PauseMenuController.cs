using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour {

	public static bool paused = false;

	public float lerpTime;

	void Awake () {
		gameObject.GetComponent<CanvasGroup> ().alpha = 0;
	}

	public void ToggleMenu () {
		paused = paused ? false : true;

		Time.timeScale = paused ? 0 : 1;

		gameObject.GetComponent<Image> ().raycastTarget = paused ? true : false;

		gameObject.GetComponent<CanvasGroup> ().interactable = paused ? true : false;

		gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = paused ? true : false;
	}

	void Update () {
		gameObject.GetComponent<CanvasGroup> ().alpha = Mathf.Lerp(gameObject.GetComponent<CanvasGroup> ().alpha, paused ? 1 : 0, Time.fixedDeltaTime / lerpTime);
	}

	public void MainMenu () {
		ToggleMenu ();

		SceneManager.LoadScene (0);
	}

	public void Reset () {
		ToggleMenu ();

		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().ResetScene (0);
	}
}
