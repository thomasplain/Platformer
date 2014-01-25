using UnityEngine;

namespace playerStateMechanics
{
	class DoubleJumping : Jumping
	{
		public DoubleJumping(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("DoubleJumping"); }
		public override void jumpActions()
		{
			// On downward arc
			if (playerInfo.player.rigidbody2D.velocity.y <= 0)
			{
				playerInfo.player.rigidbody2D.velocity = new Vector2(playerInfo.player.rigidbody2D.velocity.x, -5);
			}
		}
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