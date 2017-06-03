using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AsteroidWarnings : MonoBehaviour
{
	public float _scale = 1f;

	public Transform _arrowPrefab;

	Bounds _bounds;

	List<Transform> _asteroids = new List<Transform> ();
	List<Transform> _arrows = new List<Transform> ();

	void Start ()
	{
		var manager = FindObjectOfType<GameManager> ();
		manager.AsteroidSpawned.AddListener ((spawned) => _asteroids.Add (spawned));
	}

	void Update ()
	{
		UpdateBounds ();
		UpdateArrows ();
	}

	void UpdateArrows ()
	{
		var arrowsEnumerator = _arrows.GetEnumerator ();

		foreach (var asteroid in _asteroids.Where(a => a != null)) {
			Transform nextArrow = null;
			if (arrowsEnumerator.MoveNext ()) {
				nextArrow = arrowsEnumerator.Current;
			} else {
				nextArrow = NewArrow ();
				_arrows.Add (nextArrow);
			} 

			nextArrow.position = _bounds.ClosestPoint (asteroid.position);
			nextArrow.gameObject.SetActive (true);
		}

//		while (arrowsEnumerator.MoveNext ()) {
//			arrowsEnumerator.Current.gameObject.SetActive (false);
//		}
	}

	Transform NewArrow ()
	{
		return Instantiate<Transform> (_arrowPrefab);
	}

	void UpdateBounds ()
	{
		var position = Camera.main.transform.position;
		position.z = 0f;

		_bounds = new Bounds(position, new Vector3 (Camera.main.orthographicSize * _scale, Camera.main.orthographicSize / Camera.main.aspect * _scale));
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube (_bounds.center, _bounds.size);
	}
}
