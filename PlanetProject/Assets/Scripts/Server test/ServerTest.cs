using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTest : MonoBehaviour {

	private string secretKey = "SuperSecretKey";
	//private string addScoreURL = "http://localhost/AddScoreTest.php?";
	private string addScoreURL = "http://asteroidlab.com/games/tinydefender/AddScore.php";
	private string updateNameURL = "http://asteroidlab.com/games/tinydefender/UpdateName.php";
	private string topScoresURL = "http://asteroidlab.com/games/tinydefender/TopScores.php";
	private string neighbourScoresURL = "http://asteroidlab.com/games/tinydefender/NeighbourScores.php?";
	private string rankURL = "http://asteroidlab.com/games/tinydefender/GetRank.php?";
	private int highscore;
	private string username;
	private int rank;

	// Use this for initialization
	void Start () {
        StartCoroutine(AddScore("josé", 500));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator AddScore(string name, int score) {
	
		string hash = Md5Sum(name + score + secretKey);

        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("score", score);
        form.AddField("hash", hash);
        WWW postScoreAttempt = new WWW(addScoreURL, form);

        //WWW postURL = new WWW(addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash);
        yield return postScoreAttempt;
	
		if (postScoreAttempt.error == null)
		{
			string id = postScoreAttempt.text;
            Debug.Log("Post score success with id: " + id);
            StartCoroutine(GetNeighbourScores(id));
            //StartCoroutine(GetRank(id));
        }
		else
		{
            Debug.Log("Post score failure " + postScoreAttempt.error);			
			//Error();
		}
	}

	IEnumerator GetNeighbourScores(string id) {
	
		WWW getNeighbourScoresAttempt = new WWW(neighbourScoresURL + "id=" + id);
	
		yield return getNeighbourScoresAttempt;
	
		if (getNeighbourScoresAttempt.error == null)
		{
            Debug.Log("Get neighbour scores success:\n" + getNeighbourScoresAttempt.text);
            
        }
		else
		{
			Debug.Log("Get neighbour scores failure " + getNeighbourScoresAttempt.error);	
			//Error();
		}
	}

	IEnumerator GetRank(string id) {
	
		WWW getRankAttempt = new WWW(rankURL + "id=" + WWW.EscapeURL(id));
	
		yield return getRankAttempt;
	
		if (getRankAttempt.error == null)
		{
			rank = System.Int32.Parse(getRankAttempt.text);
            Debug.Log("Get rank success: " + rank);
            StartCoroutine(GetTopScores());
        }
		else
		{
			Debug.Log("Get rank failure " + getRankAttempt.error);	
			//Error();
		}
	}

	IEnumerator GetTopScores() {
	
		WWW getTopScoresAttempt = new WWW(topScoresURL);
	
		yield return getTopScoresAttempt;
	
		if (getTopScoresAttempt.error == null)
		{
			string scores = getTopScoresAttempt.text;
            Debug.Log("Top scores:\n" + scores);
        }
		else
		{
			Debug.Log("Get top failure " + getTopScoresAttempt.error);	
			//Error();
		}
	}

	IEnumerator UpdateName(string id, string name) {
	
		string hash = Md5Sum(id + name + secretKey);

        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("name", name);
        form.AddField("hash", hash);
        WWW postNameAttempt = new WWW(updateNameURL, form);

        yield return postNameAttempt;
	
		if (postNameAttempt.error == null)
		{
            Debug.Log("Update name successful");
        }
		else
		{
            Debug.Log("Updtate name failure " + postNameAttempt.error);			
			//Error();
		}
	}


	public  string Md5Sum(string strToEncrypt) {
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
	
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
	
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
	
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
	
		return hashString.PadLeft(32, '0');
	}
}
