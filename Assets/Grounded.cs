using UnityEngine;
using GameEngineAdaptor;

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
			return this;
		}

		public override State collisionExitActions(ICollisionAdaptor collision)
		{
			if (collision.otherObjectIsGround())
			{
				return new FallingAfterFirstJump(playerInfo);
			}

			return this;
		}
	}
}
