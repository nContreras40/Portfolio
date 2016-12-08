using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossUIManager : Singleton<BossUIManager>
{
   // kira's Health bar 
   public Slider kiraHealth;

   // A reference to the BunnyBoss
   public BunnyBoss bunnyBoss;

   // the Boss health bar
   public Slider bossHealth;

   //==========================================================================
   // Use this for initialization
   override protected void Init()
   {
      if (this.kiraHealth == null || this.bossHealth == null || this.bunnyBoss == null)
      {
         Debug.LogError("A reference was not set in the inspector for the BossUIManager, please check it out.");
      }
   }

   //==========================================================================
   // Update is called once per frame
   void Update()
   {
      this.kiraHealth.value = StatusManager.getInstance().health;
      this.bossHealth.value = this.bunnyBoss.health;  
   }

}
