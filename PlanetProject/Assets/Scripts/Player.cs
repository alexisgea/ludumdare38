using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Transform graphics;
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

		var deltaSpeed = targetSpeed - velocity.x;

		rb.AddForce (new Vector2 (deltaSpeed, 0f), ForceMode2D.Force);


		//velocity.x = Mathf.MoveTowards (velocity.x, targetSpeed, acceleration * Time.deltaTime);

		//rb.AddRelativeForce (new Vector2(velocity.x, 0f));
		//print (velocity); 



		rb.velocity = transform.TransformVector(velocity);

		if (jumpInput) {
			jumpInput = false;
			rb.AddRelativeForce (new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}

		var delta = transform.position - planet.position;
		var rotation = Quaternion.LookRotation (Vector3.forward, delta);
		rb.MoveRotation (rotation.eulerAngles.z);

		if (Mathf.Abs (Input.GetAxisRaw ("Horizontal")) > 0.1f) {
			var rot = graphics.localEulerAngles;
			rot.y = targetSpeed < 0 ? 180f : 0f;
			graphics.localEulerAngles = rot;
		}
	}

	void Update ()
	{
		jumpInput = jumpInput || Input.GetKeyDown (KeyCode.Space);
	}
}
