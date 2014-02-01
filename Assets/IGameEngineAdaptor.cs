namespace GameEngineAdaptor
{
	interface IPlayerAdaptor
	{
		void jump(float force);
		void bigJump();
		void smallJump();
		void slowDescent();
		void fireProjectile();
		float getYVelocity();
		float getXVelocity();
	}

	interface ICollisionAdaptor
	{
		bool playerHasLandedOnGround();
		bool playerHasLandedOnEnemy();
		void destroyOtherObject();
	}
}