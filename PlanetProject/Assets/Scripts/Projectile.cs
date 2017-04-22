using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float timer = 2f;


	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
		if(timer < 0f) {
			Destroy(this.gameObject);
        }
    }

	void OnCollisionEnter2D(Collision2D other) {
        Destroy(this.gameObject);
    }




}
