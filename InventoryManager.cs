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
		monoBehaviour = (MonoBehaviour)gameObject.GetComponent("MonoBehaviour");
	}

	public static bool AddToInventory (GameObject item) {
		if (item.name == "Flashlight") {
			++numFlashlights;
			monoBehaviour.StartCoroutine(ShowMessage("Picked up flashlight"));
			return true;
		}
		else if (item.name == "Pistol") {
			numPistolAmmo += numAmmo;
			monoBehaviour.StartCoroutine(ShowMessage("Picked up pistol ammo"));
			return true;
		}
		else if (item.name == "Machine Gun") {
			if (haveMachineGun) {
				numMachineGunAmmo += numAmmo;
				monoBehaviour.StartCoroutine(ShowMessage("Picked up machine gun ammo"));
			}
			else {
				haveMachineGun = true;
				numMachineGunAmmo += numAmmo;
				monoBehaviour.StartCoroutine(ShowMessage("Picked up machine gun"));
			}
			return true;
		}
		else if (item.name == "Laser Gun") {
			if (haveLaserGun) {
				numLaserGunAmmo += numAmmo;
				monoBehaviour.StartCoroutine(ShowMessage("Picked up laser gun ammo"));
			}
			else {
				haveLaserGun = true;
				numLaserGunAmmo += numAmmo;
				monoBehaviour.StartCoroutine(ShowMessage("Picked up laser gun"));
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
		numPistolAmmo = 30;
		numMachineGunAmmo = 0;
		numLaserGunAmmo = 0;
		numAmmo = 30;
		
		haveMachineGun = false;
		haveLaserGun = false;

	}


}
