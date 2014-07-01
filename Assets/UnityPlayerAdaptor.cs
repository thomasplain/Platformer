using GameEngineAdaptor;

namespace UnityGameEngineAdaptor
{
	using UnityEngine;

	class UnityPlayerAdaptor : IPlayerAdaptor
	{
		private GameObject player;
		private GameObject groundCheck;
		private GameObject projectilePrefab;

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

		private void fireProjectile(Vector3 startPosition, Vector3 fireForce)
		{
			GameObject projectile = (GameObject)GameObject.Instantiate(projectilePrefab, 
			                                                           startPosition,
			                                                           player.transform.rotation);
			projectile.rigidbody2D.AddForce(fireForce);
		}

		public void fireProjectileUp()
		{
			Vector3 projectilePosition = player.transform.position +
											Vector3.up * 1.25f;
											
			fireProjectile(projectilePosition, Vector3.up * 500f);
		}

		public void fireProjectileLeft()
		{
			Vector3 projectilePosition = player.transform.position +
											Vector3.left * 1.25f;

			fireProjectile(projectilePosition, Vector3.left * 500f);
		}

		public void fireProjectileRight()
		{
			Vector3 projectilePosition = player.transform.position +
											Vector3.right * 1.25f;
		
			fireProjectile(projectilePosition, Vector3.right * 500f);
		}

		public UnityPlayerAdaptor(GameObject playerArg, GameObject groundCheckArg, GameObject prefab)
		{
			player = playerArg;
			groundCheck = groundCheckArg;
			projectilePrefab = prefab;
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