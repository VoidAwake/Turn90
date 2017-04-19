using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GUIText scoreText;
	public GameObject overlay;
	public GameObject overlayLevelNumber;
	public GameObject overlayMessage;
	public float timeToLerp;
	public float lerpTime;
	public float yGravityEffect;
	public GameObject explosionPrefab;
	public AudioClip pickupSound;
	public AudioClip explosionSound;
	public AudioClip levelCompleteSound;

	private int score;
	private int pickupQuantity;
	private bool lerpEnabled = false;
	private GameObject ball;
	private GameObject variableStorageObject;
	private VariableStorage variableStorageScript;
	private bool slowingEnabled = false;


	IEnumerator Start () {
		score = 0;
		pickupQuantity = GameObject.FindGameObjectsWithTag ("Pickup").Length;
		UpdateScore ();

		variableStorageObject = GameObject.FindGameObjectWithTag ("Storage");
		variableStorageScript = variableStorageObject.GetComponent<VariableStorage> ();

		ball = GameObject.FindGameObjectWithTag ("Player");

		ball.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;

		overlayLevelNumber.GetComponent<Text> ().text = (SceneManager.GetActiveScene ().buildIndex + 1).ToString();

		overlayMessage.GetComponent<Text> ().text = variableStorageScript.message;

		yield return new WaitForSeconds (timeToLerp);

		lerpEnabled = true;

		ball.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
	}

	void Update () {
		if (lerpEnabled) {
			Lerp ();
		}
	}

	public void AddScore () {
		score += 1;
		UpdateScore ();
		gameObject.GetComponent<AudioSource> ().PlayOneShot (pickupSound);
	}

	void UpdateScore () {
		scoreText.text = score + " / " + pickupQuantity;
	}

	public void ResetScene (int resetCondition)  {
		switch (resetCondition) {
		case 0:
			variableStorageScript.message = "Level Reset";
			break;
		case 1:
			variableStorageScript.message = "Keep the ball moving!";
			break;
		case 2:
			variableStorageScript.message = "Get all the coins!";
			break;
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void NextScene ()  {
		if (SceneManager.GetActiveScene ().buildIndex < 2) {
			variableStorageScript.message = "";

			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		} else {
			variableStorageScript.message = "";

			SceneManager.LoadScene (0);
		}
	}
		
	IEnumerator OnTriggerEnter2D (Collider2D other) {
		if (score == pickupQuantity) {
			

			yield return new WaitForSeconds (0.5f);

			gameObject.GetComponent<AudioSource> ().PlayOneShot (levelCompleteSound);

			yield return new WaitForSeconds (2.5f );

			variableStorageScript.message = "Great work!";

			if (SceneManager.GetActiveScene ().buildIndex < 2) {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
			} else {
				SceneManager.LoadScene (0);
			}
		} else {
			slowingEnabled = true;

			//ball.GetComponent<MomentumCheck> ().checkEnabled = false;

			yield return new WaitForSeconds (2);

			Instantiate (explosionPrefab, other.gameObject.transform.position, Quaternion.Euler (new Vector3 (0, 0, 0)));

			gameObject.GetComponent<AudioSource> ().PlayOneShot (explosionSound);

			Destroy (other.gameObject);

			yield return new WaitForSeconds (3);

			Physics2D.gravity = new Vector2 (Physics2D.gravity.x, -9.81f);

			ResetScene (2);
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (slowingEnabled) {
			Physics2D.gravity = new Vector2 (Physics2D.gravity.x, other.GetComponent<Rigidbody2D> ().velocity.y * yGravityEffect);
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		Destroy (other.gameObject);
	}

	void Lerp () {
		overlay.GetComponent<CanvasGroup> ().alpha = Mathf.Lerp(overlay.GetComponent<CanvasGroup> ().alpha, 0, Time.deltaTime / lerpTime);
	}
}
