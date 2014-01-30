using UnityEngine;
using System.Collections;

using playerStateMechanics;
using UnityGameEngineAdaptor;

public class playerMovement : MonoBehaviour {

	static private GameObject groundCheck;

	private State playerState;
	static private GameObject player;

	public float jumpForce = 1000f;
	public float smallJumpForce = 300f;
	public float projectileForce = 500f;
	public float maxSpeed = 15f;
	public GameObject projectilePrefab;

	private UnityPlayerAdaptor playerAdaptor;

	void Awake()
	{
		groundCheck = GameObject.FindGameObjectWithTag ("groundCheck");
		player = GameObject.FindGameObjectWithTag ("Player");
		playerAdaptor = new UnityPlayerAdaptor(player);
		playerState = new Grounded (new StateConstructorArgs(player, groundCheck, 
															jumpForce, smallJumpForce,
															projectilePrefab, projectileForce,
															playerAdaptor));
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

	void OnCollisionEnter2D(Collision2D collision)
	{
		playerState = playerState.collisionActions (collision);
	}
}
