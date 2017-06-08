using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLine : MonoBehaviour {

	[SerializeField] Text rankField;
	[SerializeField] Text nameField;
	[SerializeField] Text scoreField;

	public void UpdateLine(string rank, string name, string score) {
		rankField.text = rank;
		nameField.text = name;
		scoreField.text = score;
	}

	public void HighlightLine() {
		// this line is the player line so highlight it
	}
}
