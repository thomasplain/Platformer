using UnityEngine;
using System.Collections;

namespace playerStateMechanics
{
	abstract class State
	{
		public abstract void printName();
		protected GameObject player;
		protected GameObject groundCheck;
		protected float jumpForce;
		public abstract void jumpActions ();
		public abstract State stateActions ();
		
		public State(GameObject playerReference, GameObject groundCheckReference, float playerJumpForce)
		{
			player = playerReference;
			groundCheck = groundCheckReference;
			jumpForce = playerJumpForce;
		}
		
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
		public Grounded(GameObject playerReference, GameObject groundCheckReference, float playerJumpForce) : 
			base(playerReference, groundCheckReference, playerJumpForce) {}
		
		public override void printName() { Debug.Log ("Grounded"); }
		public override void jumpActions()
		{
			player.rigidbody2D.AddForce (Vector2.up * jumpForce);
		}
		
		public override State stateActions()
		{
			if (!checkForGround())
			{
				return new FallingBeforeSecondJump(player, groundCheck, jumpForce);
			}
			else if (Input.GetButtonDown ("Jump"))
			{
				jumpActions();
				return new Jumping(player, groundCheck, jumpForce);
			}
			else
			{
				return this;
			}
		}
	}
	
	class Jumping : State
	{
		public Jumping(GameObject playerReference, GameObject groundCheckReference, float playerJumpForce) : 
			base(playerReference, groundCheckReference, playerJumpForce) {}
		
		public override void printName() { Debug.Log ("Jumping"); }
		public override void jumpActions()
		{
			zeroPlayerVelocity ();
			player.rigidbody2D.AddForce (Vector2.up * jumpForce);
		}
		public override State stateActions ()
		{
			if (checkForGround ())
			{
				return new Grounded (player, groundCheck, jumpForce);
			}
			else if (Input.GetButtonDown ("Jump"))
			{
				jumpActions ();
				return new DoubleJumping (player, groundCheck, jumpForce);
			}
			else if (isOnDownwardArc())
			{
				return new FallingBeforeSecondJump(player, groundCheck, jumpForce);
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
		public FallingBeforeSecondJump(GameObject playerReference, GameObject groundCheckReference, float playerJumpForce) : 
			base(playerReference, groundCheckReference, playerJumpForce) {}
		
		public override void printName() { Debug.Log ("FallingBeforeSecondJump"); }
		
		public override State stateActions()
		{
			if (checkForGround())
			{
				return new Grounded(player, groundCheck, jumpForce);
			}
			else if (Input.GetButtonDown ("Jump"))
			{
				jumpActions();
				return new DoubleJumping(player, groundCheck, jumpForce);
			}
			else if (Input.GetButton("Jump"))
			{
				return new FloatingBeforeSecondJump(player, groundCheck, jumpForce);
			}
			else
			{
				return this;
			}
		}
	}
	
	class FloatingBeforeSecondJump : State
	{
		public FloatingBeforeSecondJump(GameObject playerReference, GameObject groundCheckReference, float playerJumpForce) : 
			base(playerReference, groundCheckReference, playerJumpForce) {}
		
		public override void printName() { Debug.Log ("FloatingBeforeSecondJump"); }
		public override void jumpActions()
		{
			player.rigidbody2D.velocity = new Vector2(player.rigidbody2D.velocity.x, -5);
		}
		
		public override State stateActions()
		{
			if (checkForGround())
			{
				return new Grounded(player, groundCheck, jumpForce);
			}
			else if (Input.GetButtonUp ("Jump"))
			{
				return getFallingState();
			}
			else
			{
				jumpActions ();
				return this;
			}
		}

		protected virtual State getFallingState()
		{
			return new FallingBeforeSecondJump(player, groundCheck, jumpForce);
		}
	}
	
	class DoubleJumping : Jumping
	{
		public DoubleJumping(GameObject playerReference, GameObject groundCheckReference, float playerJumpForce) : 
			base(playerReference, groundCheckReference, playerJumpForce) {}
		
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
				return new Grounded(player, groundCheck, jumpForce);
			}
			else if (isOnDownwardArc())
			{
				return new FallingAfterSecondJump(player, groundCheck, jumpForce);
			}
			else
			{
				return this;
			}
		}
	}
	class FallingAfterSecondJump : Jumping
	{
		public FallingAfterSecondJump(GameObject playerReference, GameObject groundCheckReference, float playerJumpForce) : 
			base(playerReference, groundCheckReference, playerJumpForce) {}
		
		public override void printName() { Debug.Log ("FallingBeforeSecondJump"); }
		
		public override State stateActions()
		{
			if (checkForGround())
			{
				return new Grounded(player, groundCheck, jumpForce);
			}
			else if (Input.GetButton("Jump"))
			{
				return new FloatingAfterSecondJump(player, groundCheck, jumpForce);
			}
			else
			{
				return this;
			}
		}
	}
	
	class FloatingAfterSecondJump : FloatingBeforeSecondJump
	{
		public FloatingAfterSecondJump(GameObject playerReference, GameObject groundCheckReference, float playerJumpForce) : 
			base(playerReference, groundCheckReference, playerJumpForce) {}
		
		public override void printName() { Debug.Log ("FloatingBeforeSecondJump"); }
		
		protected override State getFallingState()
		{
			return new FallingAfterSecondJump (player, groundCheck, jumpForce);
		}
	}
}
