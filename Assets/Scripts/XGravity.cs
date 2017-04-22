using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Negates rolling friction through the implementation of gravity on the x-axis in proportion to the ball's speed
public class XGravity : MonoBehaviour {

	public float xGravityEffect;

	// Update is called once per frame
	void Update () {
		Physics2D.gravity = new Vector2 (gameObject.GetComponent<Rigidbody2D>().velocity.x * xGravityEffect, Physics2D.gravity.y);
	}
}
