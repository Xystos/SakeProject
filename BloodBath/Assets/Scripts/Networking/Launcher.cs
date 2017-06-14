using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : Photon.PunBehaviour {

	/// </summary>
	/// This client's version number
	string gameVersion = "1";
	public byte MaxPlayersPerRoom = 4;

	/// <summary>
	/// The PUN loglevel
	/// </summary>
	public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
	public GameObject Textbox;
	bool isConnecting;

	void Awake(){

		//Don't join Lobby as we need to find the rooms
		PhotonNetwork.autoJoinLobby = false;

		// Ensures we can use LoadLevel function correctly and players are synced
		PhotonNetwork.automaticallySyncScene = true;

		///Not important but starts the Log Level
		PhotonNetwork.logLevel = Loglevel;
	}

	// Use this for initialization
	void Start () {
		Textbox.SetActive (false);
	}

	//If connected we join a room - if not, we connect to Photon
	public void Connect(){
		isConnecting = true;
		Textbox.SetActive (true);
		if (PhotonNetwork.connected) {
			PhotonNetwork.JoinRandomRoom();
		} else {
			PhotonNetwork.ConnectUsingSettings (gameVersion);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnConnectedToMaster(){
		if (isConnecting) {
			Debug.Log ("Master Server was successfully connected");
			PhotonNetwork.JoinRandomRoom ();
		}
	}

	public override void OnDisconnectedFromPhoton(){
		Textbox.SetActive (false);
		Debug.Log ("Disconnected from Photon");
	}

	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg){
		//If Joining a Random Room Failed, Create a new one
		Debug.Log ("Join Random Room Failed! Room is being created!");
		PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
	}
		
	public override void OnJoinedRoom(){
		Debug.Log ("You have joined the room");

		PhotonNetwork.LoadLevel ("Bloodbath");
	}
}
