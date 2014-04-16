using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;	// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;	// Condition for whether the player should jump.
	
	
	public float moveForce = 35f;	// Amount of force added to move the player left and right.
	public float maxSpeed = 2f;	// The fastest the player can travel in the x axis.
	
	public AudioClip pin_hit_audio;	
	public AudioClip web_hit_audio;
	public AudioClip player_shoot;
	public AudioClip lose_clip;
	public float jumpForce = 10f;	// Amount of force added when the player jumps.
	
	private int tauntIndex;	// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;	// A position marking where to check if the player is grounded.
	private bool grounded = false;	// Whether or not the player is grounded.
	public Animator anim;	// Reference to the player's animator component.
	public GameObject arrowModel;
	public GameObject boss;
	private int health = 100;
	bool canMove = true;
	public GUIScript cam;
	
	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("grounded");
		
		//winText = GameObject.FindGameObjectsWithTag("Finish")[0];
		//anim = gameObject.GetComponentsInChildren<Animator>;
		//winText.renderer.enabled = false;
	}
	
	
	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Grounded"));
		//Debug.Log(grounded);
		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump") && grounded){
			jump = true;
			//Debug.Log("Jumping!");
		}
	}
	
	
	void FixedUpdate ()
	{
		if (health <= 0){
			Debug.Log ("Game Over");
			//gameObject.active = false;
			//lost.SetActive(true);
			gameObject.SetActive(false);
			AudioSource.PlayClipAtPoint(lose_clip, transform.position);
			cam.lost = true;
			cam.won = false;
		}
		if (canMove){
			// Cache the horizontal input.
			float h = Input.GetAxis("Horizontal");
			
			// The Speed animator parameter is set to the absolute value of the horizontal input.
		//	anim.SetFloat("Speed", Mathf.Abs(h));
			
			
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(h * rigidbody2D.velocity.x < maxSpeed)
				// ... add a force to the player.
				rigidbody2D.AddForce(Vector2.right * h * moveForce);
			
			// If the player's horizontal velocity is greater than the maxSpeed...
			if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
			
			// If the input is moving the player right and the player is facing left...
			if(h > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(h < 0 && facingRight)
				// ... flip the player.
				Flip();
			
			// If the player should jump...
			if(jump)
			{
				
				
				// Add a vertical force to the player.
				rigidbody2D.AddForce(new Vector2(0f, jumpForce));
				
				// Make sure the player can't jump again until the jump conditions from Update are satisfied.
				jump = false;
				
			}

			// Shoot an Arrow
			if (Input.GetButtonDown("Fire1")) {
				AudioSource.PlayClipAtPoint(player_shoot, transform.position);
				shootArrow();
			}
		}
	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void shootArrow() {
		// Create arrow at current position
		Vector3 spawn = new Vector3( transform.position.x, transform.position.y + 1f, transform.position.z) ;
		GameObject arrow = (GameObject) Instantiate(arrowModel, spawn, transform.rotation);
		// Create target
		//Vector3 target = boss.transform.position - arrow.transform.position;
		//target.Normalize();
		// Rotate by Atan2 of target
		//arrow.transform.Rotate(Vector3.forward, Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg);
		// Apply velocity (remember right is "forward")
		arrow.rigidbody2D.velocity = arrow.transform.up * 40;
//		AudioSource.PlayClipAtPoint(shoot, transform.position);
		
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "pin"){
			AudioSource.PlayClipAtPoint(pin_hit_audio, transform.position);
			//col.gameObject.SetActive(false);
			Debug.Log("Ow");
			health -= 5;
		}
		if(col.gameObject.tag == "web"){
			AudioSource.PlayClipAtPoint(web_hit_audio, transform.position);
			//col.gameObject.SetActive(false);
			Debug.Log("Ow");
			health -= 5;
			col.gameObject.transform.position = transform.position;
			StartCoroutine(stuck(5));
		}


	}

	IEnumerator stuck(float x){
		canMove = false;
		yield return new WaitForSeconds(x);
		canMove = true;
	}
	/*
	void OnCollisionEnter2D(Collision2D col)
	{
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "Sheep"){
			if(cam.CanBaa()){
				AudioSource.PlayClipAtPoint(bah, transform.position);
			}
		}
		
	}*/
	
	
	
	
}