using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
    [SerializeField] float interWaveWaiter = 2f;
	public float InterWaveWaiter {get { return interWaveWaiter; } }
	private float interWaveWaitCounter;
    [SerializeField] float minSpawnDistance = 50;
	[SerializeField] float maxSpawnDistance = 100;
    [SerializeField] float spawnRateDivider = 20f;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] Transform asteroidGroup;

	[SerializeField] float asteroidsMinLife;
	[SerializeField] float asteroidsMaxLife;
	[SerializeField] float asteroidsLifeExtraScale;

    private int maxAsteroid = 0;
	private int spawnedAsteroid = 0;
    private int waveCounter = 0;
	public int Wave {get { return waveCounter; } }
    private float spawnRate;

    private bool isInWave = false;
    [SerializeField] int startRessources = 100;
    private int ressources;
    public int Ressources {
        set {
            ressources = value;

            if(RessourcesChanged != null)
				RessourcesChanged.Invoke();
        }
        get {return ressources; }
    }
	//public int Ressources{get { return ressources; } }


    public event System.Action WaveStart;
    public event System.Action WaveEnd;
    public event System.Action GameOver;
    public event System.Action RessourcesChanged;





    // Use this for initialization
    void Start () {
        Ressources = startRessources;
        WaitForNetxtWave();

    }
	
	// Update is called once per frame
	void Update () {

        if(!isInWave) {
            interWaveWaitCounter -= Time.deltaTime;
			if(interWaveWaitCounter <= 0) {
				isInWave = true;
                StartNewWave();
            }
        }
		else {
            SpawnAsteroid();
            if(asteroidGroup.childCount == 0 && spawnedAsteroid == maxAsteroid) {
				isInWave = false;
                WaitForNetxtWave();
            }
		}
		
	}

	private void StartNewWave() {
        waveCounter += 1;
        Debug.Log("Start Wave " + waveCounter);
        spawnedAsteroid = 0;
        maxAsteroid = waveCounter * 10;
        spawnRate = maxAsteroid / (spawnRateDivider * 60f);
        RaiseWaveStart();
    }

	private void WaitForNetxtWave() {
        Debug.Log("Wave End");
		
        interWaveWaitCounter = interWaveWaiter;
        RaiseWaveEnd();
		

    }

	private void SpawnAsteroid() {
		if(spawnedAsteroid < maxAsteroid && Random.value < spawnRate) {
            spawnedAsteroid += 1;

			// Instantiate
			var asteroid = Instantiate(asteroidPrefab, GetRandomSpawnLocation(), Quaternion.Euler(0,0,Random.Range(0,360)), asteroidGroup);

			// Health and damages
			var asteroidDestroyable = asteroid.GetComponent<Destroyable> ();
			asteroidDestroyable.lifePoints = Random.Range(asteroidsMinLife, asteroidsMaxLife + Wave);
			asteroidDestroyable.dealDamages = Mathf.Max (1f, asteroidDestroyable.lifePoints / 10);

			// Scale
			var baseScale = transform.localScale.x;
			float scaleBonus = asteroidDestroyable.lifePoints * asteroidsLifeExtraScale;
			transform.localScale = new Vector3(baseScale + scaleBonus, baseScale + scaleBonus, 1f);
        }
			
	}

	private Vector2 GetRandomSpawnLocation() {
		return Random.insideUnitCircle.normalized * Random.Range(minSpawnDistance, maxSpawnDistance);
    }

	private void RaiseWaveStart() {
		if(WaveStart != null) {
            WaveStart.Invoke();
        }
	}

	private void RaiseWaveEnd() {
		if(WaveEnd != null) {
            WaveEnd.Invoke();
        }
	}

	public void RaiseGameOver() {
		if(GameOver != null) {
            GameOver.Invoke();
        }
    }


}
