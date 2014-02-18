using UnityEngine;
using System.Collections;

public class TestPistol : Weapon {
	private Animator ani;
	private int ammoRemaining;
	private bool firing = false;
	private const float RECOIL = 1.0f;
	public GameObject muzzleFlash;
//	public Transform muzzle;
	public GameObject bulletPrefab;

	public AudioClip[] fireSounds,clickSounds;

	private AudioSource audioSource;

	private const float MUZZLE_VELOCITY = 251.0f;
	private int AmmoRemaining{
		get{
			return ammoRemaining;
		}
		set{
			ammoRemaining = value;
			ani.SetInteger("Ammo",value);
		}
	}
	public int maxAmmo = 8;
	private int idleState,fireState,reloadState,emptyState;

	// Use this for initialization
	void Start () {
		audioSource = GetComponentInChildren<AudioSource>();
		ani = GetComponent<Animator>() as Animator;
		if(ani==null)ani = GetComponentInChildren<Animator>() as Animator;
		if(InventoryManager.numPistolAmmo<maxAmmo){
			AmmoRemaining = InventoryManager.numPistolAmmo;
			InventoryManager.numPistolAmmo = 0;
		}else{
			AmmoRemaining = maxAmmo;
			InventoryManager.numPistolAmmo-=maxAmmo;
		}		
		ani.SetBool("Fire", false);
		ani.SetBool("Reload",false);
		idleState = Animator.StringToHash("Base Layer.Idle");
		fireState = Animator.StringToHash("Base Layer.Fire");
		reloadState = Animator.StringToHash("Base Layer.Reload");
		emptyState = Animator.StringToHash("Base Layer.OutOfAmmo");
	}
	
	// Update is called once per frame
	void Update () {
		int cur = ani.GetCurrentAnimatorStateInfo(0).nameHash;
		muzzleFlash.GetComponent<Light>().enabled = firing;
		Renderer[] mfc = muzzleFlash.GetComponentsInChildren<Renderer>();
		foreach(Renderer r in mfc){
			r.enabled = firing;
		}
		if(cur==reloadState){
			//AmmoRemaining = maxAmmo;
			firing = false;
			ani.SetBool("Fire",false);
			ani.SetBool("Reload",false);
			return;
		}
		if(cur==idleState){
			firing = false;
			ani.SetBool("Fire",false);
		}
		if(!firing && cur == fireState){
			ani.SetBool("Fire",false);//STOP FIRING GODDAMNIT!
			return;
		}
		if(cur == emptyState){
			ani.SetBool("Fire",false);
			firing = false;
		}
		if(InventoryManager.numPistolAmmo!=0&&(cur==emptyState||cur==idleState)&&Input.GetButtonDown("Reload")&&AmmoRemaining!=maxAmmo&&!Player.Dead){
			InventoryManager.numPistolAmmo+=AmmoRemaining;
			if(InventoryManager.numPistolAmmo<maxAmmo){
				AmmoRemaining = InventoryManager.numPistolAmmo;
				InventoryManager.numPistolAmmo=0;
			}else{
				InventoryManager.numPistolAmmo-=maxAmmo;
				AmmoRemaining = maxAmmo;
			}
			ani.SetBool("Reload",true);
		}
		if(Input.GetButtonDown("Fire1")&&cur==idleState&&AmmoRemaining>0&&!Player.Dead){
			firing = true;
			ani.SetBool("Fire",true);
		}

	}
	public override void Fire ()
	{
//		if(ani.IsInTransition(0))return;
		if(AmmoRemaining<=0){
			playOne (clickSounds);
			return;
		}
		int cur = ani.GetCurrentAnimatorStateInfo(0).nameHash;
		if(cur==fireState||cur==emptyState||cur==reloadState)return;
		GameObject created = Instantiate(bulletPrefab,muzzle.position,muzzle.rotation) as GameObject;
		Bullet b = created.GetComponent<Bullet>();
		b.setVelocity(muzzle.forward.normalized*MUZZLE_VELOCITY);
		AmmoRemaining--;
		firing = true;
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
		InventoryManager.numPistolAmmo+=AmmoRemaining;
	}

	private void playOne(AudioClip[] choices){
		int chosen = Random.Range (0,choices.Length-1);
		audioSource.PlayOneShot(choices[chosen]);
	}
}
