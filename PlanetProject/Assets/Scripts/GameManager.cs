using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AsteroidEvent : UnityEngine.Events.UnityEvent<Transform>
{
	
}

public class GameManager : MonoBehaviour {
	
    [SerializeField] float interWaveWaiter = 2f;
	public float InterWaveWaiter {get { return interWaveWaiter; } }
	private float interWaveWaitCounter;
    [SerializeField] int asteroidDensity = 3;
    [SerializeField] float spawAngleSpread = 15f;
    [SerializeField] float minSpawnDistance = 50;
	[SerializeField] float maxSpawnDistance = 100;
    [SerializeField] float spawnRateDivider = 20f;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] Transform asteroidGroup;
    [SerializeField] Transform waveWarningGroup;
    [SerializeField] GameObject waveWarningPrefab;

	[SerializeField] float asteroidsMinLife;
	[SerializeField] float asteroidsMaxLife;
	[SerializeField] float asteroidsLifeExtraScale;

    [SerializeField] SoundPlayer waveStart;
    [SerializeField] SoundPlayer waveEnd;
    [SerializeField] SoundPlayer gameOver;
    
	Dictionary<BuildableType, int> buildingsCounts;

    private bool startGame = false;
    public bool StartGame {
        set {
            WaitForNetxtWave();
            startGame = value;
        }
		get {
			return startGame;
		}
    }

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

    private Vector2[] asteroidSources;


    public event System.Action WaveStart;
    public event System.Action WaveEnd;
    public event System.Action GameOver;
    public event System.Action RessourcesChanged;

	public AsteroidEvent AsteroidSpawned = new AsteroidEvent();

    // Use this for initialization
    void Start () {
        Ressources = startRessources;
        //WaitForNetxtWave();

		// Initialize building counts
		buildingsCounts = new Dictionary<BuildableType, int> ();
		var types = EnumUtil.GetValues<BuildableType> ();
		foreach (var type in types) {
			buildingsCounts.Add (type, 0);
		}
    }
	
	// Update is called once per frame
	void Update () {

        if(startGame) {
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

        if(Input.GetKeyDown(KeyCode.Escape)) {
            Time.timeScale = 0;
            FindObjectOfType<UiManager>().ViewCredit();
        }

		
	}

	private void StartNewWave() {
        waveCounter += 1;
        Debug.Log("Start Wave " + waveCounter);
        spawnedAsteroid = 0;
        maxAsteroid = waveCounter * asteroidDensity;
        spawnRate = maxAsteroid / (spawnRateDivider * 60f);

        DestroyWaveWarnings();
        waveStart.Play();
        RaiseWaveStart();
    }

	private void WaitForNetxtWave() {
        Debug.Log("Wave End");
		
        interWaveWaitCounter = interWaveWaiter;

        PrepareNextWaveSources();
        UpdateWaveWarnings();
        waveEnd.Play();
        RaiseWaveEnd();
		

    }

	private void SpawnAsteroid() {
		if(spawnedAsteroid < maxAsteroid && Random.value < spawnRate) {
            spawnedAsteroid += 1;

			// Instantiate
			var asteroid = Instantiate(asteroidPrefab, GetRandomSpawnLocation(), Quaternion.Euler(0,0,Random.Range(0,360)), asteroidGroup);
			// Health and damages
			var asteroidDestroyable = asteroid.GetComponent<Destroyable> ();
			asteroidDestroyable.lifePoints = Random.Range(asteroidsMinLife, asteroidsMaxLife + Wave * 3);
			asteroidDestroyable.dealDamages = Mathf.Max (1f, asteroidDestroyable.lifePoints / 10);
            float scaleFactor = asteroidDestroyable.lifePoints / (asteroidsMaxLife+5);
            asteroid.transform.localScale = Vector3.one * scaleFactor;
            int debrisCount = (int)(asteroid.GetComponent<DebrisSpawner>().count * Mathf.Min(scaleFactor,1)); 
            asteroid.GetComponent<DebrisSpawner>().count = debrisCount; 

            

			// Scale
			var baseScale = transform.localScale.x;
			float scaleBonus = asteroidDestroyable.lifePoints * asteroidsLifeExtraScale;
			transform.localScale = new Vector3(baseScale + scaleBonus, baseScale + scaleBonus, 1f);

			// Event
			AsteroidSpawned.Invoke (asteroid.transform);
        }

	}

	public bool CanBuild (Buildable prefab)
	{
		return prefab.Cost <= this.Ressources
			&& !BuildingMaxCountReached (prefab);
	}

	public bool BuildingMaxCountReached (Buildable prefab)
	{
		return prefab.MaxCount != 0 && buildingsCounts[prefab.Type] >= prefab.MaxCount;
	}

	public void Build (Buildable instance)
	{
		// Consume the resources
		this.Ressources -= instance.Cost;

		// Increment the building count for this type
		buildingsCounts[instance.Type]++;

		// Subscribe to event in order to decrement the building count when this building is destroyed 
		instance.OnDestroyed.AddListener (() => {
			buildingsCounts[instance.Type]--;
		});
	}
		
	private Vector2 GetRandomSpawnLocation() {
		Vector2 spawnDir = Quaternion.Euler(0, 0, Random.Range(-spawAngleSpread, spawAngleSpread)) * asteroidSources[Random.Range(0, asteroidSources.Length)];
        return spawnDir * Random.Range(minSpawnDistance, maxSpawnDistance);
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
        gameOver.Play();
    }

    private void PrepareNextWaveSources() {

        asteroidSources = new Vector2[1 + (int)((Wave)/2)];

        for (int i = 0; i < asteroidSources.Length; i++) {
            asteroidSources[i] = Random.insideUnitCircle.normalized;
        }

    }

    private void UpdateWaveWarnings() {

        DestroyWaveWarnings();

        //Bounds camBounds = new Bounds(Vector3.zero, Vector3())

        foreach(Vector2 direction in asteroidSources) {
            Instantiate(waveWarningPrefab, (Vector3)direction * 15,
            Quaternion.LookRotation(Vector3.forward, (Vector3)direction) , waveWarningGroup);
        }
    }

    private void DestroyWaveWarnings() {
        for (int i = 0; i < waveWarningGroup.childCount; i++) {
            Destroy(waveWarningGroup.GetChild(i).gameObject);
        }
    }


}
	

public static class EnumUtil {
	public static IEnumerable<T> GetValues<T>() {
		return System.Enum.GetValues(typeof(T)).Cast<T>();
	}
}