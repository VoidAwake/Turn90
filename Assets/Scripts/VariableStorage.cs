using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableStorage : MonoBehaviour {

	public int levelId;
	public string message;

	public AudioClip menuMusic;
	public AudioClip levelsMusic;
	public AudioClip infiniteMusic;

	// Prevent gameObject from being unloaded
	void Awake () {
		DontDestroyOnLoad (this);
	}
}
