using UnityEngine;
using System.Collections;

/**
 * This class represents a weapon that can be held by the player.
 * It handles moving the weapon to the proper position and rotation
 * in game.  It also ensures that all weapons have a Fire() method.
 * 
 * NOTE: child classes MUST NOT have an LateUpdate() method.  Override UpdateCallback() instead!
 * 
 * 
 **/ 
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Weapon : MonoBehaviour {
	public Transform GunPoint;
	public Transform muzzle;

	public bool lockPos = true;

	//rotation velocities
	private float rotXVel,rotYVel,rotZVel,rotWVel;

	private Vector3 posVel;

	private bool detatched = false;

	private const float smoothPosTime = 0.1f;
	private const float smoothRotTime = 0.1f;
	private const float maxDegreesDelta = 20f;
	private const float maxDistDelta = 0.05f;


	// Use this for initialization
	void Start () {
		rigidbody.isKinematic = true;
		collider.enabled = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(detatched)return;
		//first define our targets
		Quaternion curRot = transform.rotation;
		Quaternion targetRot = GunPoint.rotation;
		Vector3 curPos = transform.position;
		Vector3 targetPos = GunPoint.position;
		Quaternion newRot = Quaternion.identity;
		Vector3 newPos = Vector3.zero;

		//smooth towards target
		if(!lockPos)
		newPos = Vector3.SmoothDamp(curPos,targetPos,ref posVel,smoothPosTime);

//		Debug.Log (posVel.ToString());

		newRot.w = Mathf.SmoothDampAngle(curRot.w,targetRot.w,ref rotWVel,smoothRotTime);
		newRot.x = Mathf.SmoothDampAngle(curRot.x,targetRot.x,ref rotXVel,smoothRotTime);
		newRot.y = Mathf.SmoothDampAngle(curRot.y,targetRot.y,ref rotYVel,smoothRotTime);
		newRot.z = Mathf.SmoothDampAngle(curRot.z,targetRot.z,ref rotZVel,smoothRotTime);

		//fix them
		float deltaAngle = Quaternion.Angle(targetRot,curRot);
		if(deltaAngle>maxDegreesDelta){
			//too fast!
			float percentWhereMax = maxDegreesDelta/deltaAngle;
			newRot = Quaternion.Slerp(targetRot,newRot,percentWhereMax);
		}
		float deltaPos = Vector3.Distance(targetPos,curPos);
		if(!lockPos&&deltaPos>maxDistDelta){
			float percentWhereMaxPos = maxDistDelta/deltaPos;
			newPos = Vector3.Slerp(targetPos,curPos,percentWhereMaxPos);
//			Vector3.MoveTowards(targetPos,newPos,maxDistDelta);
		}
		if(lockPos)newPos = targetPos;

		//store new values
		transform.position = newPos;
		transform.rotation = newRot;
		//huh... "position" and "rotation" have the same number of letters...makes my code look nice!


		if(Input.GetButtonDown("Fire1")&&!Player.Dead)Fire();
		UpdateCallback();
	}

	public static Vector3 getAimPoint(){
		Camera cam = Camera.main;
		Ray ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth/2,cam.pixelHeight/2,0.0f));
		RaycastHit hit;
		Physics.Raycast(ray,out hit);
		return hit.point;
	}
	public void detatch(){
		collider.enabled = true;
		rigidbody.isKinematic = false;
		rigidbody.constraints = RigidbodyConstraints.None;
		detatched = true;
		rigidbody.AddForce(transform.forward.normalized);
		rigidbody.AddTorque(Random.onUnitSphere*5);
	}

	public abstract void Fire();
	public abstract void UpdateCallback();
	public abstract void PutAway();
}
