//=============================================================================
// Author:  Nathan C.
// Version: 1.0
// Date:    08/26/2016
// Ownership belongs to all affiliates of Scott Free Games.
// Ant.cs will represent one of the enemies in the game.
//=============================================================================

using UnityEngine;
using System.Collections;
using System.Linq;

public class Ant : Enemy
{
   //-----------------------------------------------------------------------------
   // Public Inspector-editable variables
   [Tooltip("Changing this value will change the detection radius of the Ant.")]
   public float detectionRadius = 5;

   [Tooltip("Checkmark this box if you wish to provide custom values below.")]
   public bool overrideValues;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public int antHealthCustom;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public int antDamageCustom;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public float antSpeedCustom;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public float antRotationSpeedCustom;

   //-----------------------------------------------------------------------------
   // Default values for an Ant, provided by juan.
   private const int ANTHEALTHDEFAULT = 2;
   private const int ANTDAMAGEDEFAULT = 5;
   private const float ANTSPEEDDEFAULT = 2;
   private const float ANTROTATIONSPEEDDEFAULT = 1;
   private const float ATTACKINTERVAL = 1.0f;

   private Vector3 startPos;
   private Vector3 targetDestination;
   private bool targetIsPlayer;

   //-----------------------------------------------------------------------------
   // Private member variable data.
   private float timeSinceLastAttack = 0.0f;   // The time elapsed since this Ant has last attacked.
   private bool isInAttackRadius = false;     // Is the player within this Ant's attack radius?

   private Animator myAnimator;
   public Animator animator
   {
      get
      {
         if (myAgent == null)
         {
            myAnimator = GetComponent<Animator>();
         }
         return myAnimator;
      }
   }

   private NavMeshAgent myAgent;
   public NavMeshAgent agent
   {
      get
      {
         if (myAgent == null)
         {
            myAgent = GetComponent<NavMeshAgent>();
         }
         return myAgent;
      }
   }

   //-----------------------------------------------------------------------------
   // A reference to the player.
   protected Player thePlayer;

   //=============================================================================
   // Initialize things here
   void Awake()
   {
      if (overrideValues)  // If custom values are provided, assign them to this Ant.
      {
         this.myHealth = antHealthCustom;
         this.myDamage = antDamageCustom;
         //this.mySpeed = antSpeedCustom;
         this.myRotationSpeed = antRotationSpeedCustom;
      }
      else  // If custom values are not provided, utilize the default values for this Ant.
      {
         this.myHealth = ANTHEALTHDEFAULT;
         this.myDamage = ANTDAMAGEDEFAULT;
         this.myRotationSpeed = ANTROTATIONSPEEDDEFAULT;
      }

      //this.GetComponent<NavMeshAgent>().speed = this.mySpeed;
      this.myType = enType.ANT;

   }

   //=============================================================================
   // Post Initialization things here
   void Start()
   {
      thePlayer = PlayerManager.getInstance().players.First(player => { return player != null && player is Girl; });
      this.startPos = this.transform.position;
   }

   //=============================================================================
   // Update is called once per frames
   void Update()
   {
      switch (this.myState)
      {
         case enState.IDLE:
            break;
         case enState.TRACK:
            pursueTarget();
            break;
         case enState.ATTACK:
            attackPlayer();
            break;
         case enState.MOVE:
            break;
         case enState.DEAD:
            killAnt();
            break;
         default:
            break;
      }

      checkAntHealth();
      stateUpdate();
   }

   //=============================================================================
   // Sets the detection radius of this ant to the passed value.
   public void setDetectionRadius(float radius)
   {
      this.detectionRadius = radius;
   }

   //=============================================================================
   // Updates the state of this ant, if needed.
   void stateUpdate()
   {
      switch (this.myState)
      {
         //-----------------------------------------------------------------------------
         case enState.IDLE:
            //check to see if the player has entered the aggression radius
            if (isPlayerNearby())
            {
               this.targetIsPlayer = true;
               this.targetDestination = thePlayer.transform.position;
               this.myState = enState.TRACK;
            }
            break;
         //-----------------------------------------------------------------------------
         case enState.TRACK:
            //check to see if the player has left aggression radius
            if (!isPlayerNearby())
            {
               this.myState = enState.IDLE;
            }
            break;
         //-----------------------------------------------------------------------------
         case enState.ATTACK:
            //check to see if the player has left the attack radius
            if (isPlayerNearby() && !isInAttackRadius)
            {
               this.myState = enState.MOVE;
            }
            // check to see if the player has left the aggression radius
            else if (!isPlayerNearby() && !isInAttackRadius)
            {
               this.myState = enState.IDLE;
            }
            break;
         //-----------------------------------------------------------------------------
         case enState.MOVE:
            break;
         //-----------------------------------------------------------------------------
         case enState.DEAD:
            //Ant is dead, object should be destroyed, if not already.
            killAnt();
            break;
      }
   }

