using UnityEngine;
using System.Collections;

using playerStateMechanics;

public class playerMovement : MonoBehaviour {

	static private GameObject groundCheck;

	private State playerState;
	static private GameObject player;

	public float jumpForce = 1000f;
	public float maxSpeed = 15f;

	void Awake()
	{
		groundCheck = GameObject.FindGameObjectWithTag ("groundCheck");
		player = GameObject.FindGameObjectWithTag ("Player");
		playerState = new Grounded (player, groundCheck, jumpForce);
	}

	// Update is called once per frame
	void FixedUpdate () {
		float h = Input.GetAxis ("Horizontal");

		playerState = playerState.stateActions ();

		Vector3 horizontalVelocity = Vector3.right * h * maxSpeed * Time.deltaTime; 

		transform.position += horizontalVelocity;	
	}
}
