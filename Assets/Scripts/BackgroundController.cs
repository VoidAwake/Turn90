using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	public GameObject backgroundPrefab;
	public float backgroundSpeed;
	public float startPosition;
	public float panelHeight;
	public GameObject cameraInstance;

	private GameObject panelOne;
	private GameObject panelTwo;
	private GameObject panelThree;

	// Use this for initialization
	void Start () {
		panelOne = Instantiate (backgroundPrefab, new Vector3 (1.5f, startPosition + panelHeight, 5f), Quaternion.Euler (180, 0, 0), gameObject.transform);
		panelTwo = Instantiate (backgroundPrefab, new Vector3 (1.5f, startPosition, 5f), Quaternion.Euler (180, 0, 0), gameObject.transform);
		panelThree = Instantiate (backgroundPrefab, new Vector3 (1.5f, startPosition - panelHeight, 5f), Quaternion.Euler (180, 0, 0), gameObject.transform);
	}
	
	// Update is called once per frame
	void Update () {
		panelOne.transform.Translate (Vector3.down * Time.deltaTime * backgroundSpeed);
		panelTwo.transform.Translate (Vector3.down * Time.deltaTime * backgroundSpeed);
		panelThree.transform.Translate (Vector3.down * Time.deltaTime * backgroundSpeed);
	}

	void OnTriggerEnter (Collider other) {
		Destroy (other.gameObject);
		panelOne = panelTwo;
		panelTwo = panelThree;
		panelThree = Instantiate (backgroundPrefab, new Vector3 (1.5f, panelTwo.transform.position.y - panelHeight, 5f), Quaternion.Euler (180, 0, 0), gameObject.transform); 
	}
}
