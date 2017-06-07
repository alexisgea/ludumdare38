using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BuildableType {crate, turret}

public class Buildable : MonoBehaviour {

    public BuildableType Type = BuildableType.crate;
    public int Cost = 0;
	public int MaxCount = 0; // 0 = no limit

	public UnityEvent OnDestroyed;

	void OnDestroy ()
	{
		OnDestroyed.Invoke ();
	}
}
