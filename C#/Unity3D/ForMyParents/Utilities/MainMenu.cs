using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

   public string mainMenuScene = "TitleScreen";
   public string infoScene = "ControlScene";
   public string firstScene = "IntroSequence";
   public string creditScene = "CreditsScene";

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void LateUpdate () {
      if (SoundManager.getInstance().gameObject.GetComponent<AudioSource>().isPlaying == false)
      {
         SoundManager.getInstance().playMusic("GameTheme(Temp Placement)");
      }
   }

   public void pressPlay()
   {
      Destroy(SoundManager.getInstance().gameObject);
      SceneManager.LoadScene(firstScene);
   }

   public void pressInstructions()
   {
      SceneManager.LoadScene(infoScene);
   }

   public void pressCredits()
   {
      Debug.Log("pressed");
      SoundManager.getInstance().stopAll();
      SoundManager.getInstance().playMusic("Memories_Bensound");
      SceneManager.LoadScene(creditScene);
      
   }

   public void pressTitle()
   {
      if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("CreditsScene"))
      {
         SoundManager.getInstance().stopAll();
         SoundManager.getInstance().playMusic("GameTheme(Temp Placement)");
      }
      SceneManager.LoadScene(mainMenuScene);
   }

   public void pressQuit()
   {
      Application.Quit();
   }

   
}
