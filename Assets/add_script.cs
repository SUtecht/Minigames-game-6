using UnityEngine;
using System.Collections;

public class add_script : MonoBehaviour {
	int health = 100;
	private GameObject player;
	public GameObject pinModel;
	//public GameObject add_Model;
	public GameObject webModel;
	//bool halftime = false;
	private boss the_boss;
	public AudioClip boss_shoot1;
	public AudioClip boss_shoot2;
	private Vector3 my_point;
	private GameObject[] movepoints;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		the_boss = (boss) GameObject.FindGameObjectWithTag("Spider").GetComponent<boss>();
		StartCoroutine(pinSpit(3f));
		StartCoroutine(webSpit(10f));
		movepoints = the_boss.movepoints;
		changeTarget();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine(transform.position, my_point);
		Debug.DrawRay(transform.position, transform.right, Color.red);
		if (health <= 0 && player.activeSelf){
			Debug.Log ("Add destroyed");
			the_boss.AddDown();
			gameObject.SetActive(false);
			//victory.SetActive(true);
			//AudioSource.PlayClipAtPoint(winClip, transform.position);
		}

		if (Vector3.Distance(transform.position, my_point) < 5f){
			Debug.Log("changing target");
			changeTarget();
		}
		
		transform.position = Vector3.MoveTowards( transform.position,  my_point , 5 * Time.deltaTime);
	}


	void OnTriggerEnter2D (Collider2D col)
	{
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "Bolt"){
			health = health - 3;
			Debug.Log("Add hit!");
			col.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			//AudioSource.PlayClipAtPoint(boss_damage1, transform.position);
		}
		
	}
	
	IEnumerator pinSpit(float x){
		yield return new WaitForSeconds(x);
		shootPin();
		if (health > 50){
			StartCoroutine(pinSpit(1f));
		}else{
			StartCoroutine(pinSpit(.5f));
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

	void changeTarget(){
		my_point = movepoints[Random.Range(0,movepoints.Length)].transform.position;
		//Debug.Log("moving to " + my_point);
		
	}
	
}
