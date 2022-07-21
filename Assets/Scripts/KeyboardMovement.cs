using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class KeyboardMovement : MonoBehaviour
{
    Vector2 movement;

    public Rigidbody2D rb;

    public float moveSpeed;

    public Animator playerAnimator;

    public GameObject destination;

    public AIPath pathfinder;

    public GameObject interactedObject;

    public GameObject moveTutorial;

    // Update is called once per frame
    void Update()
    {
        //arrow keys input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(StateMng.instance.CanClick && !StateMng.instance.IsSitting)
        {
            // Move character
            if (movement.x != 0 || movement.y != 0)
            {
                //disable movement toturial if open
                if (moveTutorial.activeSelf) moveTutorial.SetActive(false);

                //disable pathfinder while moving with keyboard
                ClickMng.instance.pathfinder.enabled = false;

                //update state
                StateMng.instance.keyboardMoving = true;

                //turn on movement animation
                //playerAnimator.SetBool("isMoving", true);

                // Character flipping
                // if player is facing right
                if (StateMng.instance.facingRight)
                {
                    //if movement is to left
                    if (movement.x < 0)
                    {
                        StateMng.instance.facingRight = !StateMng.instance.facingRight;
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                }
                else if (!StateMng.instance.facingRight) //if player is facing left
                {
                    //if movement is to right
                    if (movement.x > 0)
                    {
                        StateMng.instance.facingRight = !StateMng.instance.facingRight;
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                }

                //make destination accompany you, to make sure clicking movement still works
                destination.transform.position = transform.position;
            }
            else
            {
                if (StateMng.instance.keyboardMoving)
                    StateMng.instance.keyboardMoving = false;


                //makes sure that player can use moving animation when moving through clicking
                //aka destination is not equal to player
                if (destination.transform.position == transform.position && !pathfinder.enabled)
                {
                    //playerAnimator.SetBool("isMoving", false);
                    pathfinder.enabled = true;
                }
            }

        }

        
    }

    void FixedUpdate()
    {
        if(StateMng.instance.keyboardMoving)
        {
            // Movement
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        
    }
     
}
