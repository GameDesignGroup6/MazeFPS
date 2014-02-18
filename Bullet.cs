using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Bullet : MonoBehaviour {
	private LineRenderer line;
	private Vector3 oldPos;
	private float lifeTime = 0.0f;
	public float maxLife = 1.5f;
	private Vector3 velocity;//251 m/s
	private int segment = 1;

	public AudioClip[] hitEnemySounds,hitWallSounds;

	private AudioSource source;

	public void setVelocity(Vector3 newVel){
		velocity = newVel;
	}
	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
		source = GetComponent<AudioSource>();
		line.SetPosition(0,transform.position);
		line.SetPosition(1,transform.position);
		oldPos = transform.position;
		Destroy (gameObject,maxLife);
	}

	private void playOneSound(AudioClip[] list){
		int chosen = Random.Range(0,list.Length-1);
		source.PlayOneShot(list[chosen]);
	}

	// Update is called once per frame
	void Update () {
		lifeTime+=Time.deltaTime;
		//raycast; did we hit anything?
		RaycastHit hit;
		transform.position+=velocity*Time.deltaTime;
		if(Physics.Linecast(oldPos,transform.position,out hit)){
			hit.collider.gameObject.SendMessage("onHit",SendMessageOptions.DontRequireReceiver);
			if(hit.collider.gameObject.tag == "Enemy"){
				Debug.Log ("Hit enemy!");
				playOneSound(hitEnemySounds);
			}else{
				Debug.Log ("Hit wall!");
				playOneSound(hitWallSounds);
			}
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
