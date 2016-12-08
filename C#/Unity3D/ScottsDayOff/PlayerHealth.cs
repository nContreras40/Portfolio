
//============================================================
// Authors:  Nathan Contreras & Clayton Wells
// Purpose:
//    Allow the player to have a dynamic health bar displayed
//    on the UI canvas in the game.
//============================================================

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    public int myHealth;
    private GameObject mySlider;
    private Animator myAnim;
    private Image mySliderFillColor;

    //audioooo
    public AudioClip scottDeathSound;
    public AudioClip gameOverSound;
    public AudioClip hurtSound;
    private AudioSource source;
    private float volLowRange = 0.6f;
    private float volHighRange = 1.2f;


    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        myHealth = 42;
        GetComponent<Rigidbody2D>().isKinematic = false;
        mySlider = GameObject.FindGameObjectWithTag("UI_Health_Slider");
        mySliderFillColor = GameObject.FindGameObjectWithTag("UI_Health_FillColor").GetComponent<Image>();
        myAnim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        normalizeHealth();
        mySlider.GetComponent<UIHealthSlider>().setHealthSliderValue(myHealth);
        modifyColors();
    }

    //health changing helper. Triggers invincibility kind of correctly.
    public void changeHealth(int value)
    {
        //if we're invincible, don't do anything (for now)
        if (GetComponent<invFrames>().inv)
        {
            //Debug.Log ("I tried to lose health, but Player was inv");
        }
        else if ((value < 0)) //if its damage, do the damage
        { 
            myHealth += value; //change health
            GetComponent<invFrames>().trigger(); //trigger default iframes
            float vol = Random.Range(volLowRange, volHighRange); //randomize vol
            source.PlayOneShot(hurtSound, vol); // play the hurt sound
            checkDeath();
            return;
        }

        //if the input is healing, heal. Works during inv.
        if (value > 0)
            myHealth += value;
    }

    //Ensures health is between 0 and 42
    void normalizeHealth()
    {
        if (myHealth > 42)
        {
            myHealth = 42;
        }
        else if (myHealth < 0)
        {
            myHealth = 0;
        }
    }

    //Modifies the color of the health bar according to the health of the player
    void modifyColors()
    {
        if (myHealth >= 42)
        {
            mySliderFillColor.color = new Color(255, 0, 255);  // blue
        }
        else if (myHealth >= 30)
        {
            mySliderFillColor.color = new Color(0, 255, 255);  // cyan
        }
        else if (myHealth >= 20)
        {
            mySliderFillColor.color = new Color(0, 255, 0);  // green
        }
        else if (myHealth >= 10)
        {
            mySliderFillColor.color = new Color(255, 255, 0);  // yellow
        }
        else if (myHealth >= 1)
        {
            mySliderFillColor.color = new Color(255, 0, 0);  // red
        }
        else if (myHealth == 0)
        {
            mySliderFillColor.color = new Color(0, 0, 0); // black
        }

    }

    //trigger a small death, resulting in restarting the level.
    public IEnumerator smallDeath(float d)
    {
        yield return new WaitForSeconds(d);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //trigger a large death, resulting in returning to the menu.
    public IEnumerator bigDeath(float d)
    {
        yield return new WaitForSeconds(d);
        SceneManager.LoadScene(0);
    }

    // checks if the player has died
    public void checkDeath()
    {
        if (this.myHealth <= 0)
        {
            myAnim.SetTrigger("isExploding");
            PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives") - 1);

            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<PlayerMovement>().freeze = true;

            if (PlayerPrefs.GetInt("Lives") > 0)
            {
                GameObject.FindGameObjectWithTag("Sound_Engine").GetComponent<soundEngine>().oneShotNonOverlap(scottDeathSound, 1.0f);
                StartCoroutine(smallDeath(1.0f));
            }
            else
            {
                GameObject.FindGameObjectWithTag("Sound_Engine").GetComponent<soundEngine>().oneShotNonOverlap(gameOverSound, 1.0f);
                StartCoroutine(bigDeath(2.0f));
            }
        }
    }
}
