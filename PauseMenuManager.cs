using UnityEngine;

public class PauseMenuManager : MonoBehaviour {

	public GUIText inventoryText, pausedText, restartText, unpauseText, inventoryList, itemNotification, quitText;

	public int numFlashlights = 1;
	public int numPistolAmmo = 30;
	public int numMachineGunAmmo = 0;
	public int numLaserGunAmmo = 0;
	public int numAmmo = 30;

	public MonoBehaviour mouseMove;

	public Vector3 startPosition = new Vector3(0f, 1.08f, 0f);

	void Start () {
		InventoryManager.numFlashlights = numFlashlights;
		InventoryManager.numPistolAmmo = numPistolAmmo;
		InventoryManager.numMachineGunAmmo = numMachineGunAmmo;
		InventoryManager.numLaserGunAmmo = numLaserGunAmmo;
		InventoryManager.numAmmo = numAmmo;
		InventoryManager.itemNotification = itemNotification;
		Time.timeScale = 1.0f;
		transform.position = startPosition;
		InventoryManager.ClearInventory ();
		inventoryText.enabled = false;
		pausedText.enabled = false;
		restartText.enabled = false;
		unpauseText.enabled = false;
		inventoryList.enabled = false;
		itemNotification.enabled = false;
		quitText.enabled = false;
		mouseMove.enabled = true;
	}

	void Update () {
		if (Input.GetKey ("p")) {
			Time.timeScale = 0.0f;
			inventoryList.enabled = false;
			inventoryText.enabled = true;
			pausedText.enabled = true;
			restartText.enabled = true;
			unpauseText.enabled = true;
			quitText.enabled = true;
			mouseMove.enabled = false;
		}

		if (Input.GetKey ("return")) {
			Time.timeScale = 1.0f;
			inventoryList.enabled = false;
			inventoryText.enabled = false;
			pausedText.enabled = false;
			restartText.enabled = false;
			unpauseText.enabled = false;
			quitText.enabled = false;
			mouseMove.enabled = true;
		}

		if (Input.GetKey ("r") && pausedText.enabled == true) {
			Application.LoadLevel ("scene1");
		}

		if (Input.GetKey ("i")) {
			Time.timeScale = 0.0f;
			inventoryText.enabled = false;
			pausedText.enabled = false;
			restartText.enabled = false;
			unpauseText.enabled = true;
			quitText.enabled = false;
			mouseMove.enabled = false;
			InventoryManager.DisplayInventory (inventoryList);
			inventoryList.enabled = true;
		}

		if (Input.GetKey ("q") && pausedText.enabled == true) {
			Application.Quit();
		}
	}
}
