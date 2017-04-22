﻿using System.Collections;
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
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text tempScoreField;



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
		waveMessage.DisplayMessage("Wave finished \n next wave in " + gameManager.InterWaveWaiter + " sec");
	}

	private void OnRessourcesChanged() {
        ressourceCounter.GetComponentInChildren<Text>().text = gameManager.Ressources.ToString();
    }

	private void OnGameOver() {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }


    public void Retry() {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        tempScoreField.text = gameManager.Wave.ToString();
    }

	
}
