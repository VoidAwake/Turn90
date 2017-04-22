using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checks the balls speed and triggers level resets
public class MomentumCheck : MonoBehaviour {
	
	public float speedToCount = 0.01f;
	public int countsToReset = 10;

	private GameObject gameControllerObject;
	private GameController gameControllerScript;
	private int count;

	void Start () {
		gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		gameControllerScript = gameControllerObject.GetComponent<GameController> ();

		if (gameControllerObject == null) {
			Debug.Log ("Could not find game controller object!");
		}

		count = 0;
	}

	void Update () {
		if (Mathf.Abs (gameObject.GetComponent<Rigidbody2D> ().velocity.x) < speedToCount && Mathf.Abs (gameObject.GetComponent<Rigidbody2D> ().velocity.y) < speedToCount) {
			count++;
		} else {
			count = 0;
		}

		if (count == countsToReset) {
			gameControllerScript.ResetScene (1);
		}
	}
}
