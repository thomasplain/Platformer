using UnityEngine;

namespace playerStateMechanics
{
	class Grounded : State
	{
		public Grounded(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("Grounded"); }
		
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
			if (!playerInfo.playerAdaptor.hasLandedOnGround())
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
