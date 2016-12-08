using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class delayedLoad : MonoBehaviour
{
   [Tooltip("How long to wait in seconds before loading the next scene.")]
   public float timeToWait = 0.0f;
   [Tooltip("The desired scene to load")]
   public string sceneToLoad = "err";

   private float dTime = 0.0f;
   public bool start = false;
   public bool pressToSkip = true;

   //==========================================================================
   // Use this for initialization
   void Start()
   {
   }

   //==========================================================================
   // Update is called once per frame
   void Update()
   {
      if (Input.anyKeyDown && this.pressToSkip)
      {
         loadScene();
      }

      if (start)
      {
         dTime += Time.deltaTime;
         if (dTime >= timeToWait)
         {
            loadScene();
         }
      }
   }

   //==========================================================================
   // Starts the timer
   public void beginTimer()
   {
      this.start = true;
   }

   //==========================================================================
   // Loads the scene
   public void loadScene()
   {
      SceneManager.LoadScene(sceneToLoad); 
   }
}
