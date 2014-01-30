using UnityEngine;

namespace playerStateMechanics
{
	class nullPlayerState : State
	{
		public nullPlayerState(StateConstructorArgs constructorArgs) : base(constructorArgs) {}

		public override void printName() { Debug.Log ("Null"); }

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
