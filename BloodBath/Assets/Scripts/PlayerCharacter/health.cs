using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : Photon.PunBehaviour {

	public float maxHealth = 100.0f;
	public float currentHealth;
	public float percentage = 1.0f;
	public GameObject HealthBar;

	public void TakeDamage(int amount)
	{
		currentHealth -= amount;
		percentage = currentHealth/maxHealth;
		if (currentHealth <= 0)
			{
			currentHealth = 0;
			}
		var HealthChange = HealthBar.GetComponent<GUIBarScript>();
		if (HealthChange != null) {
			HealthChange.SetNewValue (percentage);
		}


	}
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
	}

	//Method called when Collided with a "Beam" collides
	void OnTriggerEnter(Collider other){

		if (!photonView.isMine) {
			return;
		}

		if (!other.name.Contains ("Beam")) {
			return;
		}

		currentHealth -= 10.0f;
	}


	// Update is called once per frame
	void Update () {
		if (currentHealth == 0) {
			Destroy (gameObject);
			GameManager.Instance.LeaveRoom ();
		}
	}
}
