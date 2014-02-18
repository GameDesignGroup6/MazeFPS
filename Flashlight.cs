using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Light))]
[RequireComponent(typeof(Rigidbody))]
public class Flashlight : MonoBehaviour {

	public Sprite[] frames;

	public int maxBattery = 1000;
	public int batteryRemaining = 1000;

	public SpriteRenderer sprite;


	private bool thrown = false;
	private bool switchedOn = true;

	public float throwVelocity = 2f;
	public Transform GunPoint;

	public Light otherLight;
	
	public bool lockPos = false;
	public bool autoLock = true;
	
	//rotation velocities
	private float rotXVel,rotYVel,rotZVel,rotWVel;
	
	private Vector3 posVel;
	
	private const float smoothPosTime = 0.1f;
	private const float smoothRotTime = 0.1f;
	private const float maxDegreesDelta = 20f;
	private const float maxDistDelta = 0.05f;

	private AudioSource source;

	public AudioClip switchOnSound,switchOffSound;

	void Start(){
		source = GetComponent<AudioSource>();
		source.PlayOneShot(switchOnSound);
	}

	void FixedUpdate(){
		if(switchedOn&&batteryRemaining>0){
			batteryRemaining--;
		}
//		Debug.Log (batteryRemaining);
//		Debug.Log (batteryRemaining);
		if(batteryRemaining==0)switchedOn = false;

		float batPercent = (float)batteryRemaining/(float)maxBattery;
		int frame = (frames.Length-1)-(Mathf.RoundToInt(batPercent*(frames.Length-1)));
		if(batteryRemaining==0)frame = frames.Length-1;
		sprite.sprite = frames[frame];

	}


	void LateUpdate () {
		if(batteryRemaining>0){
			GetComponentInChildren<Light>().enabled = true;
				if(otherLight!=null&&!thrown)otherLight.enabled = true;
		}else{
			GetComponentInChildren<Light>().enabled = false;
			if(otherLight!=null&&!thrown)otherLight.enabled = false;
		}

		if(!thrown){
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
			
//			Debug.Log (posVel.ToString());
			
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

			if(!lockPos&&autoLock && Vector3.Distance(newPos,targetPos)<0.05f){
				lockPos = true;
			}


//			if(Input.GetKeyDown(KeyCode.F))ThrowFlashlight();
		}

	}

	public void ThrowFlashlight(){
		if(batteryRemaining<=0){
			source.PlayOneShot(switchOffSound);
		}
		otherLight.enabled = false;
		collider.enabled = true;
		Physics.IgnoreCollision(Player.GetInstance().collider,collider);
//		rigidbody.velocity = forward;
		thrown = true;
		rigidbody.isKinematic = false;
		rigidbody.constraints = RigidbodyConstraints.None;
		rigidbody.AddForce(transform.forward.normalized*2f);
		rigidbody.AddTorque(Random.onUnitSphere*5);
	}
}
