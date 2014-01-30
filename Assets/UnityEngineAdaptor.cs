using GameEngineAdaptor;

namespace UnityGameEngineAdaptor
{
	using UnityEngine;

	class UnityPlayerAdaptor : IPlayerAdaptor
	{
		public GameObject player;

		public void jump(float force)
		{
			zeroPlayerVelocity ();
			player.rigidbody2D.AddForce (Vector2.up * force);
		}

		public void bigJump()
		{
			jump(1000f);
		}

		public void smallJump()
		{
			jump(300f);
		}

		public void slowDescent()
		{
			player.rigidbody2D.velocity = new Vector2(player.rigidbody2D.velocity.x, -5);
		}

		public void fireProjectile()
		{

		}

		public bool hasLandedOnGround()
		{
			return true;
		}

		public UnityPlayerAdaptor(GameObject playerArg)
		{
			player = playerArg;
		}
		
		public float getYVelocity()
		{
			return player.rigidbody2D.velocity.y;
		}
		
		public float getXVelocity()
		{
			return player.rigidbody2D.velocity.x;
		}

		private void zeroPlayerVelocity()
		{
			player.rigidbody2D.velocity = new Vector2 (player.rigidbody2D.velocity.x, 0);
		}
	}
}