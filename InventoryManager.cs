using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

	public static int numFlashlights = 1;
	public static int numPistolAmmo;
	public static int numMachineGunAmmo = 0;
	public static int numLaserGunAmmo = 0;
	public static int numAmmo = 30;

	public static MonoBehaviour monoBehaviour;

	public static GUIText itemNotification;

	private static bool haveMachineGun = false;
	private static bool haveLaserGun = false;
	
	void Start() {
//		Screen.lockCursor = true;
		Screen.showCursor = false;
		monoBehaviour = (MonoBehaviour)gameObject.GetComponent("MonoBehaviour");
	}

	public static bool AddToInventory (GameObject item) {
		if (item.tag == "Flashlight") {
			++numFlashlights;
			monoBehaviour.StartCoroutine(ShowMessage("Picked up flashlight"));
			return true;
		}
		else if (item.tag == "Pistol") {
			numPistolAmmo += 16;
			monoBehaviour.StartCoroutine(ShowMessage("Picked up 16 pistol ammo"));
			return true;
		}
		else if (item.tag == "Machine Gun") {
			if (haveMachineGun) {
				numMachineGunAmmo += 32;
				monoBehaviour.StartCoroutine(ShowMessage("Picked up 32 machine gun ammo"));
			}
			else {
				haveMachineGun = true;
				numMachineGunAmmo += 48;
				monoBehaviour.StartCoroutine(ShowMessage("Picked up machine gun"));
			}
			return true;
		}
		else if (item.tag == "Laser Gun") {
			if (haveLaserGun) {
				numLaserGunAmmo += 10;
				monoBehaviour.StartCoroutine(ShowMessage("Picked up 10 laser gun ammo"));
			}
			else {
				haveLaserGun = true;
				numLaserGunAmmo += numAmmo;
				monoBehaviour.StartCoroutine(ShowMessage("Picked up laser gun, press 2 to switch"));
			}
			return true;
		}
		else
			return false;
	}

	public static void DisplayInventory (GUIText inventoryList) {
		inventoryList.text = "Inventory: \nFlashlights: " + numFlashlights + "\nPistol Ammunition: " + numPistolAmmo;
		if (haveMachineGun)
			inventoryList.text += "\nMachine Gun Ammunition: " + numMachineGunAmmo;
		if (haveLaserGun)
			inventoryList.text += "\nLaser Gun Ammunition: " + numLaserGunAmmo;
	}

	public static IEnumerator ShowMessage (string message) {
		itemNotification.text = message;
		itemNotification.enabled = true;
		yield return new WaitForSeconds (2.5f);
		itemNotification.enabled = false;
	}

	public static void ClearInventory () {
		numFlashlights = 1;
		numPistolAmmo = 48;
		numMachineGunAmmo = 0;
		numLaserGunAmmo = 0;
		numAmmo = 30;
		
		haveMachineGun = false;
		haveLaserGun = false;

	}

	public static bool weaponUnlocked(int id){
		switch(id){
		case 0:return true;
		case 1:return haveLaserGun;
		default:return false;
		}
	}


}
