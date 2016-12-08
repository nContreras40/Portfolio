using UnityEngine;
using System.Collections;
using System.Linq;

//========================================================================================================
//                                              Damage on Collision
// This script should be applied to an item that gives damage ON CONTACT
// It then applies the public damage num to health and checks if the player is still colliding
// Damage continues until the collider is exited.
//========================================================================================================

public class DamageOnCollision : MonoBehaviour
{
   public int damageToTake;
   public int fearDamageToTake;

   //private bool takingFireDamage;
   private bool takeDamage;

   [Tooltip("Tick this box if you wish to deal fire damage instead of normal damage.")]
   public bool dealFireDamage;
   public bool isTrigger = false;

   [Tooltip("How often to deal damage, interval in seconds.  Does nothing is dealFireDamage is active.")]
   public float intervalTime = 0.0f;
   //public float fireTimer = 0.0f;
   private float timer = 0.0f;

   // Use this for initialization
   void Awake()
   {
      takeDamage = false;
   }

   void Start()
   {
      if (dealFireDamage)
      {
         StatusManager.getInstance().fireDamage = this.damageToTake;
      }
   }

   // Update is called once per frame
   void Update()
   {
      if (takeDamage)
      {
         if (timer > intervalTime)
         {
            dealDamage();
            timer = 0.0f;
         }
      }
      //if ( dealFireDamage )
      //{
      //   this.fireTimer += Time.deltaTime;
      //   if (fireTimer >= 1.0f)
      //   {
      //      this.fireDamage(false);
      //   }

      //}

      timer += Time.deltaTime;
   }

   // Called when another collider collides with this collider
   void OnCollisionEnter(Collision other)
   {
      if (!this.isTrigger && other.collider.name.Equals("Kira"))
      {
         if (this.dealFireDamage)
         {
            this.fireDamage(true);
         }
         else
         {
            takeDamage = true;
         }
      }
      else if (this.isTrigger && other.gameObject.tag.Equals("Player"))
      {
         damageFollowers(other.gameObject.name);
      }
   }

   // Called when another collider stops colliding with this collider.
   void OnCollisionExit(Collision other)
   {
      if (!this.isTrigger && other.collider.name.Equals("Kira"))
      {
         takeDamage = false;

         if (this.dealFireDamage)
         {
            this.fireDamage(false);
         }
      }
   }

   // Called when another collider enters this collision zone.
   void OnTriggerEnter(Collider other)
   {
      if (this.isTrigger && other.transform.name.Equals("Kira"))
      {
         if (this.dealFireDamage)
         {
            this.fireDamage(true);
         }
         else
         {
            takeDamage = true;
         }
      }
      else if (this.isTrigger && other.tag.Equals("Player"))
      {
         damageFollowers(other.name);
      }
   }

   // Called when another collider exits this collision zone.
   void OnTriggerExit(Collider other)
   {
      if (this.isTrigger && other.transform.name.Equals("Kira"))
      {
         takeDamage = false;

         if (this.dealFireDamage)
         {

            this.fireDamage(false);
         }
      }
   }

   // Do "damage" to the followers, teleporting them to the player
   void damageFollowers(string name)
   {
      Player[] playerUnits = PlayerManager.getInstance().players;

      for (int ix = 0; ix < playerUnits.Length; ix++)
      {
         if (playerUnits[ix].name == name)
         {
            //playerUnits[ix].transform.position = thePlayer.transform.position;
            playerUnits[ix].transform.position = PlayerManager.getInstance().players.First(player => { return player != null && player is Girl; }).transform.position;
         }
      }
   }

   // Do damage to the player
   void dealDamage()
   {
      StatusManager.getInstance().health -= damageToTake;
      StatusManager.getInstance().fear += fearDamageToTake;
   }

   // Activate or Deactivate the fire damage effect.
   void fireDamage(bool state)
   {
      StatusManager.getInstance().onFire = state;
      StatusManager.getInstance().fear += fearDamageToTake;

      //this.takingFireDamage = state;
   }
}
