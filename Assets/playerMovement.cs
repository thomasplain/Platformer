﻿using UnityEngine;
using System.Collections;


abstract class State
{
	public abstract void printName();
	protected GameObject player = playerMovement.getPlayer();
	protected GameObject groundCheck = playerMovement.getGroundCheck();
	public abstract void jumpActions ();
	public abstract State stateActions ();
	
	protected bool checkForGround()
	{
		int checkIndex = -1;
		float playerSizeX = player.renderer.bounds.size.x;
		bool grounded = false;
		while (checkIndex < 2)
		{
			Vector3 offset = Vector3.right * playerSizeX * checkIndex / 2f;
			Vector3 fromPoint = player.transform.position + offset;
			Vector3 toPoint = groundCheck.transform.position + offset;
			grounded = grounded || Physics2D.Linecast (fromPoint, toPoint, 1 << LayerMask.NameToLayer("ground"));
			checkIndex++;
		}
		
		return grounded;
	}
}

class Grounded : State
{
	public override void printName() { Debug.Log ("Grounded"); }
	public override void jumpActions()
	{
		player.rigidbody2D.AddForce (Vector2.up * playerMovement._jumpForce);
	}
	
	public override State stateActions()
	{
		if (!checkForGround())
		{
			return new FallingBeforeSecondJump();
		}
		else if (Input.GetButtonDown ("Jump"))
		{
			jumpActions();
			return new Jumping();
		}
		else
		{
			return this;
		}
	}
}

class Jumping : State
{
	public override void printName() { Debug.Log ("Jumping"); }
	public override void jumpActions()
	{
		zeroPlayerVelocity ();
		player.rigidbody2D.AddForce (Vector2.up * 1000f);
	}
	public override State stateActions ()
	{
		if (checkForGround ())
		{
				return new Grounded ();
		}
		else if (Input.GetButtonDown ("Jump"))
		{
				jumpActions ();
				return new DoubleJumping ();
		}
		else if (isOnDownwardArc())
		{
			return new FallingBeforeSecondJump();
		}
		else
		{
				return this;
		}
	}
	public void zeroPlayerVelocity()
	{
		player.rigidbody2D.velocity = new Vector2 (player.rigidbody2D.velocity.x, 0);
	}

	protected bool isOnDownwardArc()
	{
		return player.rigidbody2D.velocity.y <= 0;
	}
}

class FallingBeforeSecondJump : Jumping
{
	public override void printName() { Debug.Log ("FallingBeforeSecondJump"); }
	
	public override State stateActions()
	{
		if (checkForGround())
		{
			return new Grounded();
		}
		else if (Input.GetButtonDown ("Jump"))
		{
			jumpActions();
			return new DoubleJumping();
		}
		else if (Input.GetButton("Jump"))
		{
			return new FloatingBeforeSecondJump();
		}
		else
		{
			return this;
		}
	}
}

class FloatingBeforeSecondJump : State
{
	public override void printName() { Debug.Log ("FloatingBeforeSecondJump"); }
	public override void jumpActions()
	{
		player.rigidbody2D.velocity = new Vector2(player.rigidbody2D.velocity.x, -5);
	}
	
	public override State stateActions()
	{
		if (checkForGround())
		{
			return new Grounded();
		}
		else if (Input.GetButtonUp ("Jump"))
		{
			return new FallingBeforeSecondJump();
		}
		else
		{
			jumpActions ();
			return this;
		}
	}
}

class DoubleJumping : Jumping
{
	public override void printName() { Debug.Log ("DoubleJumping"); }
	public override void jumpActions()
	{
		// On downward arc
		if (player.rigidbody2D.velocity.y <= 0)
		{
			player.rigidbody2D.velocity = new Vector2(player.rigidbody2D.velocity.x, -5);
		}
	}
	public override State stateActions ()
	{
		if (checkForGround())
		{
			return new Grounded();
		}
		else if (Input.GetButton ("Jump"))
		{
			jumpActions ();
			return this;
		}
		else
		{
			return this;
		}
	}
}
class FallingAfterSecondJump : Jumping
{
	public override void printName() { Debug.Log ("FallingBeforeSecondJump"); }
	
	public override State stateActions()
	{
		if (checkForGround())
		{
			return new Grounded();
		}
		else if (Input.GetButton("Jump"))
		{
			return new FloatingAfterSecondJump();
		}
		else
		{
			return this;
		}
	}
}

class FloatingAfterSecondJump : State
{
	public override void printName() { Debug.Log ("FloatingBeforeSecondJump"); }
	public override void jumpActions()
	{
		player.rigidbody2D.velocity = new Vector2(player.rigidbody2D.velocity.x, -5);
	}
	
	public override State stateActions()
	{
		if (checkForGround())
		{
			return new Grounded();
		}
		else if (Input.GetButtonUp ("Jump"))
		{
			return new FallingAfterSecondJump();
		}
		else
		{
			jumpActions ();
			return this;
		}
	}
}

public class playerMovement : MonoBehaviour {

	static private GameObject groundCheck;

	private State playerState;
	static private GameObject player;

	static public float _jumpForce;
	public float jumpForce = 1000f;
	public float maxSpeed = 15f;

	void Awake()
	{
		groundCheck = GameObject.FindGameObjectWithTag ("groundCheck");
		player = GameObject.FindGameObjectWithTag ("Player");
		_jumpForce = jumpForce;
		playerState = new Grounded ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		float h = Input.GetAxis ("Horizontal");

		playerState = playerState.stateActions ();

		Vector3 horizontalVelocity = Vector3.right * h * maxSpeed * Time.deltaTime; 

		transform.position += horizontalVelocity;	
	}

	static public GameObject getPlayer()
	{
		return player;
	}

	static public GameObject getGroundCheck()
	{
		return groundCheck;
	}
}
