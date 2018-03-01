using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes the camera follow the ball's y position
public class FollowBall : MonoBehaviour {

	public GameObject ball;

	private GameObject mainCamera;

	// Disabled in levels, enabled by GameController script when in infinite mode
	void OnEnable () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	void Update () {
		if (ball != null) {
			if (ball.transform.position.y < mainCamera.transform.position.y) {
				mainCamera.transform.position = new Vector3 (mainCamera.transform.position.x, ball.transform.position.y, mainCamera.transform.position.z);
			}
		}
	}
}
