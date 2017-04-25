using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {


	public Animator _intro;
	public CameraZoom _cameraZoom;


	public float delayExplosion = 2f;

	public float delayZoom = 1f;

	public float delayStart = 1f;

	public Animator _character;

	public AudioClip _impact;


	public DamageColorAnim _planetDamage;

	public void Play ()
	{
		StartCoroutine(WaitAndStart ());
	}

	IEnumerator WaitAndStart ()
	{
		yield return new WaitForSeconds (delayExplosion);

		_planetDamage.OnDamage ();
		SoundManager.instance.Play (_impact);

		var cameraShake = FindObjectOfType<OrthoCameraShake> ();
		cameraShake.TriggerShaking ();

		_character.Play ("intro");

		yield return new WaitForSeconds (delayZoom);

		_intro.SetBool ("zoom", true);
		_cameraZoom.EaseToWaveIndex (0);

		yield return new WaitForSeconds (delayStart);

		FindObjectOfType<GameManager> ().StartGame = true;
	}
}
