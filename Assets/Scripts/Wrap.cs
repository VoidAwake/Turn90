using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allow the ball to "wrap" out of one side of the screen and into the other
public class Wrap : MonoBehaviour {

	public GameObject ballPrefab;
	public float timeDisabled;
	public float leftSpawnLocation;
	public float rightSpawnLocation;
	public float timeToDestroy;

	private GameObject newBall;
	private bool wrappingEnabled = true;
	private GameObject gameControllerObject;
	private FollowBall followBallScript;

	void Start () {
		gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");

		if (gameControllerObject == null) {
			Debug.Log ("Could not find game controller object!");
		} else {
			followBallScript = gameControllerObject.GetComponent<FollowBall> ();
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (wrappingEnabled) {
			wrappingEnabled = false;

			StartCoroutine ("EnableTimer");

			newBall = Instantiate (ballPrefab, new Vector3 (other.GetComponent<Rigidbody2D> ().velocity.x > 0 ? leftSpawnLocation : rightSpawnLocation, other.gameObject.transform.position.y, 1), other.transform.rotation);

			newBall.GetComponent<Rigidbody2D> ().velocity = other.GetComponent<Rigidbody2D> ().velocity;
		
			newBall.GetComponent<Rigidbody2D> ().angularVelocity = other.GetComponent<Rigidbody2D> ().angularVelocity;

			// Passes the ball clone to the follow ball script
			followBallScript.ball = newBall;

			Destroy (other.gameObject, timeToDestroy);
		}
	}

	IEnumerator EnableTimer () {
		yield return new WaitForSeconds (timeDisabled);

		wrappingEnabled = true;
	}
}
