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
		public virtual State collisionActions(Collision2D collision)
		{ 
			if (hittingEnemyFromAbove(collision))
			{
				GameObject.Destroy(collision.collider.gameObject);
				return new StartJump(playerInfo);
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
				
			if (checkForGround ())
			{
					return new Grounded (playerInfo);
			}


			return new nullPlayerState (playerInfo);
		}
		
		public State(StateConstructorArgs constructorArgs)
		{
			playerInfo = constructorArgs;
		}
		
		protected bool checkForGround()
		{
			int checkIndex = -1;
			float playerSizeX = playerInfo.player.renderer.bounds.size.x;
			while (checkIndex < 2)
			{
				Vector3 offset = Vector3.right * playerSizeX * checkIndex / 2f;
				Vector3 fromPoint = playerInfo.player.transform.position + offset;
				Vector3 toPoint = playerInfo.groundCheck.transform.position + offset;

				if (Physics2D.Linecast (fromPoint, toPoint, 1 << LayerMask.NameToLayer("ground")))
					return true;
				else
					checkIndex++;
			}

			return false;
		}
		
		public void zeroPlayerVelocity()
		{
			playerInfo.player.rigidbody2D.velocity = new Vector2 (playerInfo.player.rigidbody2D.velocity.x, 0);
		}

		protected bool hittingEnemyFromAbove(Collision2D collision)
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
	}

	class Stomping : State
	{
		public Stomping(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("Stomping"); }

		public override State collisionActions (Collision2D collision)
		{
			if (hittingEnemyFromAbove(collision))
			{
				GameObject.Destroy(collision.collider.gameObject);
				return new StartSmallJump(playerInfo);
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
