namespace GameEngineAdaptor
{
	interface IPlayerAdaptor
	{
		void jump(float force);
		void bigJump();
		void smallJump();
		void slowDescent();
		void fireProjectileUp();
		void fireProjectileLeft();
		void fireProjectileRight();
		float getYVelocity();
		float getXVelocity();
	}

	interface ICollisionAdaptor
	{
		bool playerHasLandedOnGround();
		bool playerHasLandedOnEnemy();
		void destroyOtherObject();

		bool otherObjectIsGround();
	}
}