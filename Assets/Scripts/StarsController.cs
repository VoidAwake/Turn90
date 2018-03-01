using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class StarsController : MonoBehaviour {

	public GameObject coinPrefab;
	public GameObject container;

	private int highscore;
	private int[] progress;
	private int[] secrets;

	void Awake () {
		loadData ();

		if (progress != null) {
			//Vector3 position;

			//foreach (int levelProgress in progress) {
			//	Transform parent = gameObject.transform.Find ("Level " + levelProgress + " Button");

			//	for (int i = 0; i < levelProgress; i++) {
			//		GameObject coinInstance = Instantiate (coinPrefab, parent.position, Quaternion.Euler(0, 0, 0), parent);
					//GameObject coinInstance = Instantiate (coinPrefab, new Vector3 (0.18f + 0.22f * i, i == 1 ? -2.23f : -2.16f, 0), Quaternion.Euler(0, 0, 0), container.transform);
			//		coinInstance.transform.localScale = new Vector3 (50, 50, 50);
			//	}
			//}
		}

		if (secrets != null) {
			foreach (int levelSecret in secrets) {
				
			}
		}
	}

	public void loadData () {
		if (File.Exists (Application.persistentDataPath + "/playerSave.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerSave.dat", FileMode.Open);
			PlayerSave data = (PlayerSave)bf.Deserialize (file);
			file.Close ();

			highscore = data.highscore;
			progress = data.progress;
			secrets = data.secrets;
		}
	}
}

[Serializable]
class PlayerSave {
	public int highscore;
	public int[] progress;
	public int[] secrets;
}
