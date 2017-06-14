using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class PlayerNameInput : MonoBehaviour {

	//Variable for Player Name
	static string playerNamePrefKey = "PlayerName";

	// Use this for initialization
	void Start () {
		string defaultName = "";
		InputField _inputfield = this.GetComponent<InputField> ();
		if(_inputfield != null){
			if(PlayerPrefs.HasKey(playerNamePrefKey)){
				defaultName = PlayerPrefs.GetString(playerNamePrefKey);
				_inputfield.text = defaultName;
			}
		}
		PhotonNetwork.playerName = defaultName;

	}

	//Sets Name for player and saves it for future uses
	void SetPlayersName(string value){
		PhotonNetwork.playerName = value + " ";
		PlayerPrefs.SetString (playerNamePrefKey, value);
	}

	// Update is called once per frame
	void Update () {

	}
}
