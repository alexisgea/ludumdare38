using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextFade : MonoBehaviour {

    [SerializeField] float fadeSpeed = 0.01f;
    Text textBox;
    public bool Animate = false;
    private Color baseColor;


    void Start() {
        textBox = GetComponent<Text>();
        baseColor = textBox.color;
    }

    // Update is called once per frame
    void Update () {
		if(Animate) {
            Color newColor = textBox.color;
            newColor.a -= fadeSpeed;
            textBox.color = newColor;

            if(newColor.a == 0f) {
                Animate = false;
            }

        }
	}

	public void DisplayMessage(string message) {
        Animate = true;
        textBox.text = message;
        textBox.color = baseColor;
    }


}
