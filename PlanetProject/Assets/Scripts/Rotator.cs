using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	public float _speed = 10f;

	void Update ()
	{
		transform.Rotate (0f, 0f, _speed * Time.deltaTime);
	}
}
