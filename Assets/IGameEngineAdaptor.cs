namespace GameEngineAdaptor
{
	interface IPlayerAdaptor
	{
		void jump(float force);
		void bigJump();
		void smallJump();
		void slowDescent();
		void fireProjectile();
		bool hasLandedOnGround();
		float getYVelocity();
		float getXVelocity();
	}
}