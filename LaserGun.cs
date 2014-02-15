using UnityEngine;
using System.Collections;

<<<<<<< HEAD
public class LaserGun : Weapon {
	public Transform FirePoint;
=======
[RequireComponent(typeof(LineRenderer))]
public class LaserGun : Weapon {
//	public Transform muzzle;
>>>>>>> 538689cf11ba6d16b24e6fd902fe27572f21f8c6
	private LineRenderer line;
	private int beamTime = 0;
	public int maxBeamTime = 10;

	// Use this for initialization
	void Start () {
		line = GetComponentInChildren<LineRenderer>();
<<<<<<< HEAD
		if(line==null){
			Debug.LogError("No line renderer on laser!?");
			return;
		}
=======
>>>>>>> 538689cf11ba6d16b24e6fd902fe27572f21f8c6
	}
	
	// Update is called once per frame
	override public void UpdateCallback () {
		if(beamTime>0){
			beamTime--;
			return;
		}
		if(beamTime==0){
			line.enabled = false;
		}
	}

	override public void Fire(){
		if(beamTime>0)return;
<<<<<<< HEAD
//		Vector3 fwd = transform.TransformDirection(Vector3.forward);
//		RaycastHit hit;
//		Physics.Raycast(transform.position,fwd,out hit);
//		float distance = hit.distance;
//		if(float.IsInfinity(distance))distance=10000f;
		Vector3 pos = Weapon.getAimPoint ();
		pos = transform.InverseTransformPoint(pos);
		line.SetPosition(1,pos);
		line.enabled = true;
		beamTime = maxBeamTime;
=======
		Vector3 fwd = muzzle.TransformDirection(Vector3.forward);
		RaycastHit hit;
		if(Physics.Raycast(muzzle.position,fwd,out hit)){
			hit.collider.gameObject.SendMessage("onHit");
			line.SetPosition(1,transform.InverseTransformPoint(hit.point));
			line.enabled = true;
			beamTime = maxBeamTime;
		}

>>>>>>> 538689cf11ba6d16b24e6fd902fe27572f21f8c6
	}
}
