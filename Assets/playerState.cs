using UnityEngine;
using System.Collections;
using GameEngineAdaptor;

namespace playerStateMechanics
{
	class StateConstructorArgs
	{
		public GameObject groundCheck;
		public GameObject player;
		public float jumpForce;
		public float smallJumpForce;
		public GameObject projectilePrefab;
		public float projectileForce;
		public IPlayerAdaptor playerAdaptor;

		public StateConstructorArgs(GameObject playerArg, GameObject groundCheckArg,
		                            float jumpForceArg, float smallJumpForceArg,
		                            GameObject projectileArg, float projForceArg,
		                            IPlayerAdaptor playerAdaptorArg)
		{
			player = playerArg;
			groundCheck = groundCheckArg;
			jumpForce = jumpForceArg;
			smallJumpForce = smallJumpForceArg;
			projectilePrefab = projectileArg;
			projectileForce = projForceArg;
			playerAdaptor = playerAdaptorArg;
		}
	}

	abstract class State
	{
		public virtual void printName(){}
		protected StateConstructorArgs playerInfo;

		public virtual bool isNull()
		{
			return false;
		}

		public virtual State keypressActions(){ return this; }
		public abstract State stateActions ();
		public virtual State collisionActions(ICollisionAdaptor collision)
		{ 
			if (collision.playerHasLandedOnEnemy())
			{
				collision.destroyOtherObject();
				return new StartJump(playerInfo);
			}

			if (collision.playerHasLandedOnGround())
			{
				return new Grounded(playerInfo);
			}

			return this;
		}

		public State generalStateActions ()
		{
			float horizontal = Input.GetAxis ("Horizontal");
			float vertical = Input.GetAxis ("Vertical");

			Vector3 relativeProjectileStartPosition = Vector3.zero;

			if (Input.GetButtonDown("Fire1"))
			{
				if (vertical < 0)
				{
					return new Stomping (playerInfo);
				}

				if (vertical > 0)
				{
					relativeProjectileStartPosition.y = 1.25f;
				}
				else
				{
					horizontal = horizontal >= 0 ? 1 : -1;
					relativeProjectileStartPosition.x = horizontal * 1.25f;
				}

				
				GameObject projectile = (GameObject)GameObject.Instantiate(playerInfo.projectilePrefab, 
				                                                           playerInfo.player.transform.position + relativeProjectileStartPosition,
				                                                           playerInfo.player.transform.rotation);
				projectile.rigidbody2D.AddForce(relativeProjectileStartPosition * playerInfo.projectileForce);
			}

			return new nullPlayerState (playerInfo);
		}
		
		public State(StateConstructorArgs constructorArgs)
		{
			playerInfo = constructorArgs;
		}
	}

	class Stomping : State
	{
		public Stomping(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("Stomping"); }

		public override State collisionActions (ICollisionAdaptor collision)
		{
			if (collision.playerHasLandedOnEnemy())
			{
				collision.destroyOtherObject();
				return new StartSmallJump(playerInfo);
			}

			if (collision.playerHasLandedOnGround())
			{
				return new Grounded(playerInfo);
			}

			return this;
		}
		
		public override State stateActions()
		{
			playerInfo.player.rigidbody2D.AddForce (Vector2.up * -1000f);
			return this;
		}
	}
}
