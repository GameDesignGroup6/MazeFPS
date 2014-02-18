using UnityEngine;
using System.Collections;

public class bouncySpinnyCubeScript : MonoBehaviour {
	private Vector3 initPos;

	public float bobHeight;
	public float bobRate;
	public Vector3 spinRate;

	private float offset;
	

	// Use this for initialization
	void Start () {
		initPos = transform.position;
		offset = Random.Range(0f,360f);
//		initRot = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		float y = initPos.y+Mathf.Sin(Time.time*bobRate+offset)*bobHeight;
		Vector3 oldPos = transform.position;
		oldPos.y = y;
		transform.position = oldPos;
		Vector3 newAngle = transform.eulerAngles;
		newAngle.x = Mathf.Repeat(Time.time*spinRate.x+offset,360);
		newAngle.y = Mathf.Repeat(Time.time*spinRate.y+offset,360);
		newAngle.z = Mathf.Repeat(Time.time*spinRate.z+offset,360);
		transform.eulerAngles = newAngle;
	}
}
