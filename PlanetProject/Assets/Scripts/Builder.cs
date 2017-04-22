﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour {

	public Transform _planet;
	public Transform _cratePrefab;
	public Transform _turretPrefab;

	public Transform _preview;

	public float _horisontalDistance = 1f;
	public float _buildHigher = 5f;
	public float _buildLower = 4f;

	private bool _placeInput;

    private GameManager gameManager;

    [SerializeField] Transform buildGroup;

	void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

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


		var canBuild = hit.collider != null;
		_preview.gameObject.SetActive (canBuild);

		if (!canBuild)
			return;
		

		var targetPosition = castOrigin - upDir * hit.distance;
		Debug.DrawLine (castOrigin, targetPosition, Color.magenta);

		_preview.rotation = boxRotation;
		_preview.position = targetPosition;

		if (Input.GetKeyDown (KeyCode.J) && gameManager.Ressources > _cratePrefab.GetComponent<Buildable>().Cost)  {
			var newCrate = Instantiate<Transform>(_cratePrefab, targetPosition, boxRotation, buildGroup);
            gameManager.Ressources -= _cratePrefab.GetComponent<Buildable>().Cost;
        }

		if (Input.GetKeyDown (KeyCode.K) && gameManager.Ressources > _turretPrefab.GetComponent<Buildable>().Cost) {
			var newTurret = Instantiate<Transform>(_turretPrefab, targetPosition, boxRotation, buildGroup);
            gameManager.Ressources -= _turretPrefab.GetComponent<Buildable>().Cost;
		}
	}

	void DebugPoint (Vector2 point, Color col)
	{
		Debug.DrawLine (Vector2.zero, point, col);
	}

}
