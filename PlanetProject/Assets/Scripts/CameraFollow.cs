using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform _player;

	void LateUpdate ()
	{
		var rot = transform.eulerAngles;
		rot.z = _player.eulerAngles.z;
		transform.eulerAngles = rot;
	}
}
