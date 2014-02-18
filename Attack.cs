using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	
	private Transform player;
	public float moveSpeed = 0.04f;
	private bool touchingPlayer;
	private Player healthScript;
	public int attackDamage = 1;
//	private Ray ray;
	private RaycastHit hit;
	private bool willChase;
	public int enemyHealth = 5;
	private Animator animator;
	public GameObject ragdollPrefab;

	private static bool aiEnabled = true;
	
	void Start() {
		//Gets the health variable from the player character's script
		player = GameObject.Find("Player").transform;
		healthScript = player.GetComponent<Player>();
		
//		ray = new Ray (transform.position, player.position - transform.position);
		animator = GetComponent<Animator>();
		touchingPlayer = false;
		willChase = false;
	}
	
	void FixedUpdate() {


		if(Input.GetKeyDown(KeyCode.F6))aiEnabled = false;
		if(Input.GetKeyDown(KeyCode.F7))aiEnabled = true;
		if(!aiEnabled)return;
//		if(enemyHealth<=2)animator.SetBool("gocrouch",true);
		//Determines whether the player character is in view (i.e. not behind a wall)
		transform.LookAt(new Vector3(player.position.x, transform.position.y, 
		                             player.position.z));
		if (Physics.Linecast (transform.position, player.position, out hit)) {
			willChase = hit.collider.gameObject.tag == "Player";
		}
		
		//Damages the player if it collides with the player
		if (touchingPlayer) {
			healthScript.hurt (attackDamage);
		}
		//		StartCoroutine (Move ());
		float distance = 0;
		if(willChase && !touchingPlayer){
			distance = Vector3.Distance(transform.position,new Vector3(player.position.x, transform.position.y, player.position.z));
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), moveSpeed);
		}
		
		//Determines if enemy is dead and destroys the object if so
		if (enemyHealth <= 0){
			Destroy(Instantiate(ragdollPrefab,transform.position,transform.rotation),5);
			Destroy (gameObject);
		}

		animator.SetBool("running", distance>=moveSpeed);
	}
	

	void OnCollisionEnter(Collision victim) {
		if (victim.gameObject.tag == "Player"){
			Debug.Log ("Hit player!");
			touchingPlayer = true;
		}
	}
	
	void OnCollisionExit(Collision victim) {
		if (victim.gameObject.tag == "Player")
			touchingPlayer = false;
	}
	
	public void onHit(){
		enemyHealth--;
	}
	
}
