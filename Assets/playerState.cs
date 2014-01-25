using UnityEngine;
using System.Collections;

namespace playerStateMechanics
{
	class StateConstructorArgs
	{
		public GameObject groundCheck;
		public GameObject player;
		public float jumpForce;
		public GameObject projectilePrefab;
		public float projectileForce;

		public StateConstructorArgs(GameObject playerArg, GameObject groundCheckArg, float jumpForceArg, GameObject projectileArg, float projForceArg)
		{
			player = playerArg;
			groundCheck = groundCheckArg;
			jumpForce = jumpForceArg;
			projectilePrefab = projectileArg;
			projectileForce = projForceArg;
		}
	}

	abstract class State
	{
		public abstract void printName();
		protected StateConstructorArgs playerInfo;

		public virtual bool isNull()
		{
			return false;
		}

		public abstract void jumpActions ();
		public abstract State stateActions ();
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
					this.hitGroundActions();
					return new Grounded (playerInfo);
			}


			return new nullPlayerState (playerInfo);
		}

		protected virtual void hitGroundActions()
		{
		}
		
		public State(StateConstructorArgs constructorArgs)
		{
			playerInfo = constructorArgs;
		}
		
		protected bool checkForGround()
		{
			int checkIndex = -1;
			float playerSizeX = playerInfo.player.renderer.bounds.size.x;
			bool grounded = false;
			while (checkIndex < 2)
			{
				Vector3 offset = Vector3.right * playerSizeX * checkIndex / 2f;
				Vector3 fromPoint = playerInfo.player.transform.position + offset;
				Vector3 toPoint = playerInfo.groundCheck.transform.position + offset;
				grounded = grounded || Physics2D.Linecast (fromPoint, toPoint, 1 << LayerMask.NameToLayer("ground"));
				checkIndex++;
			}
			
			return grounded;
		}
	}

	class Grounded : State
	{
		public Grounded(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("Grounded"); }
		public override void jumpActions()
		{
			playerInfo.player.rigidbody2D.AddForce (Vector2.up * playerInfo.jumpForce);
		}
		
		public override State stateActions()
		{
			if (!checkForGround())
			{
				return new FallingBeforeSecondJump(playerInfo);
			}
			else if (Input.GetButtonDown ("Jump"))
			{
				jumpActions();
				return new Jumping(playerInfo);
			}
			else
			{
				return this;
			}
		}
	}

	class Stomping : State
	{
		public Stomping(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("Stomping"); }
		public override void jumpActions()
		{
		}
		
		public override State stateActions()
		{
			playerInfo.player.rigidbody2D.AddForce (Vector2.up * -1000f);
			return this;
		}

		protected override void hitGroundActions()
		{
			//CircleCollider2D cc = (CircleCollider2D) GameObject.Instantiate(CircleCollider2D, playerInfo.player.transform.position);
			//cc.radius = 10;
		}
	}
	
	class Jumping : State
	{
		public Jumping(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("Jumping"); }
		public override void jumpActions()
		{
			zeroPlayerVelocity ();
			playerInfo.player.rigidbody2D.AddForce (Vector2.up * playerInfo.jumpForce);
		}
		public override State stateActions ()
		{
			if (Input.GetButtonDown ("Jump"))
			{
				jumpActions ();
				return new DoubleJumping (playerInfo);
			}
			else if (isOnDownwardArc())
			{
				return new FallingBeforeSecondJump(playerInfo);
			}
			else
			{
				return this;
			}
		}
		public void zeroPlayerVelocity()
		{
			playerInfo.player.rigidbody2D.velocity = new Vector2 (playerInfo.player.rigidbody2D.velocity.x, 0);
		}
		
		protected bool isOnDownwardArc()
		{
			return playerInfo.player.rigidbody2D.velocity.y <= 0;
		}
	}
	
	class FallingBeforeSecondJump : Jumping
	{
		public FallingBeforeSecondJump(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("FallingBeforeSecondJump"); }
		
		public override State stateActions()
		{
			if (Input.GetButtonDown ("Jump"))
			{
				jumpActions();
				return new DoubleJumping(playerInfo);
			}
			else if (Input.GetButton("Jump"))
			{
				return new FloatingBeforeSecondJump(playerInfo);
			}
			else
			{
				return this;
			}
		}
	}
	
	class FloatingBeforeSecondJump : State
	{
		public FloatingBeforeSecondJump(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("FloatingBeforeSecondJump"); }
		public override void jumpActions()
		{
			playerInfo.player.rigidbody2D.velocity = new Vector2(playerInfo.player.rigidbody2D.velocity.x, -5);
		}
		
		public override State stateActions()
		{
			if (Input.GetButtonUp ("Jump"))
			{
				return getFallingState();
			}
			else
			{
				jumpActions ();
				return this;
			}
		}

		protected virtual State getFallingState()
		{
			return new FallingBeforeSecondJump(playerInfo);
		}
	}
	
	class DoubleJumping : Jumping
	{
		public DoubleJumping(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("DoubleJumping"); }
		public override void jumpActions()
		{
			// On downward arc
			if (playerInfo.player.rigidbody2D.velocity.y <= 0)
			{
				playerInfo.player.rigidbody2D.velocity = new Vector2(playerInfo.player.rigidbody2D.velocity.x, -5);
			}
		}
		public override State stateActions ()
		{
			if (isOnDownwardArc())
			{
				return new FallingAfterSecondJump(playerInfo);
			}
			else
			{
				return this;
			}
		}
	}
	class FallingAfterSecondJump : Jumping
	{
		public FallingAfterSecondJump(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("FallingBeforeSecondJump"); }
		
		public override State stateActions()
		{
			if (Input.GetButton("Jump"))
			{
				return new FloatingAfterSecondJump(playerInfo);
			}
			else
			{
				return this;
			}
		}
	}
	
	class FloatingAfterSecondJump : FloatingBeforeSecondJump
	{
		public FloatingAfterSecondJump(StateConstructorArgs constructorArgs) : base(constructorArgs) {}
		
		public override void printName() { Debug.Log ("FloatingBeforeSecondJump"); }
		
		protected override State getFallingState()
		{
			return new FallingAfterSecondJump (playerInfo);
		}
	}
}
