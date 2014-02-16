using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	public Transform player;
	public float moveSpeed;
	public bool takeDamage;
	public Health healthScript;
	public float attackDamage;
	public Ray ray;
	public RaycastHit hit;
	public bool willChase;
	public float enemyHealth;
	public Animator animator;

	void Start() {
		//Gets the health variable from the player character's script
		healthScript = player.GetComponent<Health>();
		ray = new Ray (transform.position, player.position - transform.position);
		animator = GetComponent<Animator>();
		moveSpeed = 0.0004f;
		attackDamage = 0.01f;
		enemyHealth = 5f;
		takeDamage = false;
		willChase = false;
	}

	void Update() {
		//Determines whether the player character is in view (i.e. not behind a wall)
		if (Physics.Linecast (transform.position, player.position, out hit)) {
			if (hit.collider.gameObject.tag == "Player")
				willChase = true;
			else 
				willChase = false;
		}
		//Damages the player if it collides with the player
		if (takeDamage == true) {
			healthScript.health = healthScript.health - attackDamage;
		}
		StartCoroutine (Move ());
		//Determines if enemy is dead and destroys the object if so
		if (enemyHealth <= 0)
			Destroy (gameObject);
		if (willChase && !takeDamage)
			animator.SetBool("running", true);
		else
			animator.SetBool("running", false);
	}

	//Rotates the enemy towards the player and lerps to them if they are in sight
	IEnumerator Move() {
		while (willChase && !takeDamage) {
			transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
			transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), moveSpeed);
			yield return 0;
		}
	}

	void OnTriggerEnter(Collider victim) {
		if (victim.tag == "Player")
			takeDamage = true;
	}

	void OnTriggerExit(Collider victim) {
		if (victim.tag == "Player")
			takeDamage = false;
	}

}
