using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;

    public Rigidbody2D rb;
    public IntroManager intro;

    public Player player;

	// Use this for initialization
	void Start () {
        player = GetComponent<Player>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical);
        
        Vector2 nextPosition = transform.position + movement * speed * Time.fixedDeltaTime;
        if (nextPosition.x < -107 || nextPosition.x > 107 )
        {
            moveHorizontal = 0;
            movement = new Vector3(moveHorizontal, moveVertical);
            nextPosition = transform.position + movement * speed * Time.fixedDeltaTime;
        }
        if (nextPosition.y < -23 || nextPosition.y > 23)
        {
            moveVertical = 0;
            movement = new Vector3(moveHorizontal, moveVertical);
            nextPosition = transform.position + movement * speed * Time.fixedDeltaTime;
        }

        if ((moveHorizontal != 0 || moveVertical != 0) && player.actionButtons.activeSelf)
        {
            player.actionButtons.SetActive(false);
        }
        
        rb.MovePosition(nextPosition);

        if (intro.introIndex == 0 && (moveHorizontal != 0 || moveVertical != 0) && intro.text.CheckFinishWriting())
        {
            intro.GoToStep(1);
        }
    }
}
