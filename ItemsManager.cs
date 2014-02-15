using UnityEngine;
using System.Collections;

public class ItemsManager : MonoBehaviour {
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			if (InventoryManager.AddToInventory(gameObject)) {
				gameObject.SetActive (false);
			}
		}
	}
}
