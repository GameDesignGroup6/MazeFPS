using UnityEngine;
using System.Collections;

public class LaserGun : Weapon {
	public Transform FirePoint;
	private LineRenderer line;
	private int beamTime = 0;
	public int maxBeamTime = 10;

	// Use this for initialization
	void Start () {
		line = GetComponentInChildren<LineRenderer>();
		if(line==null){
			Debug.LogError("No line renderer on laser!?");
			return;
		}
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
	}
}
