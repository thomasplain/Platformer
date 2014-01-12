using UnityEngine;

namespace playerStateMechanics
{
	class nullPlayerState : State
	{
		public nullPlayerState(GameObject playerReference, GameObject groundCheckReference, float playerJumpForce) :
		base(playerReference, groundCheckReference, playerJumpForce) {}

		public override void printName() { Debug.Log ("Null"); }
		public override void jumpActions ()
		{
		}

		public override State stateActions ()
		{
			return this;
		}

		public override bool isNull()
		{
			return true;
		}
	}
}
