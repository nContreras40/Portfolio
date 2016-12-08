//========================================================================================================
// Author:  Nathan C.
// Version: 0.1
// Spawner.cs will function as a object that will bring enemies into the game.  It can be set to infinitely
// spawn an enemy type.  This will cause up to 5 enemies to exist on screen, and after one dies, another
// will spawn.  It can also be set to a specific limit, where once the limit has been reached, the spawner
// will cease spawning enemies.
//
// In terms of using this class:
// Attach this class to an empty game object.  You will need a secondary game object which will be the
// desired spawn location of the enemy.  You may attach a box collider and rigid body to this gameObject.
//========================================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
   enum State
   {
      ACTIVE,
      PAUSED,
      DONE
   }

   [Tooltip("The distance that the spawned enemy will recognized the player")]
   public float detectionRadius;

   [Tooltip("The total number of enemies you want this spawner to spawn.")]
   public int totalNumberOfEnemies;

   [Tooltip("The amount of time that should elapse before another enemy is spawned.")]
   public float timeBetweenWaves;

   [Tooltip("The desired enemy type to be spawned.  Be sure to use a prefab, and not the model.")]
   public GameObject enemy;

   [Tooltip("The trigger to be set when all enemies are dead.")]
   public BasicTrigger trigger;

   [Tooltip("The desired spawnpoint for this spawner.")]
   public GameObject spawnPoint;

   private bool hasStartedSpawning;   //Have we started spawning?

   private float dTime;         //How much time has elapsed?

   private State spawnState;    //Are we currently spawning enemies?

   private List<GameObject> enemyList = new List<GameObject>();       //ArrayList of the spawned enemies.

   //========================================================================================================
   // Use this for initialization
   void Awake()
   {
      this.hasStartedSpawning = false;
      this.spawnState = State.PAUSED;
      this.dTime = 0.0f;
   }

   //========================================================================================================
   // Update is called once per frame
   void Update()
   {
      if (spawnState == State.ACTIVE)
      {
         this.hasStartedSpawning = true;
         // If we have reached the time to spawn an enemy, and we are still allowed to spawn an enemy...
         if ( this.enemyList.Count < this.totalNumberOfEnemies )
         {
             if (this.dTime >= timeBetweenWaves)
             {
                 this.dTime = 0.0f;             // reset the timer
                 if (this.enemyList.FindAll(obj => { return obj.activeSelf; }).Count < 5)  // If there are fewer than 5 enemies on screen...
                 {
                    spawnEnemy();               // Spawn a new enemy
                 }
             }
            this.dTime += Time.deltaTime;  // Increment the timer
         }
         else if (this.enemyList.FindAll(obj => { return obj.activeSelf; }).Count == 0 && hasStartedSpawning) //we've finished spawning enemies
         {
            print("End");
            spawnState = State.DONE;
            foreach ( GameObject obj in enemyList )
            {
               Destroy(obj);
            }
            enemyList.Clear();
            trigger.OnAction();
         }
      }
   }

   //========================================================================================================
   // When the player enters the trigger zone, enable spawning enemies if the option has been selected.
   void OnTriggerEnter(Collider other)
   {
      if (other.tag.Equals("Player") && spawnState != State.DONE)
      {
         spawnState = State.ACTIVE;
      }
   }

   //========================================================================================================
   // Spawn an enemy
   void spawnEnemy()
   {
      Vector3 startPos = this.spawnPoint.transform.position;
      GameObject ant = Instantiate(enemy, startPos, Quaternion.identity) as GameObject;
      ant.GetComponent<Ant>().setDetectionRadius(this.detectionRadius);
      this.enemyList.Add(ant);

   }

   //========================================================================================================
   // Returns the number of remaining enemies for the spawner to spawn.
   public int getRemainingEnemies()
   {
      return (totalNumberOfEnemies - enemyList.Count);
   }

   //========================================================================================================
   // Returns whether or not this spawner is spawning enemies.
   public bool getSpawningStatus()
   {
      return this.spawnState == State.ACTIVE;
   }
}
