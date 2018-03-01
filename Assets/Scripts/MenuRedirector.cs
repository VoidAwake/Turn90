using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuRedirector : MonoBehaviour {

	public GameObject variableStoragePrefab;

	private GameObject variableStorageObject;
	private VariableStorage variableStorageScript;

	void Start () {
		variableStorageObject = GameObject.FindGameObjectWithTag ("Storage");

		if (variableStorageObject == null) {
			variableStorageObject = Instantiate (variableStoragePrefab);
		}

		variableStorageScript = variableStorageObject.GetComponent <VariableStorage> ();

		// Play music
		if (variableStorageObject.GetComponent <AudioSource> ().clip != variableStorageScript.menuMusic) {
			variableStorageObject.GetComponent <AudioSource> ().clip = variableStorageScript.menuMusic;
		}

		toggleMusic (gameObject.GetComponent <SettingToggles> ().music);
	}

	public void changeScene (int levelId) {
		variableStorageObject.GetComponent<VariableStorage> ().levelId = levelId;

		SceneManager.LoadScene (1);
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
