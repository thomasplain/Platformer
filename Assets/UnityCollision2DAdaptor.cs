using GameEngineAdaptor;

namespace UnityGameEngineAdaptor
{
	using UnityEngine;

	class UnityCollision2DAdaptor : ICollisionAdaptor
	{
		private Collision2D collision;

		public bool playerHasLandedOnGround()
		{
			if (collision.collider.gameObject.tag != "Floor")
								return false;

			foreach (ContactPoint2D contactPoint in collision.contacts)
			{
				if (contactPoint.normal == Vector2.up)
					return true;
			}

			return false;
		}

		public bool playerHasLandedOnEnemy()
		{
			if (collision.collider.gameObject.tag != "enemy")
								return false;

			foreach (ContactPoint2D contactPoint in collision.contacts)
			{
				if (contactPoint.normal == Vector2.up)
					return true;
			}

			return false;
		}

		public void destroyOtherObject()
		{

			GameObject.Destroy(collision.collider.gameObject);
		}

		public UnityCollision2DAdaptor(Collision2D collisionInstance)
		{
			collision = collisionInstance;
		}
	}
}