using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class InGameSettingToggles : MonoBehaviour {

	public GameObject soundButton;
	public GameObject musicButton;

	public bool sound;
	public bool music;

	private GameObject storageObject;

	public void initiateStorage (GameObject storage) {
		storageObject = storage;

		loadSettings ();
	}

	public void toggleSound () {
		sound = sound ? false :	true;

		saveSettings ();
	}

	public void toggleMusic () {
		music = music ? false : true;

		gameObject.GetComponent <GameController> ().toggleMusic (music);

		saveSettings ();
	}

	public void saveSettings () {
		if (storageObject != null) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/playerData.dat");

			PlayerData data = new PlayerData ();
			data.music = music;
			data.sound = sound;

			bf.Serialize (file, data);
			file.Close ();

			soundButton.transform.GetChild (0).GetComponent<Image> ().color = sound ? new Color32 (0x00, 0xFF, 0x00, 0xFF) : new Color32 (0x21, 0x59, 0x24, 0xFF);
			musicButton.transform.GetChild (0).GetComponent<Image> ().color = music ? new Color32 (0x00, 0xFF, 0x00, 0xFF) : new Color32 (0x21, 0x59, 0x24, 0xFF);
		} else {
			Debug.Log ("Settings failed to save!");
		}
	}

	public void loadSettings () {
		if (File.Exists (Application.persistentDataPath + "/playerData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerData.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();

			music = data.music;
			sound = data.sound;

			saveSettings ();
		}
	}
}

// PlayerData class is defined in SettingToggles.cs