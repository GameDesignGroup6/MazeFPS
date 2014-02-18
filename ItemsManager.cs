using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ItemsManager : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			if (InventoryManager.AddToInventory(gameObject)) {
				GetComponent<AudioSource>().Play();
				collider.enabled = false;
				foreach (Collider c in GetComponentsInChildren<Collider>()){
					c.enabled = false;
				}
				foreach (Renderer c in GetComponentsInChildren<Renderer>()){
					c.enabled = false;
				}
				foreach (Light c in GetComponentsInChildren<Light>()){
					c.enabled = false;
				}
				light.enabled = false;

				Destroy(gameObject,1);
				enabled = false;
			}
		}
	}
}
