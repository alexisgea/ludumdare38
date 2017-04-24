﻿using System.Collections;
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


	void Start ()
	{
		var rb = GetComponent<Rigidbody2D>();
		rb.centerOfMass = new Vector2(0f, 0f);
	}

	void FixedUpdate ()
	{
		var rb = GetComponent<Rigidbody2D>();

		var delta = transform.position - planet.position;
		var rotation = Quaternion.LookRotation (Vector3.forward, delta);
		rb.MoveRotation (rotation.eulerAngles.z);

		var velocity = transform.InverseTransformDirection(rb.velocity);

		var targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;

		//var deltaSpeed = targetSpeed - velocity.x;

		//rb.AddRelativeForce (new Vector2 (deltaSpeed, 0f) * rb.mass, ForceMode2D.Force);

		velocity.x = Mathf.MoveTowards (velocity.x, targetSpeed, acceleration * Time.deltaTime);

		//rb.AddRelativeForce (new Vector2(velocity.x, 0f));
		//print (velocity); 

		rb.velocity = transform.TransformVector(velocity);

		if (jumpInput) {
			jumpInput = false;
			rb.AddRelativeForce (new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}


		if (Mathf.Abs (Input.GetAxisRaw ("Horizontal")) > 0.1f) {
			var rot = graphics.localEulerAngles;
			rot.y = targetSpeed < 0 ? 180f : 0f;
			graphics.localEulerAngles = rot;
		}
	}

	void Update ()
	{
		float playerHeight = (Vector3.zero - transform.position).sqrMagnitude;
		float allowedHeight =  Mathf.Clamp(Mathf.Pow(12.5f + FindObjectOfType<GameManager>().Wave * 1.2f, 2), 0f, Mathf.Pow(25,2));
		
		if(playerHeight < allowedHeight)
			jumpInput = jumpInput || Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.W);

		if (Input.GetKeyDown (KeyCode.J)) {
			GetComponentInChildren<Shooter>().Shoot ();
		}
	}
}
