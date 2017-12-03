using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour {

    public float speed;
    public int idleDelay = 2;

    public Rigidbody2D rb;
    public Animator animator;

    public Vector3 targetDestination;

    public bool idle = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (!idle)
        {
            if (Vector3.Distance(transform.position, targetDestination) <= .1)
            {
                RandomMovement();
            }

            rb.MovePosition(Vector3.MoveTowards(transform.position, targetDestination, speed * Time.fixedDeltaTime));
        }

    }

    public void Stop()
    {
        idle = true;
        targetDestination = transform.position;
        animator.SetFloat("moveHorizontal", 0);
        animator.SetFloat("moveVertical", 0);
        StartCoroutine("DontMoveForAWhile");

    }

    IEnumerator DontMoveForAWhile()
    {
        yield return new WaitForSeconds(idleDelay);
        idle = false;
    }

    void RandomMovement()
    {
        float moveHorizontal = Random.Range(-1f, 1f);
        float moveVertical = Random.Range(-1f, 1f);
        
        animator.SetFloat("moveHorizontal", moveHorizontal);
        animator.SetFloat("moveVertical", moveVertical);

        if (Mathf.Abs(moveHorizontal) > Mathf.Abs(moveVertical))
        {
            animator.SetBool("horizontalDirection", true);
        }
        else
        {
            animator.SetBool("horizontalDirection", false);
        }

        targetDestination = transform.position + new Vector3(moveHorizontal, moveVertical);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("BlockingLayer"))
        {
            Vector3 collisionPoint = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0);
            Vector3 dir = collisionPoint - transform.position;
            targetDestination = transform.position -dir.normalized;
        }
    }
}
