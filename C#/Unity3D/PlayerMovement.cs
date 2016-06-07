
//==================================================================================
// Author:  Nathan Contreras
//    PlayerMovement class contains all necessary functions in order to provide
//    the player with the ability to move around the game world, via walking or
//    jumping.
//==================================================================================

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public bool isFacingRight;
    public bool isGrounded = false;

    public float moveSpeed;
    public float jumpForce;

	public bool freeze;

    private float myActualSpeed;

    private Rigidbody2D myRigidBody;
    private BoxCollider2D myBoxCollider;
    private Animator myAnim;


    //AUDIOOOO clayton
    public AudioClip jumpSound;
    private AudioSource source;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;

    public float dTime;


    //==================================================================================
    // Use this for initialization
    void Start()
    {
        //clayton
		freeze=false;

        source = GetComponent<AudioSource>();
        if (moveSpeed == 0)
        {
            moveSpeed = 5;
        }
        if (myActualSpeed == 0)
        {
            updateSpeeds();
        }

        if (myAnim == null)
        {
            myAnim = this.GetComponent<Animator>();
        }

        if (myRigidBody == null)
        {
            myRigidBody = this.GetComponent<Rigidbody2D>();
        }

        if (myBoxCollider == null)
        {
            myBoxCollider = this.GetComponent<BoxCollider2D>();
        }
    }
	
    //==================================================================================
    // Update is called once per frame
    void Update()
    {
        updateCalls();
    }

    //==================================================================================
    // Calls all necessary update calls needed by this script
    void updateCalls()
    {
        updateSpeeds();
        checkJump();
        movePlayer();
    }
        
    //==================================================================================
    // Checks if the player is grounded
    void checkJump()
    {
        if (!isGrounded)
        {
            if (this.GetComponent<Rigidbody2D>().velocity.y == 0)
            {
                isGrounded = true;
            }
        }
    }

    //==================================================================================
    // Updates the current movespeed
    void updateSpeeds()
    {
        myActualSpeed = moveSpeed * Time.deltaTime;
    }

    //==================================================================================
    // Flip the player sprite when he begins moving in the opposite direction
    void flipPlayerSprite()
    {
        Vector3 myScale = this.transform.localScale;
        this.transform.localScale = new Vector2(myScale.x * -1, myScale.y);
    }

    //==================================================================================
    // Moves the player around
    void movePlayer()
	{
	    if (!freeze) 
        {
		    if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) 
            {
			    if (!isFacingRight) 
                {
				    flipPlayerSprite ();
				    isFacingRight = !isFacingRight;
			    }
			    myAnim.SetBool ("isMoving", true);
			    this.transform.Translate (Vector2.left * myActualSpeed);
		    } 
            else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) 
            {
			    if (isFacingRight) 
                {
				    flipPlayerSprite ();
				    isFacingRight = !isFacingRight;
			    }
			    myAnim.SetBool ("isMoving", true);
			    this.transform.Translate (Vector2.right * myActualSpeed);
		    } 
            else
            {
			    myAnim.SetBool ("isMoving", false);
		    }

		    if (((Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.Space)) && !freeze)
		        && isGrounded == true) 
            {
			    myAnim.SetTrigger ("isJumping");
			    isGrounded = false;
			    myRigidBody.AddForce (Vector2.up * jumpForce);

			    //JUMP AROUND clayton
			    float vol = Random.Range (volLowRange, volHighRange);
			    source.PlayOneShot (jumpSound, vol);
			    //GameObject.FindGameObjectWithTag("Player").GetComponent<scoreHandler>().score++;
		    } 
            else if ((Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) && !freeze) 
            {
			    //todo: something?
		    }

		    if (Input.GetKeyDown (KeyCode.Delete)) 
            {
			    myAnim.SetTrigger ("isExploding");
		    }
	    }
    }
}
