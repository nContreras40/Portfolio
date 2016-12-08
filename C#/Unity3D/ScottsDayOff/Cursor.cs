
//==================================================================================
// Author:  Nathan Contreras
//    Cursor class allows user to move the aiming cursor around the screen in
//    order to allow the player some freedom in aiming.  
//    Cursor positions are empty game objects that are children of the player game
//    object, allowing easy calculation of the cursor location.
//==================================================================================

using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

    public GameObject player;

    //==================================================================================
	// Use this for initialization
	void Start () {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        this.transform.position = GameObject.FindGameObjectWithTag("Cursor_R").transform.position;
	}
	
    //==================================================================================
	// Update is called once per frame
	void Update () {
        moveCursorPos();
	}

    //==================================================================================
    // Moves the cube around the screen
    void moveCursorPos()
    {
        if (!player.GetComponent<PlayerMovement>().isFacingRight)
        {
            if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.I))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_T").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_T").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.L))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_R").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_R").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.J))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_L").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_L").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.O))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_TR").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_TR").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.U))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_TL").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_TL").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Period))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_BR").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_BR").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.M))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_BL").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_BL").transform.rotation;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.I))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_T").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_T").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.L))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_L").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_L").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.J))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_R").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_R").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.O))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_TL").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_TL").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.U))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_TR").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_TR").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Period))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_BL").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_BL").transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.M))
            {
                this.transform.position = GameObject.FindGameObjectWithTag("Cursor_BR").transform.position;
                this.transform.rotation = GameObject.FindGameObjectWithTag("Cursor_BR").transform.rotation;
            }
        }
    }
}
