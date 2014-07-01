using UnityEngine;

namespace playerStateMechanics
{
	class DoubleJumping : Jumping
	{
		public DoubleJumping(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("DoubleJumping"); }

		public override State stateActions ()
		{
			if (isOnDownwardArc())
			{
				return new FallingAfterSecondJump(playerInfo);
			}
			else
			{
				return this;
			}
		}
	}
	class FallingAfterSecondJump : Jumping
	{
		public FallingAfterSecondJump(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("FallingAfterFirstJump"); }
		
		public override State stateActions()
		{
			if (Input.GetButton("Jump"))
			{
				return new FloatingAfterSecondJump(playerInfo);
			}
			else
			{
				return this;
			}
		}
	}

	class FloatingAfterSecondJump : FloatingAfterFirstJump
	{
		public FloatingAfterSecondJump(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("FloatingAfterFirstJump"); }
		
		protected override State getFallingState()
		{
			return new FallingAfterSecondJump (playerInfo);
		}
	}
}