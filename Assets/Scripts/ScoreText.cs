using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    
    public Text scoreDisplay;

	// Use this for initialization
	void Start () {
        scoreDisplay = GetComponent<Text>();
        DisplayBestScore();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DisplayBestScore()
    {
        scoreDisplay.text = "Best score: " + PlayerPrefs.GetInt("highscore").ToString() + " followers.";
    }
}
