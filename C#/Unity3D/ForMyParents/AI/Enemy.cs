//=============================================================================
// Author:  Nathan C.
// Juan was here
// Version: 1.0
// Date:    08/26/2016
// Ownership belongs to all affiliates of Scott Free Games.
//=============================================================================

using UnityEngine;
using System.Collections;

//=============================================================================
// Enemy.cs
// Generic enemy class that all future enemies will inherit from.  Enemy.cs
// will contain features that all enemies share, such as a value for health, 
// or a value for damage.
public class Enemy : MonoBehaviour
{

   //-----------------------------------------------------------------------------
   // Magic position in the game world, where all enemies will be moved to when
   // they enter the DEAD state.
   protected readonly Vector3 OUTOFBOUNDS = new Vector3(-1000, -1000, -1000);

   //-----------------------------------------------------------------------------
   // Common states that most enemies, if not all, share.
   public enum enState
   {
      MOVE = 0,
      TRACK,
      ATTACK,
      IDLE,
      SPECIAL,
      DEAD
   }

   //-----------------------------------------------------------------------------
   // A list of the common enemy types that can be found in the world. 
   public enum enType
   {
      UNDEF = -1,
      FRIEND = 0,
      ANT = 1,
      SCORPION,
      SNAKE,
      PECCARY,
      CROCODILE,
      JAGUAR,
      MONKEY,
      BAT,
      SHARK,
      FLAPFLAP
   }

   //-----------------------------------------------------------------------------
   // Protected member data
   protected enState myState; // The current state of this enemy, as defined in enState.
   protected enType myType;   // The current type of this enemy, as defined in enType.

   [Tooltip("Directly editing this value will have no effect, this is purely for debugging purposes")]
   public int myHealth;   // The health of this enemy.
   protected int myDamage;   // The damage this enemy does to other GameObjects.

   //protected float mySpeed;          // The base speed at which this enemy moves.
   protected float myRotationSpeed;  // The base speed at which this enemy will "turn".

   //=============================================================================
   // Default ctor
   public Enemy()
   {
      this.myState = enState.IDLE;
      this.myType = enType.UNDEF;

      this.myHealth = 1;
      this.myDamage = -1;
      //this.mySpeed = 1;
      this.myRotationSpeed = 1;
   }

   //=============================================================================
   // Update is called once per frame
   void Update()
   {
   }

   //=============================================================================
   // set the state of this enemy
   public enState setState(enState newState)
   {
      return this.myState = newState;
   }

   //=============================================================================
   // Check to see if the enemy has been defeated.  The enemy is defeated when
   //  it's remaining health is 0.
   protected bool isDefeated()
   {
      if (this.myHealth <= 0)
      {
         return true;
      }
      return false;
   }
}

