using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {


	public Animator _intro;
	public CameraZoom _cameraZoom;


	public void Play ()
	{
		_intro.SetBool ("zoom", true);
		_cameraZoom.EaseToWaveIndex (0);
	}
}
