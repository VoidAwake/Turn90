using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes the camera follow the ball's y position
public class FollowBall : MonoBehaviour {

	public GameObject ball;

	private GameObject mainCamera;
	private bool followActivated = false;

	void Start () {
		ball = GameObject.FindGameObjectWithTag ("Player");

		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");

		if (ball != null && mainCamera != null) {
			followActivated = true;
		} else {
			Debug.Log ("Could not find main camera or player object!");
		}
	}

	void Update () {
		if (followActivated) {
			mainCamera.transform.position = new Vector3 (gameObject.transform.position.x, ball.transform.position.y, gameObject.transform.position.z - 2);
		}
	}
}
