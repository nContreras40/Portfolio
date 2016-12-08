using UnityEngine;
using System.Collections;

public class StatusManager : Singleton<StatusManager>
{
   public enum Status
   {
      NONE = 0x00,    //0000
      FEAR = 0x01,    //0001
      POISON = 0x02,  //0010
      FIRE = 0x04,    //0100
      SPOOK = 0x08    //1000
   }

   [HideInInspector]
   public Vector3 respawnPoint;

   private float myFear = 0.0f;
   public float fear
   {
      get { return myFear; }
      set
      {
         myFear = Mathf.Clamp(value, 0, 120.0f);
         if (myFear > 0)
         {
            myPlayerStatus |= Status.FEAR;
         }
         else
         {
            myPlayerStatus &= ~Status.FEAR;
         }
         updateEffects();
      }
   }

   public bool isPoisoned
   {
      get { return hasStatus(Status.POISON); }
      set
      {
         if (value)
         {
            myPlayerStatus |= Status.POISON;
         }
         else
         {
            myPlayerStatus &= ~Status.POISON;
         }
         updateEffects();
      }
   }

   public bool onFire
   {
      get { return hasStatus(Status.FIRE); }
      set
      {
         if (value)
         {
            myPlayerStatus |= Status.FIRE;
         }
         else
         {
            myPlayerStatus &= ~Status.FIRE;
         }
         updateEffects();
      }
   }

   public bool isSpooked
   {
      get { return hasStatus(Status.SPOOK); }
      set
      {
         if (value)
         {
            myPlayerStatus |= Status.SPOOK;
         }
         else
         {
            myPlayerStatus &= ~Status.SPOOK;
         }
         updateEffects();
      }
   }

   private float myHealth = 100.0f;
   public float health
   {
      get { return myHealth; }
      set { myHealth = Mathf.Clamp(value, 0, 100.0f); }
   }

   private float myStamina = 100.0f;
   public float stamina
   {
      get { return myStamina; }
      set { myStamina = Mathf.Clamp(value, 0, 100.0f); }
   }

   private Status myPlayerStatus = Status.NONE;
   public Status playerStatus
   {
      get { return myPlayerStatus; }
      set { SetStatus(value); updateEffects(); }
   }

   [Range(1.0f, 10.0f)]
   public float fearDamage;

   [Range(1.0f, 10.0f)]
   public float poisonDamage;

   [Range(1.0f, 10.0f)]
   public float fireDamage;

   [Range(1.0f, 10.0f)]
   public float spookDamage;


   public GameObject fearEffect;
   public GameObject highFearEffect;
   public GameObject poisonEffect;
   public GameObject fireEffect;
   public UnityStandardAssets.ImageEffects.Fisheye fisheye;
   public UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration vignette;

   //public GameObject 

   // Use this for initialization
   override protected void Init()
   {
      playerStatus = Status.FEAR;
      health = 100;
   }

   void Start()
   {
      this.respawnPoint = PlayerManager.getInstance().currentPlayer.transform.position;
   }
   /// <summary>
   /// this is a function to switch state., Use this to toggle player's particle effects
   /// </summary>
   /// <param name="s"></param>
   public void ToggleStatus(Status status)
   {
      if (status == Status.NONE)
      {
         myPlayerStatus = Status.NONE;
      }
      else
      {
         myPlayerStatus ^= status;
      }
      updateEffects();
   }

   /// <summary>
   /// this is a function to set state., Use this to sets the player's particle effects
   /// </summary>
   /// <param name="s"></param>
   public void SetStatus(Status status)
   {
      if (status == Status.NONE)
      {
         myPlayerStatus = Status.NONE;
      }
      else
      {
         myPlayerStatus |= status;
      }
      updateEffects();
   }

   //updates effect based on playerStatus.
   public void updateEffects()
   {
      fearEffect.SetActive(hasStatus(Status.FEAR));
      poisonEffect.SetActive(hasStatus(Status.POISON));
      fireEffect.SetActive(hasStatus(Status.FIRE));
      highFearEffect.SetActive(fear >= 100 && !highFearEffect.activeSelf);
   }

   public bool hasStatus(Status status)
   {
      return (myPlayerStatus & status) == status;
   }

   // Update is called once per frame
   void Update()
   {

      //fear accumulation
      fear += (hasStatus(Status.FEAR) ? fearDamage : -fearDamage) * Time.deltaTime;

      //set fisheye strength
      fisheye.strengthX = fisheye.strengthY = (fear / 100) * 0.8f;

      //set vignette strength
      vignette.intensity = (fear / 100) * 0.28f + 0.32f;

      //fear damage
      health -= (fear >= 100 ? fearDamage : 0.0f) * Time.deltaTime;

      //poison damage
      health -= (isPoisoned ? poisonDamage : 0.0f) * Time.deltaTime;

      //fire damage
      health -= (onFire ? fireDamage : 0.0f) * Time.deltaTime;

      //spook damage
      health -= (isSpooked ? spookDamage : 0.0f) * Time.deltaTime;

   }

   void LateUpdate()
   {
      //respawn players when kira dies.
      if (health <= 0)
      {
         Player[] players = PlayerManager.getInstance().players;
         for (int i = 0; i < players.Length; i++)
         {
            players[i].transform.position = respawnPoint;
         }
         //health = 100.0f;
         fear = 0.0f;
      }
   }
}
