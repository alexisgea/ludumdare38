using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [SerializeField] float range = 100;
	[SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] Transform mount;

    private Vector3 targetDir;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        targetDir = CheckForTarget();
        var targetRot = Quaternion.LookRotation(Vector3.forward, targetDir);
        mount.rotation = Quaternion.Lerp(mount.rotation, targetRot, rotationSpeed);
    }

    private Vector3 CheckForTarget() {
		Collider2D[] castResults = Physics2D.OverlapCircleAll(transform.position, range);


        if(castResults != null && castResults[0]) {
            return castResults[0].transform.position - transform.position;
        }
		else {
            return Vector3.zero;
        }
		
	}
}
