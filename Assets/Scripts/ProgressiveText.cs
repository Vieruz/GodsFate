using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressiveText : MonoBehaviour {

    public Text displayText;
    public string textToWrite;
    public Animator nextAnimation;

    private int textIndex = 0;

	// Use this for initialization
	void Start () {
        Reset();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(textIndex < textToWrite.Length)
        {
            textIndex++;
            displayText.text = textToWrite.Substring(0, textIndex);
        }

        if (textIndex == textToWrite.Length)
        {
            textIndex++;
            if (nextAnimation)
            {
                nextAnimation.SetTrigger("show");
            }
        }

	}

    public bool CheckFinishWriting()
    {
        if(textIndex < textToWrite.Length)
        {
            return false;
        }
        return true;
    }

    public void Reset()
    {
        displayText.text = "";
        textIndex = 0;
    }
}
