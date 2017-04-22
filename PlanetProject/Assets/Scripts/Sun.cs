using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

    [SerializeField] float rotationSpeed = 0.01f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rotation = transform.eulerAngles;
        rotation.z += rotationSpeed;

		if (rotation.z > 360) {
            rotation.z -= 360;
        }

        transform.eulerAngles = rotation;

    }
}
