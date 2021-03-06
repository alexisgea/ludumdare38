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
    [SerializeField] Text waveReachedField;
    [SerializeField] Text timeSurviveField;
    [SerializeField] GameObject SplashMenu;
    [SerializeField] GameObject CreditsScreen;

	public Intro intro;
    private bool introStarted = false;
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
        waveCounter.GetComponentInChildren<Text>().text = gameManager.Wave.ToString();
    }

	private void OnWaveEnd() {
        if(gameManager.Wave == 0) {
		    waveMessage.DisplayMessage("Get Ready!\n first wave in " + gameManager.InterWaveWaiter + " sec");

        }
        else {
		    waveMessage.DisplayMessage("Wave finished \n next wave in " + gameManager.InterWaveWaiter + " sec");

        }
	}

	private void OnRessourcesChanged() {
        ressourceCounter.GetComponentInChildren<Text>().text = gameManager.Ressources.ToString();
    }

	private void OnGameOver() {
        waveReachedField.text = "You reached wave " + gameManager.Wave.ToString();
        timeSurviveField.text = "and survived for " + gameManager.TimeSurvived.ToString();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }


    public void Retry() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        Time.timeScale = 1;
    }

    public void Quit() {
        Application.Quit();
    }

	public void PlayGame() {
        SplashMenu.SetActive(false);
        CreditsScreen.SetActive(false);

        if(Time.timeScale == 0){
            Time.timeScale = 1;
        }

        if(!introStarted) {
		    intro.Play ();
            introStarted = true;
        }

        //gameManager.StartGame = true;
    }

    public void ViewCredit() {
        SplashMenu.SetActive(false);
		CreditsScreen.SetActive(true);
    }
}
