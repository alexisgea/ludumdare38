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
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text tempScoreField;
    [SerializeField] GameObject SplashMenu;
    [SerializeField] GameObject CreditsScreen;



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
		waveMessage.DisplayMessage("Wave finished \n next wave in " + gameManager.InterWaveWaiter + " sec");
	}

	private void OnRessourcesChanged() {
        ressourceCounter.GetComponentInChildren<Text>().text = gameManager.Ressources.ToString();
    }

	private void OnGameOver() {
        tempScoreField.text = "You reached wave " + gameManager.Wave.ToString();
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
        gameManager.StartGame = true;
    }

    public void ViewCredit() {
        SplashMenu.SetActive(false);
    }
}
