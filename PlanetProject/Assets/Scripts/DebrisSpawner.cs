using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour {

	public GameObject debrisPrefab;
	public int count = 1;
	public float scaleRange = 0.5f;
	public float explosionForce = 10f;

	public void Spawn ()
	{
		for (int i = 0; i < count; i++) {
			GameObject debris = Instantiate(debrisPrefab, transform.position, Quaternion.Euler(0,0,Random.Range(0,360)),
				GameObject.Find("DebrisGroup").transform);
			debris.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * explosionForce;
			float scaleFactor = Random.Range(1 - scaleRange, 1 + scaleRange);
			debris.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
		}
	}
}
