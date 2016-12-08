using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaminaManager : MonoBehaviour
{
   private Bird theBird;
   private GameObject inWorldCanvas;
   private Slider mySlider;
   private Camera cameraToLookAt;
   
   //==========================================================================
   // Use this for initialization
   void Awake()
   {
      inWorldCanvas = GameObject.FindGameObjectWithTag("InWorldUI");
      if (inWorldCanvas == null) { Debug.LogError("NRC_ERR:  Couldn't find inWorldUI canvas, was the prefab dragged in?  Was the UI tagged \"InWorldUI\""); }
      theBird = GameObject.FindObjectOfType<Bird>();
      if (theBird == null) { Debug.LogWarning("NRC:  A Bird was not found in the scene, be aware that this UI will only show up when the bird is in the scene."); }

      mySlider = this.inWorldCanvas.GetComponentInChildren<Slider>();
      cameraToLookAt = Camera.main;
   }

   //==========================================================================
   // Update is called once per frame
   void Update()
   {
       if (StatusManager.getInstance().stamina >= 100.0f)
      {
         this.mySlider.gameObject.SetActive(false);
      }
      else
      {
         this.mySlider.gameObject.SetActive(true);
      }

      if (this.mySlider.gameObject.activeInHierarchy == true)
      {
         //Debug.Log("Updating bar");
         updateStaminaBar();
      }
      
   }

   //==========================================================================
   // 
   void updateStaminaBar()
   {
       mySlider.value = StatusManager.getInstance().stamina;

      transform.LookAt(transform.position + cameraToLookAt.transform.rotation * Vector3.forward,
                       cameraToLookAt.transform.rotation * Vector3.up);
   }

}
