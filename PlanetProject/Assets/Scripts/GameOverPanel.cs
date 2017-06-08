using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {

	[SerializeField] GameObject GetNamePanel;
	[SerializeField] GameObject neighbourScoresPanel;
	[SerializeField] GameObject topScoresPanel;
	[SerializeField] ScoreLine scoreLinePrefab;
	[SerializeField] GameObject loadingMessage;
	[SerializeField] GameObject mainButtons;
	[SerializeField] InputField nameInput;
	[SerializeField] GameObject topScoresButton;
	[SerializeField] GameObject neighbourScoresButton;
	


	private string secretKey = "SuperSecretKey";
	private string addScoreURL = "http://asteroidlab.com/games/tinydefender/AddScore.php";
	private string updateNameURL = "http://asteroidlab.com/games/tinydefender/UpdateName.php";
	private string topScoresURL = "http://asteroidlab.com/games/tinydefender/TopScores.php";
	private string neighbourScoresURL = "http://asteroidlab.com/games/tinydefender/NeighbourScores.php";
	private string rankURL = "http://asteroidlab.com/games/tinydefender/GetRank.php?";


	private string lastName = "Tiny Def"; // TODO find a nice default name
	private string playerId;
	private int rank;

	private GameManager gameManager;


	private void Start() {
		gameManager = FindObjectOfType<GameManager>();
	}

	
	public void SubmitScore() {
		int score = (int)Mathf.Round(gameManager.TimeSurvived);
		Debug.Log("submitting score " + score + " for " + lastName);
		StartCoroutine(AddScore(lastName, score));
		GetNamePanel.SetActive(false);
		loadingMessage.SetActive(true);
		mainButtons.SetActive(true);
		ViewNeighbourScores();

	}

	public void UpdateName() {
		lastName = nameInput.text;

		// if(playerId != null) {
		// 	StartCoroutine(UpdateName(playerId, name));
		// }
	}

	public void ViewNeighbourScores() {
		topScoresPanel.SetActive(false);
		neighbourScoresPanel.SetActive(true);

		neighbourScoresButton.SetActive(false);
		topScoresButton.SetActive(true);
	}

	public void ViewTopScores() {
		topScoresPanel.SetActive(true);
		neighbourScoresPanel.SetActive(false);

		neighbourScoresButton.SetActive(true);
		topScoresButton.SetActive(false);
	}

	private void UpdateNeigbourScorePanel(string neighboursScores) {
		loadingMessage.SetActive(false);

		ScoreLineArraySerializable serverResults = JsonUtility.FromJson<ScoreLineArraySerializable>(neighboursScores);
		for(int i = 0; i < serverResults.ScoreLines.Length; i++) {
			ScoreLine newScoreLine = Instantiate(scoreLinePrefab, neighbourScoresPanel.transform);
			newScoreLine.UpdateLine(serverResults.ScoreLines[i].Rank,
									serverResults.ScoreLines[i].Name,
									serverResults.ScoreLines[i].Score);
		}
	}

	private void UpdateTopScorePanel(string topScores) {
		ScoreLineArraySerializable serverResults = JsonUtility.FromJson<ScoreLineArraySerializable>(topScores);
		for(int i = 0; i < serverResults.ScoreLines.Length; i++) {
			ScoreLine newScoreLine = Instantiate(scoreLinePrefab, topScoresPanel.transform);
			newScoreLine.UpdateLine((i+1).ToString(),
									serverResults.ScoreLines[i].Name,
									serverResults.ScoreLines[i].Score);
		}
	}


	private IEnumerator AddScore(string name, int score) {
	
		string hash = Md5Sum(name + score + secretKey);

        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("score", score);
        form.AddField("hash", hash);
        WWW postScoreAttempt = new WWW(addScoreURL, form);

        yield return postScoreAttempt;
	
		if (postScoreAttempt.error == null)	{
			playerId = postScoreAttempt.text;
            Debug.Log("Post score success with id: " + playerId);
			
			StartCoroutine(GetNeighbourScores(playerId, UpdateNeigbourScorePanel));
			StartCoroutine(GetTopScores(UpdateTopScorePanel));
        }
		else {
            Debug.LogError("Post score failure " + postScoreAttempt.error);			
		}
	}

	private IEnumerator GetNeighbourScores(string id, Action<string> successCallback) {

		string hash = Md5Sum(id + secretKey);

        WWWForm form = new WWWForm();
		form.AddField("id", id);
        form.AddField("hash", hash);
        WWW getNeighbourScoresAttempt = new WWW(neighbourScoresURL, form);
	
		yield return getNeighbourScoresAttempt;
	
		if (getNeighbourScoresAttempt.error == null) {
            Debug.Log("Get neighbour scores success:\n" + getNeighbourScoresAttempt.text);
            successCallback(getNeighbourScoresAttempt.text);
        }
		else {
			Debug.LogError("Get neighbour scores failure " + getNeighbourScoresAttempt.error);	
		}
	}

	private IEnumerator GetRank(string id) {

		string hash = Md5Sum(id + secretKey);

        WWWForm form = new WWWForm();
		form.AddField("id", id);
        form.AddField("hash", hash);
        WWW getRankAttempt = new WWW(rankURL, form);
	
		yield return getRankAttempt;
	
		if (getRankAttempt.error == null) {
			rank = System.Int32.Parse(getRankAttempt.text);
            Debug.Log("Get rank success: " + rank);
        }
		else {
			Debug.LogError("Get rank failure " + getRankAttempt.error);	
		}
	}

	private IEnumerator GetTopScores(Action<string> successCallback) {

		string time = Time.time.ToString();
		string hash = Md5Sum(time + secretKey);

        WWWForm form = new WWWForm();
        form.AddField("t", time);
        form.AddField("hash", hash);
        WWW getTopScoresAttempt = new WWW(topScoresURL, form);
	
		yield return getTopScoresAttempt;
	
		if (getTopScoresAttempt.error == null) {
            Debug.Log("Top scores:\n" + getTopScoresAttempt.text);
			successCallback(getTopScoresAttempt.text);
        }
		else {
			Debug.LogError("Get top failure " + getTopScoresAttempt.error);	
		}
	}

	private IEnumerator UpdateName(string id, string name) {
	
		string hash = Md5Sum(id + name + secretKey);

        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("name", name);
        form.AddField("hash", hash);
        WWW postNameAttempt = new WWW(updateNameURL, form);

        yield return postNameAttempt;
	
		if (postNameAttempt.error == null) {
            Debug.Log("Update name successful");
        }
		else {
            Debug.LogError("Updtate name failure " + postNameAttempt.error);			
		}
	}


	private  string Md5Sum(string strToEncrypt) {
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
	
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
	
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
	
		for (int i = 0; i < hashBytes.Length; i++) {
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
	
		return hashString.PadLeft(32, '0');
	}
}

[Serializable]
public class ScoreLineArraySerializable {

	public ScoreLineSerializable[] ScoreLines;

	public ScoreLineArraySerializable(ScoreLineSerializable[] scoreLines) {
		ScoreLines = scoreLines;
	}


}

[Serializable]
public class ScoreLineSerializable {
	public string Id;
	public string Rank;
	public string Name;
	public string Score;

	public ScoreLineSerializable(string id, string rank, string name, string score) {
		Id = id;
		Rank = rank;
		Name = name;
		Score = score;	
	}
}