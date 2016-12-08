
//===========================================================
// Author: Nathan Contreras
// Purpose:
//    Script will give the enemy an "AI" of sorts, allowing
//    it to do functions on its own, such as targeting the
//    player and shooting a projectile at the player.
//===========================================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SnakeBossScript : MonoBehaviour {

    public GameObject prefabItem;
    public GameObject exitBox;

    private GameObject player;

    public Slider myHealthSlider;

    float elapsedTime;
    float timeRange;

    int myHealth;

	// Use this for initialization
	void Start () {
        myHealth = 200;
        player = GameObject.FindGameObjectWithTag("Player");

	}
	
	// Update is called once per frame
	void Update () {

        myHealthSlider.value = myHealth;

        if (elapsedTime == 0)
        {
            timeRange = Random.Range(1.0f, 3.0f);
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= timeRange)
        {
            elapsedTime = 0;
            shootProjectile();
        }

        if (myHealth <= 0)
        {
            Destroy(myHealthSlider.gameObject);
            this.gameObject.SetActive(false);
            Instantiate(exitBox, GameObject.FindGameObjectWithTag("SnakeBoss_ExitBox").transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter2D( Collider2D other ) 
    {
        if (other.tag == "PlayerWeapon")
        {
            Destroy(other.gameObject);
            myHealth -= 2;
        }
    }

    void shootProjectile()
    {
        Vector3 playerVector = player.transform.position;
        
        Vector3 vectorToTarget = player.transform.position - GameObject.FindGameObjectWithTag("SnakeBoss_BulletSpawn").transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

		GameObject temp = ((GameObject)Instantiate(prefabItem, GameObject.FindGameObjectWithTag("SnakeBoss_BulletSpawn").transform.position, q));

        Destroy(temp, 1.2f);
    }
}
