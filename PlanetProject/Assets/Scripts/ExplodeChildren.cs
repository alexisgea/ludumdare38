using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeChildren : MonoBehaviour {

	public bool destroySelf = true;

	public void Explode ()
	{
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive (true);
			transform.GetChild (i).SetParent (null);

		}

		if (destroySelf) {
			Destroy (gameObject);
		}
	}

}
