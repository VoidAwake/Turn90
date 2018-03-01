using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMovementController : MonoBehaviour {

	public float lerpSpeed;
	public GameObject coinContainer;

	private Vector2 initialPosition;
	private Vector2 newPosition;
	private Vector2[] positionArray = new Vector2[] {new Vector2 (0, 0), new Vector2 (0, 850), new Vector2(-800, 0), new Vector2(800, 0)};

	void Awake () {
		initialPosition = transform.position;	
		newPosition = transform.position;
	}

	void Update () {
		transform.position = Vector2.Lerp (transform.position, newPosition, Time.deltaTime * lerpSpeed);
	}

	public void changeNewPosition (int positionIndex) {
		newPosition = positionArray [positionIndex] + initialPosition;

		//if (positionIndex == 2) {
		//	coinContainer.SetActive (true);
		//} else {
		//	coinContainer.SetActive (false);
		//}
	}
}
