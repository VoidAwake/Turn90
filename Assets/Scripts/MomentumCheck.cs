using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumCheck : MonoBehaviour {
	
	public float xGravityEffect;
	//public float timeToFirstCheck;
	//public bool checkEnabled;

	//private GameObject gameControllerObject;
	//private GameController gameControllerScript;
	//private int count;

	void Start () {
		//checkEnabled = false;

		//gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		//gameControllerScript = gameControllerObject.GetComponent<GameController> ();

		//count = 0;

		//if (gameControllerObject == null) {
		//	Debug.Log ("Could not find game controller object!");
		//}

		//yield return new WaitForSeconds (timeToFirstCheck);

		//checkEnabled = true;
	}

	void Update () {
		//if (checkEnabled) {
		//	if (Mathf.Abs (gameObject.GetComponent<Rigidbody2D> ().velocity.x) < 0.01 && Mathf.Abs (gameObject.GetComponent<Rigidbody2D> ().velocity.y) < 0.01) {
		//		count++;
		//	} else {
		//		count = 0;
		//	}

		//	if (count == 10) {
		//		gameControllerScript.ResetScene (1);
		//	}
		//}

		Physics2D.gravity = new Vector2 (gameObject.GetComponent<Rigidbody2D>().velocity.x * xGravityEffect, Physics2D.gravity.y);
	}
}
