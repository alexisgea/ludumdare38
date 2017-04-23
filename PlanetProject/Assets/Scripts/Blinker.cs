using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour {

    [SerializeField] float blinkSpeed = 10;

    private bool fading = true;

    // Use this for initialization
    void Start () {
        blinkSpeed = blinkSpeed / 255;
    }
	
	// Update is called once per frame
	void Update () {
        Color col = GetComponent<SpriteRenderer>().color;
		if(fading) {
			if(col.a < blinkSpeed) {
                fading = !fading;
                col.a = 0;
            }
			else {
                col.a -= blinkSpeed;
            }
        }
		else {
			if(col.a+blinkSpeed > 1) {
                fading = !fading;
                col.a = 1;
            }
			else {
                col.a += blinkSpeed;
            }
		}
		GetComponent<SpriteRenderer>().color = col;

		
	}
}
