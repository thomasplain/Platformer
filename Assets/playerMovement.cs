using UnityEngine;
using System.Collections;

using playerStateMechanics;

public class playerMovement : MonoBehaviour {

	static private GameObject groundCheck;

	private State playerState;
	static private GameObject player;

	public float jumpForce = 1000f;
	public float projectileForce = 500f;
	public float maxSpeed = 15f;
	public GameObject projectilePrefab;

	void Awake()
	{
		groundCheck = GameObject.FindGameObjectWithTag ("groundCheck");
		player = GameObject.FindGameObjectWithTag ("Player");
		playerState = new Grounded (new StateConstructorArgs(player, groundCheck, jumpForce, projectilePrefab, projectileForce));
	}

	void Update()
	{
		State nextState = playerState.generalStateActions ();
		if (!nextState.isNull ())
			playerState = nextState;

		playerState = playerState.keypressActions ();

	}
	
	void FixedUpdate () {
		float h = Input.GetAxis ("Horizontal");

		playerState = playerState.stateActions ();

		Vector3 horizontalVelocity = Vector3.right * h * maxSpeed * Time.deltaTime; 

		transform.position += horizontalVelocity;	
	}
}
