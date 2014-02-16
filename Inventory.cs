using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	private GameObject equipedFlashlight;
	public GameObject flashlightPrefab;

	public Weapon[] weapons;
	private GameObject curWeapon = null;

	public Transform flashlightPoint,gunPoint;

	public Light flashlightAlt;

	public int flashlights{get; set;}

	private static Inventory instance;
	// Use this for initialization
	void Start () {
//		flashlights=0;
		instance = this;
		switchToWeapon(0);
	}
	
	// Update is called once per frame
	void Update () {
		//grab items off ground!?
		if(equipedFlashlight!=null){
			Flashlight f = equipedFlashlight.GetComponent<Flashlight>();
			if(f.batteryRemaining<=0){
				f.ThrowFlashlight();
				equipedFlashlight = null;
			}
		}
		//not an else so that it can happen in the same frame
		if(equipedFlashlight==null&&flashlights>0){
			GameObject o = Instantiate(flashlightPrefab,transform.TransformPoint(Vector3.zero),transform.rotation) as GameObject;
			equipedFlashlight = o;
			Flashlight f = o.GetComponent<Flashlight>();
			f.GunPoint = flashlightPoint;
			f.autoLock = true;
			f.lockPos = false;
			f.otherLight = flashlightAlt;
			flashlights--;
		}
		if(Input.GetKeyDown(KeyCode.F4)){
			flashlights++;
		}
		for(int i = 0; i<weapons.Length;i++){
			if(Input.GetKeyDown((i+1).ToString())){
				switchToWeapon(i);
			}
		}




	}

	public void switchToWeapon(int num){
		Debug.Log("Switch to "+num);
		if(curWeapon!=null)curWeapon.GetComponent<Weapon>().PutAway();
		GameObject toDestroy = curWeapon;
		DestroyImmediate(toDestroy);
		curWeapon = null;
		curWeapon = Instantiate(weapons[num].gameObject,transform.position, transform.rotation)as GameObject;
		Weapon w = curWeapon.GetComponent<Weapon>() as Weapon;
		w.GunPoint = gunPoint;
		CrossHairScript.updateCrosshair(w.muzzle);
	}
	public static Inventory GetInstance(){return instance;}
}
