using UnityEngine;
using System.Collections;

public class EnemyManager : Singleton<EnemyManager>
{

   //==========================================================================
   // Use this for initialization
   override protected void Init()
   {
   }

   //==========================================================================
   // Update is called once per frame
   void Update()
   {
   }

   //==========================================================================
   // Kills all the enemies, what did ya think this did?
   // In reality this sets all enemies to be in the dead state, and the enemies
   // will move themselves to the "dead" zone.
   public void killAllEnemies()
   {
      GameObject[] enArr = GameObject.FindGameObjectsWithTag("Enemy");

      for (int ix = 0; ix < enArr.Length; ix++)
      {
         enArr[ix].GetComponent<Enemy>().setState(Enemy.enState.DEAD);
      }
   }
}
