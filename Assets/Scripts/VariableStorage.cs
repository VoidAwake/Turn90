using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gives a place to store variables that will persist between levels
public class VariableStorage : MonoBehaviour {

	public string message;

	// Prevents the gameObject from being unloaded when the scene changes
	void Awake () {
		DontDestroyOnLoad (this);
	}
}
