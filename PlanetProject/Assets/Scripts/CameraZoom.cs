using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
	public Camera _camera;
	public SpriteGroup _clouds;
	public SpriteRenderer _sky;

	public GameManager _gameManager;

	public float _easingDuration;

	public WaveParams[] _waves;

	void Start ()
	{
		_gameManager.WaveEnd += OnWaveEnd;
	}

	void OnWaveEnd ()
	{
		var currentWave = _gameManager.Wave;
		if (currentWave < _waves.Length)
			EaseToWaveIndex (currentWave);
	}

	public void EaseToWaveIndex (int i)
	{
		StopAllCoroutines ();
		StartCoroutine (EaseToParams (_waves[i]));
	}

	IEnumerator EaseToParams (WaveParams wavesParams)
	{
		//var zoomVelocity = 0f;;
		var startTime = Time.time;

		var startZoom = _camera.orthographicSize;
		var targetZoom = wavesParams.cameraSize;

		var startCloudColor = _clouds.color;
		var targetCloudColor = wavesParams.cloudsColor;

		var startSkyColor = _sky.color;
		var targetSkyColor = wavesParams.skyColor;

		while (true) {
			var normalizedTime = Mathf.Clamp01 ((Time.time - startTime) / _easingDuration);
			if (normalizedTime >= 1f) {
				break;
			}

			// Camera zoom
			_camera.orthographicSize = Mathf.SmoothStep (startZoom, targetZoom, normalizedTime);

			// Clouds color
			_clouds.color = ColorSmoothStep (startCloudColor, targetCloudColor, normalizedTime);

			// Sky color
			_sky.color = ColorSmoothStep (startSkyColor, targetSkyColor, normalizedTime);

			yield return null;
		}
	}

	Color ColorSmoothStep (Color start, Color end, float t)
	{
		return new Color (
			Mathf.SmoothStep (start.r, end.r, t),
			Mathf.SmoothStep (start.g, end.g, t),
			Mathf.SmoothStep (start.b, end.b, t),
			Mathf.SmoothStep (start.a, end.a, t)
		);
	}
}

[System.Serializable]
public class WaveParams
{
	public float cameraSize;
	public Color skyColor;
	public Color cloudsColor;
}