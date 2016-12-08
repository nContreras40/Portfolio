using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Reticule : MonoBehaviour {

   private bool isTargeting;
   private Player thePlayer;

   public Image theReticule;

   private readonly Vector3 cameraOutOfBounds = new Vector3(-1000f, -1000f, -1000f);

   void Awake()
   {
   }

   // Use this for initialization
   void Start() {
      thePlayer = PlayerManager.getInstance().players.First(player => { return player != null && player is Girl; });
   }

   // Update is called once per frame
   void Update() {

      checkIfTargeting();

      if (isTargeting)
      {
         theReticule.transform.position = Camera.main.WorldToScreenPoint(this.thePlayer.gameObject.GetComponent<Girl>().target.position);
      }
      else
      {
         theReticule.transform.position = cameraOutOfBounds;
      }
   }

   void checkIfTargeting()
   {
      if (this.thePlayer.gameObject.GetComponent<Girl>().target != null)
      {
         isTargeting = true;
      }
      else
      {
         isTargeting = false;
      }
   }
}
