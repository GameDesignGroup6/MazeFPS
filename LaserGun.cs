using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class LaserGun : Weapon {
	//	public Transform muzzle;
	private LineRenderer line;
	public AudioClip fireSound;
	private int beamTime = 0;
	public int maxBeamTime = 10;
	
	// Use this for initialization
	void Start () {
		line = GetComponentInChildren<LineRenderer>();
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
		GetComponent<AudioSource>().PlayOneShot(fireSound);
		Vector3 fwd = muzzle.TransformDirection(Vector3.forward);
		RaycastHit hit;
		if(Physics.Raycast(muzzle.position,fwd,out hit)){
			hit.collider.gameObject.SendMessage("onHit");
			line.SetPosition(1,transform.InverseTransformPoint(hit.point));
			line.enabled = true;
			beamTime = maxBeamTime;
		}
		
	}
	public override void PutAway ()
	{
//		throw new System.NotImplementedException ();
	}

}