   //=============================================================================
   // If something enters the trigger box, do something based upon it's type.
   void OnTriggerEnter(Collider other)
   {
      if (other.tag.Equals("Projectile"))
      {
         damageAnt();
         SoundManager.getInstance().playEffect("AntSplat");
      }
   }

   //=============================================================================
   // If something enters the trigger box, do something based upon it's type.
   void OnTriggerStay(Collider other)
   {
      if (other.transform.name.Equals("Kira") && this.myState != enState.ATTACK)
      {
         this.myState = enState.ATTACK;
         this.targetIsPlayer = false;
         this.targetDestination = startPos;

      }
   }

   //=============================================================================
   // If something enters the trigger box, do something based upon it's type.
   void OnTriggerExit(Collider other)
   {
      if (other.tag.Equals("Player"))

      {
         this.myState = enState.TRACK;
      }
   }

   //=============================================================================
   // Check to see if the health of this Ant is 0, if so, change the state of this
   // Ant to enState.DEAD
   void checkAntHealth()
   {
      if (isDefeated())
      {
         this.myState = enState.DEAD;
      }
   }

   //=============================================================================
   // Follow the player around, until the player enters the hitbox for attacking.
   void pursueTarget()
   {
      this.transform.LookAt(new Vector3(targetDestination.x,
                                        this.transform.position.y,
                                        targetDestination.z));

      if (this.targetIsPlayer == false)
      {
         if (Vector3.Distance(this.targetDestination, this.transform.position) <= 0.2f)
         {
            Debug.Log("Targeting Player");
            this.targetDestination = thePlayer.transform.position;
            this.targetIsPlayer = true;
         }
      }
      else
      {
         this.targetDestination = thePlayer.transform.position;
      }

      this.GetComponent<NavMeshAgent>().SetDestination(targetDestination);
   }

   //=============================================================================
   // Returns whether or not the player is within aggression radius.
   bool isPlayerNearby()
   {
      bool withinX = false;
      bool withinY = false;
      bool withinZ = false;

      if (Mathf.Abs(this.transform.position.x - thePlayer.transform.position.x) <= this.detectionRadius)
      {
         withinX = true;
      }
      if (Mathf.Abs(this.transform.position.y - thePlayer.transform.position.y) <= this.detectionRadius)
      {
         withinY = true;
      }
      if (Mathf.Abs(this.transform.position.z - thePlayer.transform.position.z) <= this.detectionRadius)
      {
         withinZ = true;
      }

      return ((withinX == withinY) && (withinY == withinZ) && (withinZ == withinX));
   }

   //=============================================================================
   // Attack the player, and prevent damaging for a small period of time.
   void attackPlayer()
   {
      if (timeSinceLastAttack <= 0.0f)
      {
         if (StatusManager.getInstance().health < 1)
         {
         }
         else
         {
            //do damage to player.
            SoundManager.getInstance().playEffect("Ant_Attack_01");
            StatusManager.getInstance().health -= 5;
            StatusManager.getInstance().fear += 5;
         }

         timeSinceLastAttack += Time.deltaTime;
      }
      else
      {
         timeSinceLastAttack += Time.deltaTime;
      }

      if (timeSinceLastAttack >= ATTACKINTERVAL)
      {
         timeSinceLastAttack = 0;
      }
   }

   //=============================================================================
   // "Destroys" the ant and all assosciated gameobjects.
   // ALL enemies will be moved to a magic value, where they will be deactivated.
   void killAnt()
   {

      this.GetComponent<NavMeshAgent>().enabled = false;  // Disable the NavmeshAgent in order to prevent the Ant
                                                          // from clipping back onto the platform after being "killed".
      this.transform.position = OUTOFBOUNDS;              // Move this Ant out of bounds to the predefined location.
      this.gameObject.SetActive(false);                   // Disable this Ant, preventing interactability.
   }

   //=============================================================================
   // Deal a single point of damage to the ant.
   void damageAnt()
   {
      this.myHealth -= 1;

   }

   //=============================================================================
   // Deal a specific amount of damage to the ant.  A negative number may be
   // passed to heal the ant by the passed amount.
   void damageAnt(int damage)
   {
      this.myHealth -= damage;
   }

   void OnAnimatorMove()
   {
      // Update position based on animation movement using navigation surface height
      Vector3 position = animator.rootPosition;
      position.y = agent.nextPosition.y;
      transform.position = position;
   }

}
