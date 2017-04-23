﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {

	public Transform _explosionPrefab;

	public float _lifePoints = 10f;

	public LayerMask _dealDamagesToLayer;

	public float _dealDamages = 1f;

	public bool _destroyOnDeath = true;

	public UnityEngine.Events.UnityEvent _onDeath;

	void OnCollisionEnter2D (Collision2D collider)
	{
		// Dark magic
		var isInLayermask = _dealDamagesToLayer == (_dealDamagesToLayer | (1 << collider.gameObject.layer));

		if (isInLayermask)
		{
			// We want to hurt the thing we touched! -- Kamikaz
			Die ();

			// Damagable
			var other = collider.gameObject.GetComponent<Destroyable> ();
			if (other != null) {
				Debug.LogFormat ("{0} dealing {1} damage(s) to {2}", this.name, _dealDamages, collider.gameObject.name);
				other.Dammage (_dealDamages);
			}
		}
	}

	public void Dammage (float amount)
	{
		_lifePoints -= amount;

		if (_lifePoints <= 0f) {
			Die ();
		}
	}

	public void Die ()
	{
		_onDeath.Invoke ();

		if (_destroyOnDeath) {
			Destroy (gameObject);
		}

		if (_explosionPrefab != null) {
			Instantiate (_explosionPrefab);
		}
	}
}
