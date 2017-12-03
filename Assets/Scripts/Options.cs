using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {

    public Animator restartAnim;
    public Animator exitAnim;

    bool hide = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwapOptions()
    {
        hide = !hide;
        restartAnim.SetBool("cancel", hide);
        exitAnim.SetBool("cancel", hide);
    }
}
