using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public bool Fire = true;
	public GameObject playerc;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		CanSeePlayer (playerc);
	//	print (CanSeePlayer (playerc));
	}

	//from http://forum.unity3d.com/threads/raycasting-a-cone-instead-of-single-ray.39426/
	bool CanSeePlayer(GameObject target){

		float heightofPlayer = 0.5f;

		Vector3 startVector = transform.position;
		startVector.y += heightofPlayer;
		Vector3 forwardVector = transform.forward;
		forwardVector.y = heightofPlayer;

		RaycastHit hit;
		Vector3 rayDirection = target.transform.position - startVector;

		// If the ObjectToSee is close to this object and is in front of it, then return true
		if ((Vector3.Angle(rayDirection, forwardVector)) < 110 &&
			(Vector3.Distance(startVector, target.transform.position) <= 5f))
		{
			//Create Bullet using the Prefab at X/Y Location
			var bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
			//Move Bullet forward using Rigidbody Velocity
			bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * 6;
			//Destroy Bullet after 2 seconds.
			Destroy (bullet, 0.4f);


			//Debug.Log("close");
			return true;
		}
		if ((Vector3.Angle(rayDirection, forwardVector)) < 90 &&
			Physics.Raycast(startVector, rayDirection, out hit, 25f))
		{ // Detect if player is within the field of view

			if (hit.collider.gameObject == target)
			{
			//	Debug.Log("Can see player");
				//Create Bullet using the Prefab at X/Y Location
				var bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
				//Move Bullet forward using Rigidbody Velocity
				bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * 6;
				//Destroy Bullet after 2 seconds.
				Destroy (bullet, 0.4f);


				return true;
			}
			else
			{
			//	Debug.Log("Can not see player");
				return false;
			}
		}
		return false;
	}
		
	}
