using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Camera))]
public class OrthoCameraShake
	: MonoBehaviour
{
	public float Amplitude = 1f;
	public float AmplitudeVariation = 0.25f;
	public float PeekDuration = 0.1f;
	public float EaseInDuration = 0.05f;
	public float EaseOutDuration = 0.2f;

	float _startTime;
	Vector3 _startPosition;

	[ContextMenu("Shake")]
	public void TriggerShaking()
	{
		_startTime = Time.unscaledTime;
	}

	// Use this for initialization
	void Start()
	{
		_startPosition = transform.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
		if (_startTime != 0)
		{
			// Compute amplitude based on elapsed time
			float nominalAmpl = 0;

			float diff = Time.unscaledTime - _startTime;
			if (diff < EaseInDuration)
			{
				nominalAmpl = Mathf.Lerp(0, Amplitude, diff / EaseInDuration);
			}
			else
			{
				diff -= EaseInDuration;
				if (diff < PeekDuration)
				{
					nominalAmpl = Amplitude;
				}
				else
				{
					diff -= PeekDuration;
					if (diff < EaseOutDuration)
					{
						nominalAmpl = Mathf.Lerp(Amplitude, diff / EaseOutDuration, 0);
					}
					else
					{
						_startTime = 0;
					}
				}
			}

			// Reset to original position
			transform.localPosition = _startPosition;

			// Apply some random shaking
			if (nominalAmpl > 0)
			{
				if (AmplitudeVariation > 0)
				{
					// Add some variation to the amplitude
					nominalAmpl *= 1 + AmplitudeVariation * (2 * Random.value - 1);
				}
				var randDir = Random.insideUnitCircle.normalized;
				transform.localPosition += (Vector3)(randDir * nominalAmpl);
			}
		}
	}
}
