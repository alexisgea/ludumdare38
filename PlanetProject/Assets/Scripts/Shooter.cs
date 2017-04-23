using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

	[SerializeField] Rigidbody2D _projectilePrefab;
	[SerializeField] float _muzzleVelocity;

	public void Shoot ()
	{
		var rb = Instantiate<Rigidbody2D> (_projectilePrefab, transform.position, transform.rotation, transform);
		rb.AddForce (transform.up * _muzzleVelocity, ForceMode2D.Impulse);
	}
}
