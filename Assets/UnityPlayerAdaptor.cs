using GameEngineAdaptor;

namespace UnityGameEngineAdaptor
{
	using UnityEngine;

	class UnityPlayerAdaptor : IPlayerAdaptor
	{
		private GameObject player;
		private GameObject groundCheck;

		public void jump(float force)
		{
			zeroPlayerYVelocity ();
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

		public UnityPlayerAdaptor(GameObject playerArg, GameObject groundCheckArg)
		{
			player = playerArg;
			groundCheck = groundCheckArg;
		}
		
		public float getYVelocity()
		{
			return player.rigidbody2D.velocity.y;
		}
		
		public float getXVelocity()
		{
			return player.rigidbody2D.velocity.x;
		}

		private void zeroPlayerYVelocity()
		{
			player.rigidbody2D.velocity = new Vector2 (player.rigidbody2D.velocity.x, 0);
		}
	}
}