using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOut : MonoBehaviour
{

   public float fadeInTime;
   public float fadeOutWaitTime;
   public float fadeOutTime;

   private float timer = 0.0f;

   private bool isFadingIn = false;
   private bool isWaiting = false;
   private bool isFadingOut = false;

   private Image myImage;

   // Use this for initialization
   void Start()
   {
      this.myImage = this.GetComponent<Image>();
      this.isFadingIn = true;
   }

   // Update is called once per frame
   void Update()
   {
      if (this.isFadingIn)
      {
         this.timer += Time.deltaTime;

         float interpolator = this.timer / this.fadeInTime;

         Color temp = this.myImage.color;
         temp.a = Mathf.Lerp(temp.a, 0, interpolator);
         this.myImage.color = temp;

         if (interpolator >= 0.99f)
         {
            this.isFadingIn = false;
            this.isWaiting = true;
            this.timer = 0.0f;
         }
      }
      if (this.isWaiting)
      {
         this.timer += Time.deltaTime;
         if (this.timer >= this.fadeOutWaitTime)
         {
            this.isWaiting = false;
            this.isFadingOut = true;
            this.timer = 0.0f;
         }
      }
      if (this.isFadingOut)
      {
         timer += Time.deltaTime;

         float interpolator = this.timer / this.fadeInTime;

         Color temp = this.myImage.color;
         temp.a = Mathf.Lerp(temp.a, 1, interpolator);
         this.myImage.color = temp;

         if (interpolator >= 0.99f)
         {
            this.isFadingIn = false;
            this.isWaiting = false;
            this.timer = 0.0f;
         }
      }
   }
}
