﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Bullet : MonoBehaviour {
	private LineRenderer line;
	private Vector3 oldPos;
	private float lifeTime = 0.0f;
	public float maxLife = 1.5f;
	private Vector3 velocity;//251 m/s
	private int segment = 1;

	public void setVelocity(Vector3 newVel){
		velocity = newVel;
	}
	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
		line.SetPosition(0,transform.position);
		line.SetPosition(1,transform.position);
		oldPos = transform.position;
		Destroy (gameObject,maxLife);
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime+=Time.deltaTime;
		//raycast; did we hit anything?
		RaycastHit hit;
		transform.position+=velocity*Time.deltaTime;
		if(Physics.Linecast(oldPos,transform.position,out hit)){
			hit.collider.gameObject.SendMessage("onHit",SendMessageOptions.DontRequireReceiver);
			Destroy(gameObject);
		}
		line.SetVertexCount(segment+1);
		line.SetPosition(segment,transform.position);
		segment++;
		//next...
//		//adjust the trail
//		for(int i = 0; i<segment;i++){
//			Color start = new Color(1,1,1,(1.0 - lifeTime * 5.0)*0.05);
//			Color end = new Color(1,1,1,5.0*0.05);
//		}

	}
}
