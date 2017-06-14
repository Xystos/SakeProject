using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float distance = 7.0f;
	public float height = 3.0f;
	public float heightSmoothLag = 0.3f;
	public Vector3 centerOffset = Vector3.zero;
	public bool followOnStart = false;

	Transform cameraTransform;
	bool isFollowing;
	private float heightVelocity = 0.0f;
	private float targetHeight = 100000.0f;



	// Use this for initialization
	void Start () {

		if (followOnStart) {
			OnStartFollowing ();
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (cameraTransform == null && isFollowing) {
			OnStartFollowing ();
		}
		if (isFollowing) {
			Apply (); 
		}		
	}

	public void OnStartFollowing(){
		cameraTransform = Camera.main.transform;
		isFollowing = true;
		Cut ();
	}

	void Apply(){
		Vector3 targetCenter = transform.position + centerOffset;

		//Calculate the current and Target Rotation angles
		float originalTargetAngle = transform.eulerAngles.y;
		float currentAngle = cameraTransform.eulerAngles.y;

		//adjust real target angle when camera is locked
		float targetAngle = originalTargetAngle;

		currentAngle = targetAngle;
		targetHeight = targetCenter.y + height;

		//Damp the height
		float currentHeight = cameraTransform.position.y;
		currentHeight = Mathf.SmoothDamp (currentHeight, targetHeight, ref heightVelocity, heightSmoothLag);

		//Convert the angle into a rotation by which we then reposition the camera
		Quaternion currentRotation = Quaternion.Euler (0, currentAngle, 0);

		//Set the position of teh camer on the x-z plane to X distance meters behind target
		cameraTransform.position = targetCenter;
		cameraTransform.position += currentRotation * Vector3.back * distance;

		//Set the height of the camera
		cameraTransform.position = new Vector3 (cameraTransform.position.x, currentHeight, cameraTransform.position.z);

		//Always look at the target
		SetUpRotation (targetCenter);
	}

	void Cut(){
		float oldHeightSmooth = heightSmoothLag;
		heightSmoothLag = 0.001f;

		Apply ();
		heightSmoothLag = oldHeightSmooth;
	}

	void SetUpRotation(Vector3 centerPos){
		Vector3 cameraPos = cameraTransform.position;
		Vector3 offsetToCenter = centerPos - cameraPos;

		//Generate base rotation around y-axis
		Quaternion yRotation = Quaternion.LookRotation(new Vector3 (offsetToCenter.x, 0, offsetToCenter.z));
		
			Vector3 relativeOffset = Vector3.forward * distance + Vector3.down * height;
			cameraTransform.rotation = yRotation * Quaternion.LookRotation(relativeOffset);
	}
}
