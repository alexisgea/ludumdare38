using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Transform graphics;
	public Animator animator;
	public Transform planet;
	public float speed = 10f;
	public float acceleration = 10f;
	public float jumpForce = 100f;

	public ParticleSystem jumpPuffParticles;

	bool jumpInput;

	GameManager gm;

	void Start ()
	{
		var rb = GetComponent<Rigidbody2D>();
		rb.centerOfMass = new Vector2(0f, 0f);
		gm = FindObjectOfType<GameManager> ();
	}

	void FixedUpdate ()
	{
		if (!gm.StartGame)
			return;

		var rb = GetComponent<Rigidbody2D>();

		var delta = transform.position - planet.position;
		var rotation = Quaternion.LookRotation (Vector3.forward, delta);
		rb.MoveRotation (rotation.eulerAngles.z);

		var velocity = transform.InverseTransformDirection(rb.velocity);

		var hotizontalInput = Input.GetAxisRaw ("Horizontal");
		var targetSpeed = hotizontalInput * speed;

		//var deltaSpeed = targetSpeed - velocity.x;

		//rb.AddRelativeForce (new Vector2 (deltaSpeed, 0f) * rb.mass, ForceMode2D.Force);

		velocity.x = Mathf.MoveTowards (velocity.x, targetSpeed, acceleration * Time.deltaTime);

		//rb.AddRelativeForce (new Vector2(velocity.x, 0f));
		//print (velocity); 

		rb.velocity = transform.TransformVector(velocity);

		if (jumpInput) {
			jumpInput = false;
			jumpPuffParticles.Play ();
			rb.AddRelativeForce (new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}


		if (Mathf.Abs (hotizontalInput) > 0.1f) {
			var rot = graphics.localEulerAngles;
			rot.y = targetSpeed < 0 ? 180f : 0f;
			graphics.localEulerAngles = rot;
			animator.Play ("Run");
		}
		else {
			animator.Play ("Idle");
		}
	}

	void Update ()
	{
		float playerHeight = (Vector3.zero - transform.position).sqrMagnitude;
		float allowedHeight =  Mathf.Clamp(Mathf.Pow(12.5f + FindObjectOfType<GameManager>().Wave * 1.2f, 2), 0f, Mathf.Pow(25,2));
		
		if(playerHeight < allowedHeight)
			jumpInput = jumpInput || Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.Z);

		if (Input.GetKeyDown (KeyCode.J)) {
			GetComponentInChildren<Shooter>().Shoot ();
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (transform.InverseTransformDirection (col.relativeVelocity).y > 0.5f) {
			jumpPuffParticles.Play ();
		}
	}
}
