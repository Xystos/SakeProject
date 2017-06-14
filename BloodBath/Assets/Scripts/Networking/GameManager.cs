using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour {

	static public GameManager Instance;
	public GameObject playerCharacterPreFab;

	// Use this for initialization
	void Start () {
		Instance = this;
		if (playerCharacterPreFab == null) {
			Debug.Log ("Missing Player Character");
		} else {
			if (PlayerManager.LocalPlayerInstance == null) {
				playerCharacterPreFab = PhotonNetwork.Instantiate (this.playerCharacterPreFab.name, new Vector3 (0f, 5f, 0f), Quaternion.identity, 0);
			} else {
				Debug.Log ("Ignoring Scene Load");
			}
		}
	}

	//Called when the local player left the room
	public void OnLeftRoom(){

		SceneManager.LoadScene (0);

	}

	public void LeaveRoom(){

		PhotonNetwork.LeaveRoom ();
	}

	void LoadArena(){

		if (!PhotonNetwork.isMasterClient) {
			Debug.LogError ("Not Masterclient; Level Load failed");
		}
		Debug.Log ("Loading Level...");
		PhotonNetwork.LoadLevel ("Bloodbath");
	}

	public override void OnPhotonPlayerConnected(PhotonPlayer other){
		Debug.Log ("Player has connected - " + other.NickName);

		if (PhotonNetwork.isMasterClient) {
			LoadArena();
		}
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer other){
		Debug.Log("Player has disconnected - " + other.NickName);
		
			if(PhotonNetwork.isMasterClient){
				LoadArena();
			}
	}


	// Update is called once per frame
	void Update () {
		
	}
}
