using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    public Animator playerAnimator;

    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    Vector2 movement;

    public GameObject interactedObject;

    public GameObject interactCanvas;

    public bool sitting;

    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Move character
        if(movement.x != 0 || movement.y != 0)
        {
            //get up if sitting
            if(sitting)
            {
                playerAnimator.SetBool("isSitting", false);
            }

            //turn on movement animation
            playerAnimator.SetBool("isMoving", true);
            
            // Character flipping
            // if player is facing right
            if(transform.localScale.x > 0)
            {
                //if movement is to left
                if(movement.x < 0)
                {
                    transform.localScale = new Vector3(- transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
            else if (transform.localScale.x < 0) //if player is facing left
            {
                //if movement is to right
                if (movement.x > 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }

            

        } else
        {
            playerAnimator.SetBool("isMoving", false);
        }

        // press button on object
        if(interactedObject != null)
        {
            // if object is a chair
            if(interactedObject.GetComponent<ChairTrigger>() != null)
            {
                //if key is pressed
                if(Input.GetKeyDown("space"))
                {
                    if(interactCanvas.activeSelf == false)
                    {
                        //turn on interaction canvas 
                        interactCanvas.SetActive(true);

                    } else
                    {
                        //turn off interaction canvas and sit
                        interactCanvas.SetActive(false);
                        SitDown();

                    }
                    

                    //remove this?
                    //interactedObject.GetComponent<ChairTrigger>().Interact();
                }
            }
        }

    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // When colliding with objects
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ChairTrigger>() != null)
        {
            interactedObject = collision.gameObject;
            collision.gameObject.GetComponent<ChairTrigger>().ShowCanvas();
        }
    }

    // When de-colliding with objects
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("trigger exit");
        if (collision.gameObject.GetComponent<ChairTrigger>() != null)
        {
            interactedObject = null;
            collision.gameObject.GetComponent<ChairTrigger>().CloseCanvas();
        }

    }

    public void SitDown()
    {
        sitting = true;
        gameObject.transform.position = new Vector3(-8.5f, -4.5f, -6f);
        playerAnimator.SetBool("isSitting", true);

    }
}
