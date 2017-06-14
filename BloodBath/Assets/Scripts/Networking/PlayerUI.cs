using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public Text PlayerNameText;

	public Slider PlayerHealthSlider;

	public Vector3 ScreenOffset = new Vector3(0f,30f,0f);

	PlayerManager _target;

	float _characterControllerHeight = 0f;
	Transform _targetTransform;
	Vector3 _targetPosition;

	public void SetTarget(PlayerManager target){
		if (target == null) {
			Debug.LogError ("Manager not found");
			return;
		}
		_target = target;
		Debug.Log ("The Target is... " + target);
		Debug.Log ("We got the Target... " + _target);
		_targetPosition = _target.CharPos;
		_targetTransform = _target.CharTransform;
		_characterControllerHeight = _target.CharHeight;
		Debug.Log ("The Target Position is " + _targetPosition);
		Debug.Log ("The Target Transform is " + _targetTransform);
		Debug.Log ("The Target Height is " + _target.CharHeight);
		//CharacterController _characterController = _target.GetComponent<CharacterController> ();
		//Get data from the player that won't change for the lifetime
		//if (_characterController != null) {
		//	_characterControllerHeight = _characterController.height;
		//	_targetTransform = _characterController.transform;
		//	_targetPosition = _characterController.transform.position;
	//	}
		if (PlayerNameText != null) {
			PlayerNameText.text = _target.photonView.owner.name;
		}
	}
	void Awake(){
		this.GetComponent<Transform>().SetParent (GameObject.Find("Canvas").GetComponent<Transform>());
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerHealthSlider != null) {
			PlayerHealthSlider.value = _target.percentageHealth;
		}

		if (_target == null) {
			Destroy (this.gameObject);
			return;
		}
	}

	void LateUpdate() {
		//Follow the Target GameObject on Screen.
		if (_targetTransform != null) {
			Debug.Log ("Working In Late Update");
			_targetPosition = _targetTransform.position;
			_targetPosition.y += _characterControllerHeight;
			this.transform.position = _targetPosition;
		}
	}
}
