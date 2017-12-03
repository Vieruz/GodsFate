using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour {

    public int introIndex = 0;
    public string[] startingTexts;

    public ProgressiveText text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (introIndex == startingTexts.Length)
        {
            Destroy(gameObject);
            return;
        }

        if (text.textToWrite != startingTexts[introIndex])
        {
            text.Reset();
            text.textToWrite = startingTexts[introIndex];
        }

        
    }

    public void ValidateIntro(int step)
    {
        if (introIndex == step)
        {
            introIndex++;
        }
    }

    public void GoToStep(int index)
    {
        introIndex = index;
    }


}
