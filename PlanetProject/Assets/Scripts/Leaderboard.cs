using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Leaderboard : MonoBehaviour {

	private string secretKey = "SuperSecretKey";
	private string addScoreURL = "http://asteroidlab.com/games/tinydefender/AddScore.php";
	private string updateNameURL = "http://asteroidlab.com/games/tinydefender/UpdateName.php";
	private string topScoresURL = "http://asteroidlab.com/games/tinydefender/TopScores.php";
	private string neighbourScoresURL = "http://asteroidlab.com/games/tinydefender/NeighbourScores.php?";
	private string rankURL = "http://asteroidlab.com/games/tinydefender/GetRank.php?";


	private string lastName = "Tiny Def"; // TODO find a nice default name
	private string playerId;
	private int rank;

	
	public void SubmitScore(int score) {
		StartCoroutine(AddScore(lastName, score));
	}

	public void UpdateName(string name) {
		Debug.Assert(playerId != null, "No player Id, cannot update name!");
		lastName = name;
		StartCoroutine(UpdateName(playerId, name));

	}

	private void UpdateNeigbourScorePanel(string neighbourScore) {

	}

	private void UpdateTopScorePanel(string topScore) {

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
	
		WWW getNeighbourScoresAttempt = new WWW(neighbourScoresURL + "id=" + id);
	
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
	
		WWW getRankAttempt = new WWW(rankURL + "id=" + WWW.EscapeURL(id));
	
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
	
		WWW getTopScoresAttempt = new WWW(topScoresURL);
	
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
