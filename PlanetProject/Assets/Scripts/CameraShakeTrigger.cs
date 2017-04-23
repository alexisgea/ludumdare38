using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeTrigger : MonoBehaviour {

	public void Shake ()
	{
		var cameraShake = FindObjectOfType<OrthoCameraShake> ();
		cameraShake.TriggerShaking ();
	}
}
