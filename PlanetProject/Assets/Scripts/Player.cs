using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Transform planet;
	public float speed = 10f;
	public float acceleration = 10f;
	public float jumpForce = 100f;
	bool jumpInput;

	void FixedUpdate ()
	{
		var rb = GetComponent<Rigidbody2D>();

		var velocity = transform.InverseTransformDirection(rb.velocity);

		var targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;

		velocity.x = Mathf.MoveTowards (velocity.x, targetSpeed, acceleration * Time.deltaTime);

		rb.velocity = transform.TransformVector(velocity);

		if (jumpInput) {
			jumpInput = false;
			rb.AddRelativeForce (new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}

		var delta = transform.position - planet.position;
		var rotation = Quaternion.LookRotation (Vector3.forward, delta);
		rb.MoveRotation (rotation.eulerAngles.z);
	}

	void Update ()
	{
		jumpInput = jumpInput || Input.GetKeyDown (KeyCode.Space);
	}
}
