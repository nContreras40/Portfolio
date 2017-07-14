//===<Authors>=================================================================
// Created by Nathan Contreras.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===<Requirements>============================================================
// Component Rigidbody2D required for script to be functional.
[RequireComponent(typeof(Rigidbody2D))]

//===<Class>===================================================================
// BasicMovement class is responsible for providing base-line functional
// movement to the player character.
public class BasicMovement : MonoBehaviour
{

   [Tooltip("The layer which will hold collidable floor environments")]
   public LayerMask groundLayer;

   [Tooltip("The speed of the player.")]
   public float playerSpeed = 5.0f;
   [Tooltip("The jump force of the player.")]
   public float jumpForce = 5.0f;

   [Tooltip("The distance we want to check when raycasting to find the ground.")]
   public float lengthCheck = 0.55f;

   void Start()
   {
      if (this.groundLayer == 0)
         throw new System.Exception("[" + this.gameObject.name + "]'s BasicMovement.groundLayer is set to \"nothing\", no jump check can occur."
                                    + "\nCheck this object's inspector and assign a non-nothing value, preferably something within Environment.");
   }


   //=========================================================================
   // Physics calculation every frame to emulate movement of the character
   // based on input from horizontal and vertical axes.
   void FixedUpdate()
   {
      float h = Input.GetAxis("Horizontal");
      float v = 0.0f;

      Debug.DrawLine(this.transform.position, new Vector2(this.transform.position.x, this.transform.position.y - this.lengthCheck));
      RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, this.lengthCheck, groundLayer);

      if (hit)
      {
         v = Input.GetAxis("Vertical");
      }

      Vector2 movement = new Vector2(h * playerSpeed, v * jumpForce);
      this.GetComponent<Rigidbody2D>().velocity = movement;
   }
}
