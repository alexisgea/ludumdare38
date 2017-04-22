using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [SerializeField] float range = 100;
	[SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] float firingArc = 60;
    [SerializeField] Transform mount;
	[SerializeField] Transform gunTip;
    [SerializeField] float RateOfFire = 6f;
    [SerializeField] float muzzleVelocity;
    [SerializeField] GameObject projectilePrefab;
	[SerializeField] Transform projectileGroup;

	private Transform target;
    private Vector3 toTarget;
    private float lastFire = 0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        CheckForTarget();
        AdjustAimAndShoot();

    }

    private void CheckForTarget() {

        LayerMask targetLayer = LayerMask.GetMask(new string[] { "target" });
        Collider2D[] castResults = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);

        if(castResults.Length > 0) {
			target = castResults[0].transform;
			foreach(Collider2D potentialTarget in castResults) {
				if((transform.position - potentialTarget.transform.position).magnitude > (transform.position - target.transform.position).magnitude){
					target = potentialTarget.transform;
				}
			}
        }
		else if (target != null) {
            target = null;
        }
		
	}

	private void AdjustAimAndShoot() {

        lastFire += Time.deltaTime;

        if(target != null) {

            Vector3 toTarget = target.position - transform.position;
            float angleToTarget = Vector2.Angle(toTarget, transform.up);

			if(Mathf.Abs(angleToTarget) < firingArc) {
				Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, toTarget);
				mount.rotation = Quaternion.Lerp(mount.rotation, targetRot, rotationSpeed);

				if(lastFire >= RateOfFire/100)
					Shoot();
			}
			else {
        		mount.rotation = Quaternion.Lerp(mount.rotation, Quaternion.identity, rotationSpeed);
			}
		}
		else {
        	mount.rotation = Quaternion.Lerp(mount.rotation, Quaternion.identity, rotationSpeed);
		}
	}

	private void Shoot() {
        lastFire = 0;
        GameObject projectile = Instantiate(projectilePrefab, gunTip.position, gunTip.rotation, projectileGroup);
        projectile.GetComponent<Rigidbody2D>().velocity = gunTip.rotation * Vector2.up * muzzleVelocity;
    }
}
