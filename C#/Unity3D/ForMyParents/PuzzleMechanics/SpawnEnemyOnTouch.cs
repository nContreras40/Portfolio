using UnityEngine;
using System.Collections;

public class SpawnEnemyOnTouch : MonoBehaviour
{

   public GameObject theEnemy;
   public GameObject spawnPOS;
   //public AudioSource wrongSound;

   // Use this for initialization
   void Start()
   {
   }

   // Update is called once per frame
   void Update()
   {
   }

   void OnTriggerEnter(Collider other)
   {
      if (other.name.Equals("Kira"))
      {
         //if (this.wrongSound != null)
         //{
         //   Debug.Log("Made it in the collider for the wrong blocks");
         //   this.wrongSound.Play();
         //}
         Instantiate(theEnemy, spawnPOS.transform.position, Quaternion.identity);
      }
   }
}
