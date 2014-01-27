using UnityEngine;

namespace playerStateMechanics
{
	class StartJump : State
	{
		protected float jumpForceToUse;
		protected State nextState;
		public StartJump(StateConstructorArgs constructorArgs) : base(constructorArgs)
		{
			jumpForceToUse = playerInfo.jumpForce;
			nextState = new Jumping(playerInfo);
		}
		
		public override State stateActions ()
		{
			zeroPlayerVelocity ();
			playerInfo.player.rigidbody2D.AddForce (Vector2.up * jumpForceToUse);
			return nextState;
		}
		
	}

	class StartSmallJump : StartJump
	{
		public StartSmallJump(StateConstructorArgs constructorArgs) : base(constructorArgs)
		{
			jumpForceToUse = playerInfo.smallJumpForce;
		}
	}

	class Jumping : State
	{
		public Jumping(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("Jumping"); }

		public override void jumpActions()
		{
			zeroPlayerVelocity ();
			playerInfo.player.rigidbody2D.AddForce (Vector2.up * playerInfo.jumpForce);
		}

		public override State stateActions ()
		{
			if (Input.GetButtonDown ("Jump"))
			{
				jumpActions ();
				return new DoubleJumping (playerInfo);
			}
			else if (isOnDownwardArc())
			{
				return new FallingAfterFirstJump(playerInfo);
			}
			else
			{
				return this;
			}
		}
		
		protected bool isOnDownwardArc()
		{
			return playerInfo.player.rigidbody2D.velocity.y <= 0;
		}
	}

	class FallingAfterFirstJump : Jumping
	{
		public FallingAfterFirstJump(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("FallingAfterFirstJump"); }
		
		public override State stateActions()
		{
			if (Input.GetButtonDown ("Jump"))
			{
				jumpActions();
				return new DoubleJumping(playerInfo);
			}
			else if (Input.GetButton("Jump"))
			{
				return new FloatingAfterFirstJump(playerInfo);
			}
			else
			{
				return this;
			}
		}
	}

	class FloatingAfterFirstJump : State
	{
		public FloatingAfterFirstJump(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("FloatingAfterFirstJump"); }
		public override void jumpActions()
		{
			playerInfo.player.rigidbody2D.velocity = new Vector2(playerInfo.player.rigidbody2D.velocity.x, -5);
		}
		
		public override State stateActions()
		{
			if (Input.GetButtonUp ("Jump"))
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
			return new FallingAfterFirstJump(playerInfo);
		}
	}
}