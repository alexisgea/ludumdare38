using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {

	public Transform _explosionPrefab;
    [SerializeField] float explosionForce = 1f;
    [SerializeField] GameObject debrisPrefab;
    [SerializeField] float debrisScaleRange = 0.5f;

    public float LifePoints{ set; get; }
    private float startLifePoints;
    [SerializeField] float minLifePoint = 5;
    [SerializeField] float maxLifePoint = 15;
    [SerializeField] float scaleDevider = 8;
	public LayerMask _dealDamagesToLayer;

    public float DealDamages = 1f;

    public bool _destroyOnDeath = true;
    [SerializeField] bool autoDestroy = false;
    [SerializeField] float autoDestroyTimer = 2f;

    public UnityEngine.Events.UnityEvent _onDeath;


    void Start() {

		if(gameObject.layer == LayerMask.NameToLayer("target")) {
            LifePoints = Random.Range(minLifePoint, maxLifePoint + FindObjectOfType<GameManager>().Wave);
            //Debug.LogWarning("LP " + LifePoints);
            startLifePoints = LifePoints;
            DealDamages = LifePoints / 10;
            DealDamages = DealDamages <= 0f ? 1f : DealDamages;
            float scaleFactor = LifePoints / scaleDevider;
			transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
		}
    }

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(autoDestroy) {
            autoDestroyTimer -= Time.deltaTime;
			if(autoDestroyTimer <= 0f)
                Destroy(this.gameObject);
        }
	}

	void OnCollisionEnter2D (Collision2D collider)
	{
		// Dark magic
		var isInLayermask = _dealDamagesToLayer == (_dealDamagesToLayer | (1 << collider.gameObject.layer));

		if (isInLayermask)
		{
			// We want to hurt the thing we touched! -- Kamikaz
			Die ();

			// Damagable
			var other = collider.gameObject.GetComponent<Destroyable> ();
			if (other != null) {
				//Debug.LogFormat ("{0} dealing {1} damage(s) to {2}", this.name, DealDamages, collider.gameObject.name);
				other.Dammage (DealDamages);
			}
		}

		else if(autoDestroy) {
            Destroy(this.gameObject);
        }
	}

	public void Dammage (float amount)
	{
		LifePoints -= amount;

        if (LifePoints <= 0f) {
			Die ();
		}
	}

	public void Die ()
	{
		_onDeath.Invoke ();

		if (_destroyOnDeath) {
			Destroy (gameObject);
		}

		if (_explosionPrefab != null) {
			Instantiate (_explosionPrefab);
		}

		if(debrisPrefab != null) {
            for (int i = 0; i < startLifePoints; i++) {
                GameObject debris = Instantiate(debrisPrefab, transform.position, Quaternion.Euler(0,0,Random.Range(0,360)),
				GameObject.Find("DebrisGroup").transform);
                debris.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * explosionForce;
                float scaleFactor = Random.Range(1 - debrisScaleRange, 1 + debrisScaleRange);
                debris.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            }

        }
	}
}
