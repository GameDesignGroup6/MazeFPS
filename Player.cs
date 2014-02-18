using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private GameObject equipedFlashlight;
	public GameObject flashlightPrefab;

	public float health;

	public Weapon[] weapons;
	private GameObject curWeapon = null;

	public Transform flashlightPoint,gunPoint;

	public Light flashlightAlt;

	private static Player instance;

	private bool dead = false;
	public static bool Dead{
		get{return instance.dead;}
	}

	private AudioSource source;

	public AudioClip[] deathSounds,hurtSounds,footstepSounds;

	public GameObject head;
	public GameObject body;

	private CharacterController controller;

	private Vector3 previousLocation;
	private float distanceTraveled = 0.0f;
	public float stepDistance = 2.0f;

	private bool tooSoon = false;

	public int maxInvulTime = 10;
	private int invulTime;
	
	// Use this for initialization
	void Start () {
//		flashlights=0;
		instance = this;
		switchToWeapon(0);
		source = GetComponentInChildren<AudioSource>();
		controller = GetComponent<CharacterController>();
		previousLocation = transform.position;
		dead = false;
		head.rigidbody.isKinematic = true;
		head.collider.enabled = false;
		body.renderer.enabled = true;
		collider.enabled = true;
		invulTime = maxInvulTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F9)){
			health = 0;
			hurt (0);
		}
		if(dead){
			if(invulTime<=0){
				if(Input.anyKeyDown){
					Application.LoadLevel(0);
				}
			}
			return;
		}
		//grab items off ground!?
		if(equipedFlashlight!=null){
			Flashlight f = equipedFlashlight.GetComponent<Flashlight>();
			if(f.batteryRemaining<=0){
				f.ThrowFlashlight();
				equipedFlashlight = null;
			}
		}
		//not an else so that it can happen in the same frame
		if(equipedFlashlight==null&&InventoryManager.numFlashlights>0){
			GameObject o = Instantiate(flashlightPrefab,transform.TransformPoint(Vector3.zero),transform.rotation) as GameObject;
			equipedFlashlight = o;
			Flashlight f = o.GetComponent<Flashlight>();
			f.GunPoint = flashlightPoint;
			f.autoLock = true;
			f.lockPos = false;
			f.otherLight = flashlightAlt;
			InventoryManager.numFlashlights--;
		}
		if(Input.GetKeyDown(KeyCode.F4)){
			InventoryManager.numFlashlights++;
		}
		for(int i = 0; i<weapons.Length;i++){
			if(Input.GetKeyDown((i+1).ToString())){
				if(InventoryManager.weaponUnlocked(i))switchToWeapon(i);
			}
		}
	}

	public void FixedUpdate(){
		if(invulTime>0)invulTime--;

		//footsteps
		//lets have this only run every other FixedUpdate
		if(tooSoon){
			tooSoon = false;
			return;
		}
		tooSoon = true;

		if(!controller.isGrounded)return;
		distanceTraveled+=Vector3.Distance(transform.position,previousLocation);
		if(distanceTraveled>=stepDistance){
			playOneSound(footstepSounds);
			while(distanceTraveled>=stepDistance)distanceTraveled-=stepDistance;//in case the player was in the air
		}
		previousLocation = transform.position;
	}

	private void playOneSound(AudioClip[] list){
		int chosen = Random.Range(0,list.Length-1);
		source.PlayOneShot(list[chosen]);
	}

	public void switchToWeapon(int num){
//		Debug.Log("Switch to "+num);
		if(curWeapon!=null)curWeapon.GetComponent<Weapon>().PutAway();
		GameObject toDestroy = curWeapon;
		DestroyImmediate(toDestroy);
		curWeapon = null;
		curWeapon = Instantiate(weapons[num].gameObject,transform.position, transform.rotation)as GameObject;
		Weapon w = curWeapon.GetComponent<Weapon>() as Weapon;
		w.GunPoint = gunPoint;
		CrossHairScript.updateCrosshair(w.muzzle);
	}
	public static Player GetInstance(){return instance;}

	public void hurt(int damage){
		if(invulTime>0||health<0)return;
		invulTime = maxInvulTime;
		health-=damage;
		playOneSound(hurtSounds);
		if(health<=0.0f && !dead){
			invulTime = 150;
			playOneSound(deathSounds);
			head.GetComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseXAndY;
			dead = true;
			equipedFlashlight.GetComponent<Flashlight>().ThrowFlashlight();
			curWeapon.GetComponent<Weapon>().detatch();
			head.rigidbody.isKinematic = false;
			head.collider.enabled = true;
			gameObject.transform.DetachChildren();
			body.renderer.enabled = false;
			collider.enabled = false;

		}
		Debug.Log (health);
	}
}
