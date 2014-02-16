using UnityEngine;
using System.Collections;

public class CrossHairScript : MonoBehaviour {
	private static CrossHairScript instance;
	private Transform firePoint;
	// Use this for initialization
	void Awake () {
		instance = this;
	}

	void LateUpdate(){
		if(firePoint==null)return;

		Vector3 fwd = firePoint.forward;
		RaycastHit hit;
		if(Physics.Raycast(firePoint.position,fwd,out hit)){
			if(hit.collider==null)return;
			transform.position = Camera.current.WorldToViewportPoint(hit.point);
		}
	}

	
	public static void updateCrosshair(Transform firePoint){
		instance.firePoint = firePoint;
	}
}
