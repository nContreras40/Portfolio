using UnityEngine;
using System.Collections;

//========================================================================================================
//                                              Fear Shadow
// This script is attached to anything that needs to apply fear damage (that misses the light collider)
// 
//========================================================================================================

public class FearShadow : MonoBehaviour {

    [Tooltip("damage to take")]
   public float fearDamage;

   void OnTriggerStay(Collider other)
   {
      if (other.transform.tag == "Player" && other.gameObject.GetComponent<Girl>() != null)
      {
          StatusManager.getInstance().fear += fearDamage;
      }
   }
}
