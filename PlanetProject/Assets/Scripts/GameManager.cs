using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
    [SerializeField] float interWaveWaiter = 20f;
	private float interWaveWaitCounter;
    [SerializeField] float minSpawnDistance = 50;
	[SerializeField] float maxSpawnDistance = 100;
    [SerializeField] float spawnRateDivider = 20f;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] Transform asteroidGroup;

    private int maxAsteroid = 0;
	private int spawnedAsteroid = 0;
    private int waveCounter = 1;
	public int Wave {get { return waveCounter; } }
    private float spawnRate;

    private bool isInWave = false;

    private int ressources = 100;
	public int Ressources{get { return ressources; } }


    public event System.Action WaveStart;
    public event System.Action WaveEnd;
    public event System.Action GameOver;





    // Use this for initialization
    void Start () {
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
        spawnedAsteroid = 0;
        maxAsteroid = waveCounter * 10;
        spawnRate = maxAsteroid / (spawnRateDivider * 60f);
    }

	private void WaitForNetxtWave() {
        interWaveWaitCounter = interWaveWaiter;

    }

	private void SpawnAsteroid() {
		if(spawnedAsteroid <= maxAsteroid && Random.value < spawnRate) {
            spawnedAsteroid += 1;
            GameObject asteroid = Instantiate(asteroidPrefab, GetRandomSpawnLocation(),
			Quaternion.Euler(0,0,Random.Range(0,360)), asteroidGroup);
        }
	}

	private Vector2 GetRandomSpawnLocation() {
		return Random.insideUnitCircle.normalized * Random.Range(minSpawnDistance, maxSpawnDistance);
    }


}
