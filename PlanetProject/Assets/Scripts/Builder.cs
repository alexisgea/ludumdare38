﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour {

	public Transform _planet;
	public Transform _cratePrefab;
	public Transform _turretPrefab;

	public SpriteRenderer _preview;

	public float _horisontalDistance = -1.2f;
	public float _buildHigher = 5f;
	public float _buildLower = 4f;
	public LayerMask _layerMask;


	private bool _buildInput;

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

		var size = _cratePrefab.GetComponent<BoxCollider2D> ().size * _cratePrefab.localScale.x;

		var hit = Physics2D.BoxCast (castOrigin, size, boxRotation.eulerAngles.z, -upDir, _buildHigher + _buildLower, _layerMask);

		var spaceAvaliable = hit.collider != null && hit.distance != 0f;
		_preview.gameObject.SetActive (spaceAvaliable);

		var onTopOfTurret = hit.collider != null && LayerMask.LayerToName (hit.collider.gameObject.layer) == "turret";
		_preview.color = onTopOfTurret ? Color.red : Color.white;

		if (!spaceAvaliable) 
			return;

		var targetPosition = castOrigin - upDir * hit.distance;
		Debug.DrawLine (castOrigin, targetPosition, Color.magenta);

		_preview.transform.rotation = boxRotation;
		_preview.transform.position = targetPosition;

		if (onTopOfTurret)
			return;

		if (Input.GetKeyDown (KeyCode.K)) {
			Build (_cratePrefab, targetPosition, boxRotation);
		}

		if (Input.GetKeyDown (KeyCode.L)) {
			Build (_turretPrefab, targetPosition, boxRotation);
		}

//		if (Input.GetKeyDown (KeyCode.K) && gameManager.Ressources >= _cratePrefab.GetComponent<Buildable>().Cost)  {
//			Instantiate<Transform>(_cratePrefab, targetPosition, boxRotation, buildGroup);
//            gameManager.Ressources -= _cratePrefab.GetComponent<Buildable>().Cost;
//        }
//
//		if (Input.GetKeyDown (KeyCode.L) && gameManager.Ressources >= _turretPrefab.GetComponent<Buildable>().Cost) {
//			Instantiate<Transform>(_turretPrefab, targetPosition, boxRotation, buildGroup);
//            gameManager.Ressources -= _turretPrefab.GetComponent<Buildable>().Cost;
//		}
	}

	void Build (Transform prefab, Vector3 position, Quaternion rotation)
	{
		var buildable = prefab.GetComponent<Buildable> ();
		var canBuild = gameManager.CanBuild (buildable);
		if (canBuild) {
			var newBuilding = Instantiate<Transform>(prefab, position, rotation, buildGroup);
			gameManager.Build (newBuilding.GetComponent<Buildable>());
		}
	}

	void DebugPoint (Vector2 point, Color col)
	{
		Debug.DrawLine (Vector2.zero, point, col);
	}

}
