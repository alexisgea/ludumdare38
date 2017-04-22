using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {

	public Transform _explosionPrefab;

	void OnCollisionEnter2D (Collision2D collider)
	{
		print (collider.collider.name);
		if (collider.gameObject.layer != LayerMask.NameToLayer ("asteroid")) {
			Destroy (gameObject);

			if (_explosionPrefab != null) {
				Instantiate (_explosionPrefab);
			}
		}
	}
}
