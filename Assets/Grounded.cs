using UnityEngine;

namespace playerStateMechanics
{
	class Grounded : State
	{
		public Grounded(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("Grounded"); }
		public override void jumpActions()
		{
			playerInfo.player.rigidbody2D.AddForce (Vector2.up * playerInfo.jumpForce);
		}

		public override State keypressActions()
		{
			if (Input.GetButtonDown ("Jump"))
			{
				return new StartJump(playerInfo);
			}

			return this;
		}
		
		public override State stateActions()
		{
			if (!checkForGround())
			{
				return new FallingAfterFirstJump(playerInfo);
			}
			else
			{
				return this;
			}
		}
	}
}
