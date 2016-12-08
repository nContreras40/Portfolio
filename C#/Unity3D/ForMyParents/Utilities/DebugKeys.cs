using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DebugKeys : MonoBehaviour
{

   public bool debugKeysEnabled = false;
   public bool cheatCodesEnabled = true;
   private Vector3 worldSpawn;
   private Player thePlayer;

   private bool godMode = false;
   private bool soulsMode = false;

   private ArrayList keys;

   // Use this for initialization
   void Awake()
   {
      if (debugKeysEnabled)
         Debug.Log("DebugKeys is in use, look for the DebugKeys script under the Director is you wish to disable these!");
      if (cheatCodesEnabled)
         Debug.Log("CheatCodes are in use");
      this.keys = new ArrayList();
   }
   

   void Start()
   {
      thePlayer = PlayerManager.getInstance().players.First(player => { return player != null || player is Girl; });
      worldSpawn = thePlayer.transform.position;
   }

   //==========================================================================
   // Update is called once per frame
   void Update()
   {
      if (Input.anyKeyDown)
      {
         foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
         {
            if (Input.GetKeyDown(key))
            {
               this.keys.Add(key);
            }
         }
      }

      //=======================================================================
      // If cheat codes are enabled, use these codes
      if (this.keys.Count >= 5 && cheatCodesEnabled)
      {
         // load next scene
         if (getCode(KeyCode.G, KeyCode.N, KeyCode.E, KeyCode.X, KeyCode.T))
         {
            loadNextScene();
            this.keys.Add(KeyCode.Colon);
            print("Loading next scene");
         }
         // load previous scene
         if (getCode(KeyCode.G, KeyCode.P, KeyCode.R, KeyCode.E, KeyCode.V))
         {
            loadPrevScene();
            this.keys.Add(KeyCode.Colon);
            print("Loading previous scene");
         }
         // reset the level
         if (getCode(KeyCode.R, KeyCode.E, KeyCode.S, KeyCode.E, KeyCode.T))
         {
            this.keys.Add(KeyCode.Colon);
            print("Loading previous scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);                    
         }
         // god mode
         if (getCode(KeyCode.I, KeyCode.D, KeyCode.D, KeyCode.Q, KeyCode.D))
         {
            this.godMode = !this.godMode;
            this.keys.Add(KeyCode.Colon);
            print("God Mode Enabled");
         }
         // souls mode
         if (getCode(KeyCode.S, KeyCode.O, KeyCode.U, KeyCode.L, KeyCode.S))
         {
            this.soulsMode = !this.soulsMode;
            if (this.soulsMode) { StatusManager.getInstance().fear = 0.0f; }
            this.keys.Add(KeyCode.Colon);
            print("Souls mode enabled");
         }
         if (getCode(KeyCode.L, KeyCode.O, KeyCode.W, KeyCode.H, KeyCode.P))
         {
            StatusManager.getInstance().health = 10;
         }
         if (getCode(KeyCode.M, KeyCode.A, KeyCode.X, KeyCode.H, KeyCode.P))
         {
            StatusManager.getInstance().health = 100;
         }
         if (getCode(KeyCode.T, KeyCode.O, KeyCode.K, KeyCode.E, KeyCode.N))
         {
            moveTokens();
         }
		 if (getCode(KeyCode.S, KeyCode.P, KeyCode.E, KeyCode.E, KeyCode.D))
		 {
				thePlayer.movementSpeed *= 2;
            SoundManager.getInstance().playEffect("notSonicTheHedgehog");
				(thePlayer as Girl).animator.speed *= 2;
				this.keys.Clear();
		 }

         updateStatus();

         //-----------------------------------------------------------------------
         // removes the first element of the keys array
         for (int ix = this.keys.Count; ix > 5; ix--)
         {
            this.keys.RemoveAt(0);
         }
      }
   
      //=======================================================================
      // If debug/developer keys are enabled, do these debug keys.  These are
      // quick alternatives to cheat codes.
      if (debugKeysEnabled)
      {
         //-----------------------------------------------------------------------
         // Load next scene or previous scene
         if (Input.GetKeyDown(KeyCode.PageUp))
         {
            loadNextScene();
         }
         else if (Input.GetKeyDown(KeyCode.PageDown))
         {
            loadPrevScene();
         }

         //-----------------------------------------------------------------------
         // Bring animals back to kira
         if (Input.GetKeyDown(KeyCode.Insert))
         {
            Player[] playerUnits = PlayerManager.getInstance().players;

            for (int ix = 0; ix < playerUnits.Length; ix++)
            {
               if (!(playerUnits[ix] is Girl))
               {
                  playerUnits[ix].transform.position = thePlayer.transform.position;
               }
            }
         }

         //-----------------------------------------------------------------------
         // Kill all active enemies
         if (Input.GetKeyDown(KeyCode.Delete))
         {
            EnemyManager.getInstance().killAllEnemies();
         }

         //-----------------------------------------------------------------------
         // Return to beginning of level
         if (Input.GetKeyDown(KeyCode.Home))
         {
            Player[] playerUnits = PlayerManager.getInstance().players;

            for (int ix = 0; ix < playerUnits.Length; ix++)
            {
               playerUnits[ix].transform.position = this.worldSpawn;
            }
         }

         //-----------------------------------------------------------------------
         // Set Kira's health to 0
         if (Input.GetKeyDown(KeyCode.End))
         {
            StatusManager.getInstance().health = 0;
         }

         //-----------------------------------------------------------------------
         // IDDQD
         if (Input.GetKeyDown(KeyCode.Equals))
         {
            this.godMode = !this.godMode;
         }
      }
   }

   //==========================================================================
   // 
   private void updateStatus()
   {
      if (this.godMode)
      {
         StatusManager.getInstance().health = 100;
         StatusManager.getInstance().fear = 0;
      }
      if (this.soulsMode)
      {
         StatusManager.getInstance().fear = 100;
      }
   }

   //==========================================================================
   // Loads the next scene
   private void loadNextScene()
   {
      if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings)
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      }
   }

   //==========================================================================
   // Loads the previous scene
   private void loadPrevScene()
   {
      if (SceneManager.GetActiveScene().buildIndex > 0)
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
      }
   }

   //==========================================================================
   // Move the tokens in the level in front of the player
   private void moveTokens()
   {
      GameObject[] tokens = GameObject.FindGameObjectsWithTag("BunnyToken");
      if (tokens.Count<GameObject>() > 0)
      {
         float pos = 1.0f;
         foreach(GameObject token in tokens)
         {
            pos += 1.0f;
            token.transform.position = new Vector3(thePlayer.transform.position.x,
                                                   thePlayer.transform.position.y,
                                                   thePlayer.transform.position.z + pos);
         }
      }
   }

   //==========================================================================
   // Check whether or not the keys array contains the code passed. 
   // If code passed matches the keys in the array, return true, the code was
   // successful.  Else, return false, the code didn't match.
   private bool getCode(KeyCode keyOne, KeyCode keyTwo, KeyCode keyThree, KeyCode keyFour, KeyCode keyFive)
   {
      if (this.keys[0].Equals(keyOne) &&
          this.keys[1].Equals(keyTwo) &&
          this.keys[2].Equals(keyThree) &&
          this.keys[3].Equals(keyFour) &&
          this.keys[4].Equals(keyFive))
      {
         SoundManager.getInstance().playEffect("Light1");
         return true;
      }
      else
      {
         return false;
      }
   }
}
