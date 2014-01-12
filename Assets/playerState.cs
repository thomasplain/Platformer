using UnityEngine;
using System.Collections;

namespace playerStateMechanics
{
	abstract class State
	{
		public abstract void printName();
		protected GameObject player;
		protected GameObject groundCheck;
		public abstract void jumpActions ();
		public abstract State stateActions ();

		State(GameObject playerReference, GameObject groundCheckReference)
		{
			player = playerReference;
			groundCheck = groundCheckReference;
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
}
