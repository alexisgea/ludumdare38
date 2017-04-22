using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour {

	public Transform _planet;
	public Transform _cratePrefab;

	public Transform _preview;

	public float _horisontalDistance = 1f;
	public float _buildHigher = 5f;
	public float _buildLower = 4f;

	private bool _placeInput;

	void Update ()
	{
		var spawnTarget = transform.position + transform.right * _horisontalDistance;
		var upDir = (spawnTarget - _planet.position).normalized;


		var castOrigin = spawnTarget + upDir * _buildHigher;
		//_crate.position = transform.position;
		//var col = _crate.GetComponent<Collider2D> ();

		DebugPoint (castOrigin, Color.yellow);
		DebugPoint (upDir, Color.green);

		var boxRotation = Quaternion.LookRotation (Vector3.forward, upDir);

//		var mask = LayerMask.GetMask (new string[] { "" })
		var hit = Physics2D.BoxCast (castOrigin, Vector2.one, boxRotation.eulerAngles.z, -upDir, _buildHigher + _buildLower);

		_preview.gameObject.SetActive (hit.collider != null);

		var targetPosition = castOrigin - upDir * hit.distance;
		Debug.DrawLine (castOrigin, targetPosition, Color.magenta);

		_preview.rotation = boxRotation;
		_preview.position = targetPosition;

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			var newCrate = Instantiate<Transform>(_cratePrefab, targetPosition, boxRotation);
		}
	}

	void DebugPoint (Vector2 point, Color col)
	{
		Debug.DrawLine (Vector2.zero, point, col);
	}

//	void Update ()
//	{
//	}
}
