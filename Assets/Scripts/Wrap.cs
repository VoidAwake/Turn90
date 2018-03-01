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

	private GameObject gameControllerObject;
	private GameController gameControllerScript;
	private FollowBall followBallScript;

	private WrapChild wrapChildLeft;
	private WrapChild wrapChildRight;
	private int currentWrapChild;

	void Start () {
		wrapChildLeft = gameObject.transform.GetChild(0).gameObject.GetComponent<WrapChild>();
		wrapChildRight = gameObject.transform.GetChild (1).gameObject.GetComponent<WrapChild>();

		gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");

		if (gameControllerObject == null) {
			Debug.Log ("Could not find game controller object!");
		} else {
			gameControllerScript = gameControllerObject.GetComponent<GameController> ();

			followBallScript = gameControllerObject.GetComponent<FollowBall> ();
		}
	}

	public void OnChildTriggerEnter2D (Collider2D other, int childID) {
		currentWrapChild = childID;

		if (childID == 0) {
			wrapChildRight.checking = false;
		} else {
			wrapChildLeft.checking = false;
		}

		newBall = Instantiate (ballPrefab, new Vector3 (childID == 1 ? leftSpawnLocation : rightSpawnLocation, other.gameObject.transform.position.y, 1), other.transform.rotation);

		newBall.GetComponent<Rigidbody2D> ().velocity = other.GetComponent<Rigidbody2D> ().velocity;

		newBall.GetComponent<Rigidbody2D> ().angularVelocity = other.GetComponent<Rigidbody2D> ().angularVelocity;

		newBall.GetComponent<MomentumCheck> ().enabled = true;

		gameControllerScript.ball.GetComponent<MomentumCheck> ().enabled = false;

		gameControllerScript.ball = newBall;

		followBallScript.ball = newBall;

		Destroy (other.gameObject, timeToDestroy);
	}

	public void OnChildTriggerExit2D (Collider2D other, int childID) {
		if (childID != currentWrapChild) {
			if (currentWrapChild == 0) {
				wrapChildRight.checking = true;
			} else {
				wrapChildLeft.checking = true;
			}
		}

		if (gameControllerScript.ball == other.gameObject) {
			if (((other.gameObject.GetComponent <Rigidbody2D> ().velocity.x > 0 && currentWrapChild == 0) || (other.gameObject.GetComponent <Rigidbody2D> ().velocity.x < 0 && currentWrapChild == 1))) {
				gameControllerScript.ResetScene (1);
			}
		}
	}
}
