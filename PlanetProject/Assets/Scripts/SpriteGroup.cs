using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGroup : MonoBehaviour {

	private SpriteRenderer[] _renderers;

	[SerializeField]
	private Color _color = Color.white;

	// Small letter on public property to be consistent with unity apis
	public Color color {
		set {
			_color = value;

			foreach(var spriteRenderer in _renderers) {
				spriteRenderer.color = _color;
			}
		}
		get {
			return _color;
		}
	}

	void Start ()
	{
		_renderers = GetComponentsInChildren<SpriteRenderer> ();
	}
}
