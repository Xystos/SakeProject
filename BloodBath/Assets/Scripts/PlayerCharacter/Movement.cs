using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Photon.MonoBehaviour {

	public float health = 100.0f;

	Vector3 TargetPosition;
	public float duration = 50.0f;
	public float yAxis = 1.0f;
	public bool movement = false;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public GameObject MovementFeedback;
	public GameObject SpawnedMovementFB;

	// Use this for initialization
	void Start () {
		TargetPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (photonView.isMine == false && PhotonNetwork.connected == true) {
			return;
		}
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				TargetPosition = hit.point;
				TargetPosition.y = yAxis;
				movement = true;
				SpawnedMovementFB = PhotonNetwork.Instantiate (this.MovementFeedback.name, TargetPosition, Quaternion.identity, 0);
			}
		}
		//Destroy Movement Feedback to the Player
		if (Input.GetMouseButtonUp (0)) {
			PhotonNetwork.Destroy (SpawnedMovementFB);
		}

		if (movement == true && transform.position != TargetPosition) {
			transform.position = Vector3.Lerp(transform.position, TargetPosition, 1/(duration*(Vector3.Distance(transform.position, TargetPosition))));
			transform.LookAt (TargetPosition);
		}
		else if (movement == true && transform.position == TargetPosition) {
			movement = false;
		}

		if(Input.GetKeyDown(KeyCode.A)){
			Attack();
		}
	}
	
	void Attack() {
		//Create Bullet using the Prefab at X/Y Location
		var bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		//Move Bullet forward using Rigidbody Velocity
		bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * 6;
		//Destroy Bullet after 2 seconds.
		Destroy (bullet, 0.4f);
	}
}
