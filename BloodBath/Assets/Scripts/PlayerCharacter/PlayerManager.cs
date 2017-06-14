using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Photon.PunBehaviour, IPunObservable {

	public GameObject Bullets;
	public Transform bulletSpawn;
	bool IsFiring;

	public float MaxHealth = 100f;
	public float CurrentHealth;
	public float percentageHealth;
	public GameObject HealthBar;

	public static GameObject LocalPlayerInstance;
	public GameObject PlayerUiPrefab;

	public Vector3 CharPos;
	public float CharHeight;
	public Transform CharTransform;

	void Awake(){
		//Used in GameManager.cs - keep track of local player
		if (photonView.isMine) {
			PlayerManager.LocalPlayerInstance = this.gameObject;
		}
		DontDestroyOnLoad (this.gameObject);
	}


	// Use this for initialization
	void Start () {
		CurrentHealth = MaxHealth;
		CharPos = this.gameObject.transform.position;
		CharHeight = 10.0f;
		CharTransform = this.gameObject.transform;

	}
	
	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {
			ProcessInputs ();
		}
			
		CharPos = this.gameObject.transform.position;
		CharHeight = 10.0f;
		CharTransform = this.gameObject.transform;

		percentageHealth = CurrentHealth / MaxHealth;
		var HealthChange = HealthBar.GetComponent<GUIBarScript>();
		if (HealthChange != null) {
			HealthChange.SetNewValue (percentageHealth);
		}
		if ( CurrentHealth <= 0f)
		{
			GameManager.Instance.LeaveRoom();
		}

		if (Bullets != null && IsFiring != Bullets.GetActive ()) {
			var bullet = (GameObject)Instantiate (Bullets, bulletSpawn.position, bulletSpawn.rotation);
			//Move Bullet forward using Rigidbody Velocity
			bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * 6;
			//Destroy Bullet after 2 seconds.
			Destroy (bullet, 0.4f);
		}
	}

	void ProcessInputs(){
		if (Input.GetKeyDown (KeyCode.A)) {
			IsFiring = true;
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			IsFiring = false;
		}
	}

	void OnTriggerEnter(Collider other){

		Debug.Log ("Trigger Happened");
		if (!photonView.isMine) {
			Debug.Log ("PhotonViewISMINE");
			return;
		}

		//if (!other.name.Contains ("bullet")) {
		//	return;
		//}

		CurrentHealth -= 10f;
		Debug.Log ("The Current Health is " + CurrentHealth);

	}

	void CalledOnLevelWasLoaded(){
		GameObject _uiGo = Instantiate(this.PlayerUiPrefab) as GameObject;
		_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
	}
		
	#region IPunObservable implementation
	void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (IsFiring);
		} else {
			this.IsFiring = (bool)stream.ReceiveNext ();
		}
		if (stream.isWriting) {
			//We own this player: send others our data
			stream.SendNext (IsFiring);
			stream.SendNext (CurrentHealth);
		} else {
			this.IsFiring = (bool)stream.ReceiveNext ();
			this.CurrentHealth = (float)stream.ReceiveNext ();
		}
	}
	#endregion
}
