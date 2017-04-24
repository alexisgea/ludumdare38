using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {


	public Animator _intro;
	public CameraZoom _cameraZoom;

	public float delay = 1f;



	public void Play ()
	{
		_intro.SetBool ("zoom", true);
		_cameraZoom.EaseToWaveIndex (0);
		StartCoroutine(WaitAndStart ());
	}

	IEnumerator WaitAndStart ()
	{
		yield return new WaitForSeconds (delay);
		FindObjectOfType<GameManager> ().StartGame = true;
	}
}
