using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    GameManager gameManager;
    [SerializeField] GameObject waveCounter;
    [SerializeField] GameObject ressourceCounter;
    [SerializeField] GameObject boxItem;
    [SerializeField] GameObject turretItem;
    [SerializeField] TextFade waveMessage;
    [SerializeField] Text gameOver;
	


    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.WaveEnd += OnWaveEnd;
        gameManager.WaveStart += OnWaveStart;
        gameManager.GameOver += OnGameOver;
        gameManager.RessourcesChanged += OnRessourcesChanged;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnWaveStart() {
        waveMessage.DisplayMessage("Wave " + gameManager.Wave + " starts");
    }

	private void OnWaveEnd() {
		waveMessage.DisplayMessage("Wave finished /n next wave in " + gameManager.InterWaveWaiter + " sec");
	}

	private void OnRessourcesChanged() {
        ressourceCounter.GetComponentInChildren<Text>().text = gameManager.Ressources.ToString();
    }

	private void OnGameOver() {

	}

	
}
