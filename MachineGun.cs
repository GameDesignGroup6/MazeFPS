using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class MachineGun : Weapon {
	private Animator ani;
	private int ammoRemaining;
	private bool firing = false;
	private const float RECOIL = 1.0f;
	public GameObject muzzleFlash;
	//	public Transform muzzle;
	public GameObject bulletPrefab;
	public AudioClip[] fireSounds,clickSounds;
	private const float MUZZLE_VELOCITY = 251.0f;
	private AudioSource audioSource;
	private int idleState,fireState,reloadState;

	private float time = 0;
	private bool canFire = true;

	private int AmmoRemaining{
		get{
			return ammoRemaining;
		}
		set{
			ammoRemaining = value;
			ani.SetInteger("Ammo",value);
		}
	}
	public int maxAmmo = 35;
	public static float machineGunDelay=0.12f;
	
	// Use this for initialization
	void Start () {
		audioSource = GetComponentInChildren<AudioSource>();
		ani = GetComponent<Animator>() as Animator;
		if(ani==null)ani = GetComponentInChildren<Animator>() as Animator;
		if(InventoryManager.numMachineGunAmmo<maxAmmo){
			AmmoRemaining = InventoryManager.numMachineGunAmmo;
			InventoryManager.numMachineGunAmmo = 0;
		}else{
			AmmoRemaining = maxAmmo;
			InventoryManager.numMachineGunAmmo-=maxAmmo;
		}
		ani.SetBool("Fire", false);
		ani.SetBool("Reload",false);
		idleState = Animator.StringToHash("Base Layer.Idle");
		fireState = Animator.StringToHash("Base Layer.Fire");
		reloadState = Animator.StringToHash("Base Layer.Reload");
	}
	
	// Update is called once per frame
	void Update () {
		int cur = ani.GetCurrentAnimatorStateInfo(0).nameHash;
		time+=Time.deltaTime;
		if(InventoryManager.numMachineGunAmmo!=0&&Input.GetButtonDown("Reload")&&AmmoRemaining!=maxAmmo&&!Player.Dead){
			Debug.Log ("Reload!");
			InventoryManager.numMachineGunAmmo+=AmmoRemaining;
			if(InventoryManager.numMachineGunAmmo<maxAmmo){
				AmmoRemaining = InventoryManager.numMachineGunAmmo;
				InventoryManager.numMachineGunAmmo=0;
			}else{
				InventoryManager.numMachineGunAmmo-=maxAmmo;
				AmmoRemaining = maxAmmo;
			}
			ani.SetBool("Reload",true);
			return;
		}
//		if(time>=machineGunDelay){
//			canFire = true;
//		}else{
//			//canFire = false;
//		}
//		firing = false;

//		if(Input.GetButton("Fire1")&&AmmoRemaining>0&&!Player.Dead&&time>=machineGunDelay){
//			firing = true;
//			ani.SetBool("Fire",true);
//		}

		muzzleFlash.GetComponent<Light>().enabled = time<machineGunDelay;
		Renderer[] mfc = muzzleFlash.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in mfc){
			r.enabled = time<machineGunDelay;
		}
		if(cur==reloadState){
			//AmmoRemaining = maxAmmo;
//			firing = false;
//			canFire = false;
			ani.SetBool("Fire",false);
			ani.SetBool("Reload",false);
			return;
		}

//		if(!firing && cur == fireState){
//			ani.SetBool("Fire",false);//STOP FIRING GODDAMNIT!
//			return;
//		}
		if(Input.GetButton("Fire1")&&AmmoRemaining>0&&!Player.Dead){
			firing = true;
			ani.SetBool("Fire",true);
			ActualFire();
		}



		
	}
	public override void Fire ()
	{
//		throw new System.NotImplementedException ();
	}
	public void ActualFire ()
	{
		if(time<machineGunDelay)return;
//		Debug.Log (ammoRemaining);

		if(AmmoRemaining<=0&&time>=machineGunDelay){
			playOne (clickSounds);
			return;
		}
		int cur = ani.GetCurrentAnimatorStateInfo(0).nameHash;
		if(cur==reloadState)return;
		//GameObject created = Instantiate (bulletPrefab, transform.position, transform.rotation) as GameObject;
		GameObject created = Instantiate (bulletPrefab, muzzle.position, muzzle.rotation) as GameObject;
		Bullet b = created.GetComponent<Bullet> ();
		b.setVelocity (muzzle.forward.normalized * MUZZLE_VELOCITY);
		AmmoRemaining--;
		firing = true;
		canFire = false;
		time = 0;
		ani.SetBool("Fire",true);
		playOne(fireSounds);
		//do recoil
		Vector3 curRot = transform.localEulerAngles;
		curRot.x-=Random.Range(5f,10f)*RECOIL;
		curRot.y+=Random.Range(-1f,1f)*RECOIL;
		curRot.z+=Random.Range(-0.5f,0.5f)*RECOIL;
		transform.localEulerAngles=curRot;
		
		//		muzzleFlash.transform.localRotation = Random.rotationUniform;
	}
	
	public override void UpdateCallback ()
	{
		//throw new System.NotImplementedException ();
	}
	
	public override void PutAway ()
	{
		audioSource.Stop();
		InventoryManager.numMachineGunAmmo+=AmmoRemaining;
	}
	private void playOne(AudioClip[] choices){
		int chosen = Random.Range (0,choices.Length-1);
		audioSource.PlayOneShot(choices[chosen]);
	}
	
}
