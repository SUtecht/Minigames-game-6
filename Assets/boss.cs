using UnityEngine;
using System.Collections;

public class boss : MonoBehaviour {
	int health = 100;
	public GameObject player;
	public GameObject pinModel;
	public GameObject add_Model;
	public GameObject webModel;
	bool halftime = false;
	bool adds_up = false;
	public GameObject[] movepoints;
	private Vector3 my_point;
	public AudioClip boss_shoot1;
	public AudioClip boss_shoot2;
	public AudioClip boss_damage1;
	public AudioClip victory_Clip;
	public GUIScript cam;

	// Use this for initialization
	void Start () {
		StartCoroutine(pinSpit(2f));
		StartCoroutine(pinSpit(1.5f));
		StartCoroutine(webSpit(5f));
		changeTarget();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine(transform.position, my_point);
		Debug.DrawRay(transform.position, transform.right, Color.red);
		if (health <= 0 && player.activeSelf){
			Debug.Log ("Game Ended");
			gameObject.SetActive(false);
			AudioSource.PlayClipAtPoint(victory_Clip, transform.position);
			cam.won = true;
		}
		if (health <= 50 && halftime == false){
			spawnAdd();
			halftime = true;
		}

		if (Vector3.Distance(transform.position, my_point) < 5f){
			Debug.Log("changing target");
			changeTarget();
		}

		transform.position = Vector3.MoveTowards( transform.position,  my_point , 5 * Time.deltaTime);

	}

	void OnTriggerEnter2D (Collider2D col){
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "Bolt" && !adds_up){
			health = health - 3;
			Debug.Log("Boss hit!");
			col.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			AudioSource.PlayClipAtPoint(boss_damage1, transform.position);
		}

	}

	IEnumerator pinSpit(float x){
		yield return new WaitForSeconds(x);
		shootPin();
		if (health > 50){
			StartCoroutine(pinSpit(2f));
		}else{
			StartCoroutine(pinSpit(1.5f));
		}
	}

	IEnumerator webSpit(float x){
		yield return new WaitForSeconds(x);
		shootWeb();
		if (health > 50){
			StartCoroutine(webSpit(5f));
		}else{
			StartCoroutine(webSpit(2f));
		}
	}
	void shootWeb() {
		AudioSource.PlayClipAtPoint(boss_shoot2, transform.position);
		// Create arrow at current position
		GameObject web = (GameObject) Instantiate(webModel, transform.position, transform.rotation);
		// Create target
		Vector3 target = player.transform.position - web.transform.position;
		target.Normalize();
		// Rotate by Atan2 of target
		web.transform.Rotate(Vector3.forward, Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg);
		// Apply velocity (remember right is "forward")
		web.rigidbody2D.velocity = web.transform.right * 10;

	}

	void spawnAdd() {
		Vector3 spawn = new Vector3( transform.position.x + 10f, transform.position.y + 1f, transform.position.z) ;
		GameObject add_ = (GameObject) Instantiate(add_Model, spawn, transform.rotation);
		// Create target
		Vector3 target = player.transform.position - add_.transform.position;
		target.Normalize();
		// Rotate by Atan2 of target
		add_.transform.Rotate(Vector3.forward, Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg);
		// Apply velocity (remember right is "forward")
		//add_.rigidbody2D.velocity = add_.transform.right * 10;
		adds_up = true;
		
	}
	void shootPin() {
		AudioSource.PlayClipAtPoint(boss_shoot1, transform.position);
		// Create arrow at current position
		GameObject pin = (GameObject) Instantiate(pinModel, transform.position, transform.rotation);
		// Create target
		Vector3 target = player.transform.position - pin.transform.position;
		target.Normalize();
		// Rotate by Atan2 of target
		pin.transform.Rotate(Vector3.forward, Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg);
		// Apply velocity (remember right is "forward")
		pin.rigidbody2D.velocity = pin.transform.right * 20;
		
	}

	public void AddDown(){
		adds_up = false;
	}

	void changeTarget(){
		my_point = movepoints[Random.Range(0,movepoints.Length)].transform.position;
		//Debug.Log("moving to " + my_point);

	}

}
