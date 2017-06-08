using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JsonTest : MonoBehaviour {


	// Use this for initialization
	// void Start () {
	// 	ScoreLineSerializable scoreLine1 = new ScoreLineSerializable("13", "2", "alexis", "50");
	// 	ScoreLineSerializable scoreLine2 = new ScoreLineSerializable("11", "5", "tim", "40");
	// 	ScoreLineArraySerializable jsonRecept = new ScoreLineArraySerializable(new ScoreLineSerializable[2]{scoreLine1, scoreLine2});
		
	// 	string jsoned = JsonUtility.ToJson(jsonRecept);
	// 	Debug.Log(jsoned);
	// }
	

}

// [Serializable]
// public class ScoreLineArraySerializable {

// 	public ScoreLineSerializable[] ScoreLines;

// 	public ScoreLineArraySerializable(ScoreLineSerializable[] scoreLines) {
// 		ScoreLines = scoreLines;
// 	}


// }

// [Serializable]
// public class ScoreLineSerializable {
// 	public string Id;
// 	public string Rank;
// 	public string Name;
// 	public string Score;

// 	public ScoreLineSerializable(string id, string rank, string name, string score) {
// 		Id = id;
// 		Rank = rank;
// 		Name = name;
// 		Score = score;	
// 	}

// }
