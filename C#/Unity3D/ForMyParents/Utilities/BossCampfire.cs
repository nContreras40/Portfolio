using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossCampfire : MonoBehaviour
{
	public float healSpeed = 15f;
	public Transform respawnPoint;
	public Transform noLivesRespawn;
	public ParticleSystem fire;

	public UnityEngine.Events.UnityEvent onPlayerDeath;
	public UnityEngine.Events.UnityEvent onPlayerFinalDeath;

	private int revivesLeft = 2;
	private bool lifeTimerActive = false;
	private float timer = 0.0f;


	//==========================================================================
	// Use this for initialization
	void Awake ()
	{
		if (respawnPoint == null) {
			respawnPoint = this.GetComponentInChildren<Transform> ();
		}
	}

	//==========================================================================
	// Update is called once per frame
	void Update ()
	{
//		if (this.revivesLeft == 0) {
//			this.respawnPoint.position = this.noLivesRespawn.position;
//			this.fire.Stop ();
//		}

		//if ( !this.lifeTimerActive && StatusManager.getInstance().health < 1 )
		//{
		//   decreaseRevives();
		//}
		//else if (this.lifeTimerActive)
		//{
		//   this.timer += Time.deltaTime;
		//   if (this.timer >= 2.5f)
		//   {
		//      this.lifeTimerActive = false;
		//   }
		//}
	}

	//==========================================================================
	// When the player enters the trigger zone, set to current spawn point
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.name.Equals ("Kira")) {
			Debug.Log ("Respawn point changed");
			StatusManager.getInstance ().respawnPoint = new Vector3 (this.respawnPoint.position.x,
				this.respawnPoint.position.y + 2,
				this.respawnPoint.position.z);
			//GameObject.FindGameObjectWithTag("HealthManager").GetComponent<HealthPlayer>().campfireRespawn = this.respawnPoint;
			if (StatusManager.getInstance ().hasStatus (StatusManager.Status.FEAR)) {
				Debug.Log ("ping");
				StatusManager.getInstance ().ToggleStatus (StatusManager.Status.FEAR);
			}
			//GameObject.FindGameObjectWithTag("FearManager").GetComponent<StatusManager>().ToggleStatus();


		}
		if (StatusManager.getInstance ().health < 1) 
		{
			if (revivesLeft < 0) 
			{
				StatusManager.getInstance().health = 100;
				onPlayerFinalDeath.Invoke ();
			} else 
			{
				print (revivesLeft);
				revivesLeft--;
				onPlayerDeath.Invoke ();
			}
			//StatusManager.getInstance().health = 100;
		}
	}

	//==========================================================================
	// While the player stays in the trigger zone, heal the player.
	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.name.Equals ("Kira")) {
			healPlayer ();
		}
	}

	//==========================================================================
	//
	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.name.Equals ("Kira")) {
			if (!StatusManager.getInstance ().hasStatus (StatusManager.Status.FEAR)) {
				StatusManager.getInstance ().ToggleStatus (StatusManager.Status.FEAR);
			}
		}
	}

	//==========================================================================
	// heals the player
	void healPlayer ()
	{
		StatusManager.getInstance ().health += healSpeed * Time.deltaTime;
		StatusManager.getInstance ().fear -= healSpeed * Time.deltaTime;
	}

	//==========================================================================
	// Remove a life from the player
	public void decreaseRevives ()
	{
		if (!this.lifeTimerActive) {
			this.revivesLeft--;
			this.lifeTimerActive = true;
		}
	}

	//==========================================================================
	// Returns the number of lives remaining
	public int getRevivesRemaining ()
	{
		return this.revivesLeft;
	}
}
