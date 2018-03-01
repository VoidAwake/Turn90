using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SettingToggles : MonoBehaviour {

	public GameObject soundButton;
	public GameObject musicButton;

	private bool sound;
	public bool music;

	void Awake () {
		loadSettings ();
	}

	public void toggleSound () {
		sound = sound ? false :	true;

		saveSettings ();
	}

	public void toggleMusic () {
		music = music ? false : true;

		gameObject.GetComponent <MenuRedirector> ().toggleMusic (music);

		saveSettings ();
	}

	public void resetAll () {
		music = true;

		sound = true;

		saveSettings ();

		if (File.Exists (Application.persistentDataPath + "/playerSave.dat")) {
			File.Delete (Application.persistentDataPath + "/playerSave.dat");
		}

		SceneManager.LoadScene (0);
	}

	public void saveSettings () {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerData.dat");

		PlayerData data = new PlayerData ();
		data.music = music;
		data.sound = sound;

		bf.Serialize (file, data);
		file.Close ();

		soundButton.GetComponent<Text> ().text = sound ? "SOUND ON" : "SOUND OFF";
		musicButton.GetComponent<Text> ().text = music ? "MUSIC ON" : "MUSIC OFF";
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
		} else {
			music = true;
			sound = true;

			saveSettings ();
		}
	}
}

[Serializable]
class PlayerData {
	public bool music;
	public bool sound;
}
