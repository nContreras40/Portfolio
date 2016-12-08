
//=============================================================================
// Author:  Nathan C.
// Version: 1.0
// Date:    09/07/2016
// Ownership belongs to all affiliates of Scott Free Games.
//
// Scorpion will be one of the various enemies found within the game.
//=============================================================================

using UnityEngine;
using System.Collections;
using System.Linq;

public class Scorpion : Enemy
{
   //-----------------------------------------------------------------------------
   // Public Inspector-editable variables
   [Tooltip("Changing this value will change the detection radius of the Scorpion.")]
   public float detectionRadius;

   [Tooltip("Checkmark this box if you wish to provide custom values below.")]
   public bool overrideValues;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public int scorpionHealthCustom;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public int scorpionDamageCustom;  

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public float scorpionSpeedCustom; 

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public float scorpionRotationSpeedCustom;  

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public int scorpionPoisonDamageCustom;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"\nInstances refers to how many times the player will take poison damage")]
   public int scorpionPoisonInstancesCustom;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"\nIntervals refers to how often the player will take poison damage")]
   public float scorpionPoisonIntervalCustom;

   //-----------------------------------------------------------------------------
   // Default values for a Scorpion, provided by juan.
   private const int SCORPIONHEALTHDEFAULT = 4;
   private const int SCORPIONDAMAGEDEFAULT = 5;
   private const float SCORPIONSPEEDDEFAULT = 1;
   private const float SCORPIONROTATIONSPEEDDEFAULT = 1.0f;

   private const int SCORPIONPOISONDAMAGEDEFAULT = 3;
   private const int SCORPIONPOISONINSTANCESDEFAULT = 5;
   private const float SCORPIONPOISONINTERVALDEFAULT = 1.0f;

   private const float ATTACKINTERVAL = 1.0f;        

   //-----------------------------------------------------------------------------
   // Private member variable data.
   private float timeSinceLastAttack = 0.0f;   // The time elapsed since this Scorpion has last attacked.
   private bool isInAttackRadius = false;     // Is the player within this Scorpion's attack radius?

   //-----------------------------------------------------------------------------
   // A reference to the player.
   protected Player thePlayer;
   private bool targetIsPlayer;
   private Vector3 startPos;
   private Vector3 targetDestination;

   private float poisonTimer = 0.0f;
   private float poisonDuration = 5.0f;
   private bool hasPoisonedPlayer = false;


   //=============================================================================
   // Initialize things here
   void Awake()
   {
      if (overrideValues)  // If custom values are provided, assign them to this Scorpion.
      {
         this.myHealth = scorpionHealthCustom;
         this.myDamage = scorpionDamageCustom;
         //this.mySpeed = scorpionSpeedCustom;
         this.myRotationSpeed = scorpionRotationSpeedCustom;
      }
      else  // If custom values are not provided, utilize the default values for this Scorpion.
      {
         this.myHealth = SCORPIONHEALTHDEFAULT;
         this.myDamage = SCORPIONDAMAGEDEFAULT;
         this.myRotationSpeed = SCORPIONROTATIONSPEEDDEFAULT;

         this.scorpionPoisonDamageCustom = SCORPIONPOISONDAMAGEDEFAULT;
         this.scorpionPoisonInstancesCustom = SCORPIONPOISONINSTANCESDEFAULT;
         this.scorpionPoisonIntervalCustom = SCORPIONPOISONINTERVALDEFAULT;
      }

      //this.GetComponent<NavMeshAgent>().speed = this.mySpeed;

      this.myType = enType.SCORPION;
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
            //do some patroling, maybe?
            break;
         case enState.DEAD:
            killScorpion();
            break;
         default:
            break;
      }

      poisonUpdate();
      checkScorpionHealth();
      stateUpdate();
   }

   //=============================================================================
   // Prevent the scorpion from stacking multiple instances of poison
   public void poisonUpdate()
   {
      if (StatusManager.getInstance().isPoisoned && hasPoisonedPlayer)
      {
         this.poisonTimer += Time.deltaTime;
         if (this.poisonTimer >= this.poisonDuration)
         {
            StatusManager.getInstance().isPoisoned = false;
            this.poisonTimer = 0.0f;
            this.hasPoisonedPlayer = false;
         }
      }
   }

   //=============================================================================
   // Sets the detection radius of this ant to the passed value.
   public void setDetectionRadius(float radius)
   {
      this.detectionRadius = radius;
   }

   //=============================================================================
   // Updates the state of this Scorpion, if needed.
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
            if (this.isInAttackRadius)
            {
               this.attackPlayer();
            }
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
            //if the Scorpion will patrol, patrol the Scorpion around.
            break;
         //-----------------------------------------------------------------------------
         case enState.DEAD:
            //Scorpion is dead, object should be destroyed, if not already.
            killScorpion();
            break;
      }
   }

   //=============================================================================
   // If something enters the trigger box, do something based upon it's type.
   void OnTriggerEnter(Collider other)
   {
      if (other.tag.Equals("Projectile"))
      {
         damageScorpion();
      }
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
   void checkScorpionHealth()
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
      if (timeSinceLastAttack == 0.0f)
      {
         if (StatusManager.getInstance().health < 1)
         {
         }
         else
         {
            //do damage to player.
            StatusManager.getInstance().health -= 8;
            StatusManager.getInstance().fear += 7;
            if (StatusManager.getInstance().isPoisoned == false)
            {
               StatusManager.getInstance().poisonDamage = scorpionPoisonDamageCustom;
               StatusManager.getInstance().isPoisoned = true;
               this.hasPoisonedPlayer = true;
            }
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
   void killScorpion()
   {

      this.GetComponent<NavMeshAgent>().enabled = false;  // Disable the NavmeshAgent in order to prevent the Ant
                                                          // from clipping back onto the platform after being "killed".
      this.transform.position = OUTOFBOUNDS;              // Move this Ant out of bounds to the predefined location.
      this.gameObject.SetActive(false);                   // Disable this Ant, preventing interactability.
   }

   //=============================================================================
   // Deal a single point of damage to the ant.
   void damageScorpion()
   {
      this.myHealth -= 1;

   }

   //=============================================================================
   // Deal a specific amount of damage to the ant.  A negative number may be
   // passed to heal the ant by the passed amount.
   void damageScorpion(int damage)
   {
      this.myHealth -= damage;
   }
}
