using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {

	// Control variables
	private int levelId;

	// Overlay variables
	[Header("Overlay")]
	public GameObject overlay;
	public GameObject overlayNumberObject;
	public GameObject overlayImageObject;
	public GameObject overlayMessageObject;

	// Scoring variables
	[Header("Scoring UI")]
	public Text scoreText; // Consider changing this to a GUI event
	public static int score;
	public static int coinAmount;
	private bool newHighscore;

	// Effects variables
	[Header("Effects")]
	public GameObject explosionPrefab;
	public AudioClip explosionSound;
	public AudioClip levelCompleteSound;
	public AudioClip pickupSound;
	private GameObject explosionInstance;

	// Variable storage
	[Header("Variable Storage")]
	public GameObject variableStorageObject;
	public VariableStorage variableStorageScript;

	// Animation variables
	[Header("Animations")]
	public float timeToFirstCheck;
	public GameObject ball;
	public float timeToLerp;
	public float lerpTime;
	public float yGravityEffect;

	public float timeToDeath;
	public float timeToReset;

	private bool lerpEnabled = false;
	private bool slowingEnabled = false;

	// Save variables
	private int highscore;
	private int[] progress;
	private int[] secrets;

	// Tutorial variables
	[Header("Tutorial")]
	public GameObject tutorialContainer;

	// Infinite variables
	[Header("Infinite UI")]
	public GameObject topPanel;

	IEnumerator Start () {
		// Find variable storage and get levelId
		variableStorageObject = GameObject.FindGameObjectWithTag ("Storage");

		if (variableStorageObject == null) {
			Debug.Log ("Can't find variable storage object! Redirecting back to main menu...");

			SceneManager.LoadScene (0);
		} else {
			gameObject.GetComponent<InGameSettingToggles> ().initiateStorage (variableStorageObject);

			variableStorageScript = variableStorageObject.GetComponent<VariableStorage> ();

			levelId = variableStorageScript.levelId;
			overlayMessageObject.GetComponent <Text> ().text = variableStorageScript.message;
			variableStorageScript.message = "";

			// Play music
			if (variableStorageObject.GetComponent <AudioSource> ().clip != (levelId == 0 ? variableStorageScript.infiniteMusic : variableStorageScript.levelsMusic)) {
				variableStorageObject.GetComponent <AudioSource> ().clip = levelId == 0 ? variableStorageScript.infiniteMusic : variableStorageScript.levelsMusic;
			}

			toggleMusic (gameObject.GetComponent <InGameSettingToggles> ().music);

			// Find ball gameobject
			ball = GameObject.FindGameObjectWithTag ("Player");

			// Determine the operating gamemode and run appropriate generation
			if (levelId == 0) {
				// Start generating modules
				gameObject.GetComponent<InfiniteGeneration> ().enabled = true;

				// Enable camera tracking and pass ball reference
				gameObject.GetComponent<FollowBall> ().enabled = true;
				gameObject.GetComponent<FollowBall> ().ball = ball;

				overlayNumberObject.SetActive (false);

				overlayImageObject.SetActive (true);

				gameObject.GetComponent<Collider2D> ().offset = new Vector2(0,-0.5f);

				topPanel.SetActive (true);
			} else {
				// Start generating level
				gameObject.GetComponent<LevelsGeneration> ().enabled = true;

				int[] returnData = gameObject.GetComponent<LevelsGeneration> ().generateLevel (levelId - 1);

				coinAmount = returnData [0];
				ball.transform.position = new Vector3 (returnData[1], 2, 1);

				overlayNumberObject.GetComponent<Text> ().text = levelId.ToString ();
			}

			loadData ();

			// Initialise score
			score = 0;
			UpdateScore ();

			Physics2D.gravity = new Vector2 (0, -9.81f);

			// Freeze ball and wait to enable momentum check
			ball.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
			yield return new WaitForSeconds (timeToLerp);
			lerpEnabled = true;
			ball.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
			yield return new WaitForSeconds (timeToFirstCheck);
			ball.GetComponent<MomentumCheck> ().enabled = true;
		}
	}
		
	void Update () {
		if (lerpEnabled) {
			overlay.GetComponent<CanvasGroup> ().alpha = Mathf.Lerp(overlay.GetComponent<CanvasGroup> ().alpha, 0, Time.deltaTime / lerpTime);
		}

		// Increase gravity
		Physics2D.gravity = new Vector2 (Physics2D.gravity.x, Physics2D.gravity.y - 0.01f);
	}

	public void ResetSceneTest () {
		SceneManager.LoadScene(1);
	}
		
	public void ResetScene (int resetCondition)  {
		switch (resetCondition) {
			case 0:
				variableStorageScript.message = "LEVEL RESET";
				break;
			case 1:
				variableStorageScript.message = "KEEP THE BALL MOVING";
				break;
			case 2:
				variableStorageScript.message = "GET ALL THE COINS";
				break;
		}

		if (levelId == 0) {
			saveData ();
		}

		SceneManager.LoadScene(1);
	}

	public void NextLevel ()  {
		if (levelId > 0 && levelId < 6) {
			variableStorageScript.message = "WELL DONE!";

			variableStorageScript.levelId++;


		} else {
			variableStorageScript.message = "";

			variableStorageScript.levelId = 0;
		}

		SceneManager.LoadScene (1);
	}
		
	IEnumerator OnTriggerEnter2D (Collider2D other) {
		if (levelId == 0) {
			if (levelId == 0) {
				gameObject.GetComponent<InfiniteGeneration> ().generateModule (false, -1);

				gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (0, gameObject.GetComponent<BoxCollider2D> ().offset.y - 3);
			}
		} else {
			if (score == coinAmount) {

				yield return new WaitForSeconds (0.5f);

				playSound (2);

				yield return new WaitForSeconds (2f);

				variableStorageScript.message = "Great work!";

				saveData ();

				NextLevel ();
			} else {
				slowingEnabled = true;

				ball.GetComponent<MomentumCheck> ().enabled = false;

				yield return new WaitForSeconds (timeToDeath);

				explosionInstance = Instantiate (explosionPrefab, other.gameObject.transform.position, other.gameObject.transform.rotation);

				explosionInstance.GetComponent<Rigidbody2D> ().angularVelocity = other.gameObject.GetComponent<Rigidbody2D> ().angularVelocity;

				playSound (1);

				Destroy (other.gameObject);

				yield return new WaitForSeconds (timeToReset);

				Physics2D.gravity = new Vector2 (Physics2D.gravity.x, -9.81f);

				saveData ();

				ResetScene (2);
			}
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (slowingEnabled) {
			Physics2D.gravity = new Vector2 (Physics2D.gravity.x, other.GetComponent<Rigidbody2D> ().velocity.y * yGravityEffect);
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (levelId != 0) {
			Destroy (other.gameObject);
		}
	}

	public void AddScore () {
		score += 1;
		UpdateScore ();
		playSound (0);

		// Increase gravity
		Physics2D.gravity = new Vector2 (Physics2D.gravity.x, Physics2D.gravity.y + 1);
	}

	void UpdateScore () {
		if (levelId == 0) {
			scoreText.text = score.ToString ();

			if (!newHighscore) {
				if (score > highscore) {
					scoreText.color = Color.yellow;

					newHighscore = true;
				}
			}
		} else {
			scoreText.text = "";
		}
	}

	void playSound (int soundId) {
		if (gameObject.GetComponent<InGameSettingToggles> ().sound) {
			switch (soundId) {
			case 0:
				gameObject.GetComponent<AudioSource> ().PlayOneShot (pickupSound);
				break;
			case 1:
				gameObject.GetComponent<AudioSource> ().PlayOneShot (explosionSound);
				break;
			case 2:
				gameObject.GetComponent<AudioSource> ().PlayOneShot (levelCompleteSound);
				break;
			}
		}
	}

	void loadData() {
		if (File.Exists (Application.persistentDataPath + "/playerSave.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerSave.dat", FileMode.Open);
			PlayerSave data = (PlayerSave)bf.Deserialize (file);
			file.Close ();

			highscore = data.highscore;
			progress = data.progress;
			secrets = data.secrets;
		} else {
			highscore = 0;
			progress = new int[] {0,0,0,0,0,0};
			secrets = new int[] {0,0,0,0,0,0};

			saveData ();

			if (levelId == 1) {

				Instantiate (tutorialContainer);
			}
		}
	}

	void saveData () {
		if (levelId == 0) {
			if (score > highscore) {
				highscore = score;
			}
		} else {
			//var scoreRatio = (float)score / coinAmount;

			//if (scoreRatio == 0) {
			//	progress [levelId] = 0;
			//} else if (scoreRatio > 0 && scoreRatio < 0.5) {
			//	progress [levelId] = 1;
			//} else if (scoreRatio >= 0.5 && scoreRatio < 1) {
			//	progress [levelId] = 2;
			//} else if (scoreRatio == 1) {
			//	progress [levelId] = 3;
			//} else {
			//	Debug.Log ("An error occured!");
			//}
		}

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerSave.dat");

		PlayerSave data = new PlayerSave ();
		data.highscore = highscore;
		data.progress = progress;
		data.secrets = secrets;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void toggleMusic (bool music) {
		if (music) {
			if (!variableStorageObject.GetComponent <AudioSource> ().isPlaying) {
				variableStorageObject.GetComponent <AudioSource> ().Play ();
			}
		} else {
			variableStorageObject.GetComponent <AudioSource> ().Stop ();
		}
	}
}

public class Coin
{
	public Vector2 position;

	public Coin (float xInput, float yInput) {
		position = new Vector2 (xInput, yInput);
	}
}