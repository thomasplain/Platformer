using UnityEngine;

namespace playerStateMechanics
{
	class StartJump : State
	{
		protected State nextState;
		public StartJump(StateConstructorArgs constructorArgs) : base(constructorArgs)
		{
			nextState = new Jumping(playerInfo);
		}
		
		public override State stateActions ()
		{
			playerInfo.playerAdaptor.bigJump();
			return nextState;
		}
		
	}

	class StartSmallJump : StartJump
	{
		public StartSmallJump(StateConstructorArgs constructorArgs) : base(constructorArgs){}

		public override State stateActions()
		{
			playerInfo.playerAdaptor.smallJump();
			return nextState;
		}
	}

	class Jumping : State
	{
		public Jumping(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("Jumping"); }

		public override State stateActions ()
		{
			if (Input.GetButtonDown ("Jump"))
			{
				playerInfo.playerAdaptor.bigJump();
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
			return playerInfo.playerAdaptor.getYVelocity() <= 0;
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
				playerInfo.playerAdaptor.bigJump();
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
		
		public override State stateActions()
		{
			if (Input.GetButtonUp ("Jump"))
			{
				return getFallingState();
			}
			else
			{
				playerInfo.playerAdaptor.slowDescent();
				return this;
			}
		}
		
		protected virtual State getFallingState()
		{
			return new FallingAfterFirstJump(playerInfo);
		}
	}
}