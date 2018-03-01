using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes the camera follow the ball's y position
public class FollowBall : MonoBehaviour {

	public GameObject ball;

	private GameObject mainCamera;

	void OnEnable () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	void Update () {
		if (ball != null) {
			mainCamera.transform.position = new Vector3 (gameObject.transform.position.x, ball.transform.position.y, gameObject.transform.position.z - 2);
		}
	}
}
