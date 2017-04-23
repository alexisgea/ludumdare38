using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RessourceCollector : MonoBehaviour {

    Rigidbody2D playerBody;
    GameManager gameManager;

    // Use this for initialization
    void Start () {
        playerBody = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }
	

	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("debris")) {
        	Debug.Log("TAG TAG TAG");
            gameManager.Ressources += other.gameObject.GetComponent<Debris>().RessourceValue;
            Destroy(other.gameObject);
        }
	}
}
