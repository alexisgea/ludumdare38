using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColorAnim : MonoBehaviour {

	public float _duration = 1f;

	public Color _damageColor = Color.red;

	private Color _startColor;

	private SpriteGroup _group;

	private SpriteRenderer _renderer;

	void Start ()
	{
		_group = GetComponent<SpriteGroup> ();

		if (_group == null)
			_renderer = GetComponent<SpriteRenderer> ();
		
		_startColor = GetColor ();

		GetComponent<Destroyable>()._onDamage.AddListener (OnDamage);
	}

	public void OnDamage ()
	{
		StopAllCoroutines ();
		StartCoroutine (Animate());
	}

	IEnumerator Animate ()
	{
		var startTime = Time.time;

		while (true) {
			var normalizedTime = (Time.time - startTime) / _duration;

			if (normalizedTime >= 1f)
				break;

			SetColor (Color.Lerp (_damageColor, _startColor, normalizedTime));

			yield return null;
		}
	}

	void SetColor (Color col)
	{
		if (_group != null)
			_group.color = col;
		else
			_renderer.color = col;
	}

	Color GetColor ()
	{
		if (_group != null)
			return _group.color;
		else
			return _renderer.color;
	}
}
