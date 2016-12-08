using UnityEngine;
using System.Collections;
using System.Linq;

public class Jaguar : Enemy
{
   //-----------------------------------------------------------------------------
   // Public Inspector-editable variables
   [Tooltip("Changing this value will change the detection radius of the jaguar.")]
   public float detectionRadius = 5;

   [Tooltip("Checkmark this box if you wish to provide custom values below.")]
   public bool overrideValues;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public int jaguarHealthCustom;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public int jaguarDamageCustom;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public float jaguarSpeedCustom;

   [Tooltip("If you wish to override this value, checkmark \"Override Values\"")]
   public float jaguarRotationSpeedCustom;

   private NavMeshAgent myAgent;

   //-----------------------------------------------------------------------------
   // Default values for an jaguar, provided by juan.
   private const int jaguarHEALTHDEFAULT = 5;
   private const int jaguarDAMAGEDEFAULT = 10;
   private const float jaguarSPEEDDEFAULT = 1000;
   private const float jaguarROTATIONSPEEDDEFAULT = 2;
   private const float ATTACKINTERVAL = 0.5f;

   //private Vector3 startPos;
   private Vector3 targetDestination;
   private bool targetIsPlayer;

   //-----------------------------------------------------------------------------
   // Private member variable data.
   private float timeSinceLastAttack = 0.0f;   // The time elapsed since this jaguar has last attacked.
   private bool isInAttackRadius = false;     // Is the player within this jaguar's attack radius?

   //-----------------------------------------------------------------------------
   // A reference to the player.
   protected Player thePlayer;

   //=============================================================================
   // Initialize things here
   void Awake()
   {
      if (overrideValues)  // If custom values are provided, assign them to this jaguar.
      {
         this.myHealth = jaguarHealthCustom;
         this.myDamage = jaguarDamageCustom;
         //this.mySpeed = jaguarSpeedCustom;
         this.myRotationSpeed = jaguarRotationSpeedCustom;
      }
      else  // If custom values are not provided, utilize the default values for this jaguar.
      {
         this.myHealth = jaguarHEALTHDEFAULT;
         this.myDamage = jaguarDAMAGEDEFAULT;
         this.myRotationSpeed = jaguarROTATIONSPEEDDEFAULT;
      }

      //this.GetComponent<NavMeshAgent>().speed = this.mySpeed;
      this.myType = enType.JAGUAR;

   }

   //=============================================================================
   // Post Initialization things here
   void Start()
   {
      thePlayer = PlayerManager.getInstance().players.First(player => { return player != null && player is Girl; });
      //this.startPos = this.transform.position;
      this.myAgent = this.GetComponent<NavMeshAgent>();

      //this.myAgent.speed = this.mySpeed;
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
            killjaguar();
            break;
         default:
            break;
      }

      checkjaguarHealth();
      stateUpdate();
   }

   //=============================================================================
   // Sets the detection radius of this jaguar to the passed value.
   public void setDetectionRadius(float radius)
   {
      this.detectionRadius = radius;
   }

   //=============================================================================
   // Updates the state of this jaguar, if needed.
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
            //jaguar is dead, object should be destroyed, if not already.
            killjaguar();
            break;
      }
   }

   //=============================================================================
   // If something enters the trigger box, do something based upon it's type.
   void OnTriggerEnter(Collider other)
   {
      if (other.tag.Equals("Projectile"))
      {
         damagejaguar();
         SoundManager.getInstance().playEffect("jaguarSplat");
      }
   }

   //=============================================================================
   // If something enters the trigger box, do something based upon it's type.
   void OnTriggerStay(Collider other)
   {
      if (other.transform.name.Equals("Kira") && this.myState != enState.ATTACK)
      {
         this.myState = enState.ATTACK;
         //this.targetIsPlayer = false;
         //this.targetDestination = startPos;

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
   // Check to see if the health of this jaguar is 0, if so, change the state of this
   // jaguar to enState.DEAD
   void checkjaguarHealth()
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
         //if (Vector3.Distance(this.targetDestination, this.transform.position) <= 0.2f)
         //{
         //   Debug.Log("Targeting Player");
         //   this.targetDestination = thePlayer.transform.position;
         //   this.targetIsPlayer = true;
         //}
      }
      else
      {
         this.targetDestination = thePlayer.transform.position;
      }

      this.myAgent.SetDestination(targetDestination);
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
            StatusManager.getInstance().health -= this.myDamage;
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
   // "Destroys" the jaguar and all assosciated gameobjects.
   // ALL enemies will be moved to a magic value, where they will be deactivated.
   void killjaguar()
   {

      this.GetComponent<NavMeshAgent>().enabled = false;  // Disable the NavmeshAgent in order to prevent the jaguar
                                                          // from clipping back onto the platform after being "killed".
      this.transform.position = OUTOFBOUNDS;              // Move this jaguar out of bounds to the predefined location.
      this.gameObject.SetActive(false);                   // Disable this jaguar, preventing interactability.
   }

   //=============================================================================
   // Deal a single point of damage to the jaguar.
   void damagejaguar()
   {
      this.myHealth -= 1;

   }

   //=============================================================================
   // Deal a specific amount of damage to the jaguar.  A negative number may be
   // passed to heal the jaguar by the passed amount.
   void damagejaguar(int damage)
   {
      this.myHealth -= damage;
   }

}
