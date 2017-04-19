using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Actions to occur on the triggering of a Pickup object
public class Pickup : MonoBehaviour {
	private GameObject gameControllerObject;
	private GameController gameControllerScript;

	void Start () {
		gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");

		if (gameControllerObject == null) {
			Debug.Log ("Could not find game controller object!");
		} else {
			gameControllerScript = gameControllerObject.GetComponent<GameController> ();
		}
	}
		
	void OnTriggerEnter2D (Collider2D other) {
		gameControllerScript.AddScore ();

		Destroy (gameObject);
	}
}
