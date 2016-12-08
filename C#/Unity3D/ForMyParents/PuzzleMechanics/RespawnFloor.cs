using UnityEngine;
using System.Collections;

//RespawnFloor code
//Creator: Pat Sipes
//Version 2.0
//Description: This code works in conjunction with the basicTrigger script. 
//It is placed on the empty object to be respawned to.
//Death floor must have the basic trigger script send the message "respawn" and be repeatable.

public class RespawnFloor : MonoBehaviour
{

   
   public float damage = 15.0f;


   // Use this for initialization
   void Awake()
   {

   }

   // Update is called once per frame
   void Update()
   {

   }

   public void OnEvent(BasicTrigger trigger) //Basic Trigger script
   {
      {
         foreach (Player unit in PlayerManager.getInstance().players)
         {
             if (trigger.activator.Equals(unit.gameObject))
            {
               unit.transform.position = transform.position; //respawn to designated respawn block
               if (unit is Girl)
               {
                   StatusManager.getInstance().health -= damage;
               }
            }
            
         }
      }
   }
}
