using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class PlayerControllerRegular : MonoBehaviour
{
    private float speed = 4;

    private Vector3 targetPosition;
    public bool isTargetted = false;
    private bool isMoving = false;

    private Collider backgroundCollider;

    void Start(){

        backgroundCollider = GameObject.Find("0_House").GetComponent<Collider>();

    }

    void Update()
    {

        if(isTargetted && !isMoving)
        {
            //move when clicked on target position AND player is not moving
            SetTargetPosition();
        }

        if(isMoving)
        {
            Move();
        }
        
    }

    void SetTargetPosition()
    {
        //set cursor position as the target
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = transform.position.z;

        isMoving = true;

    }

    public void Move()
    {
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPosition);

        //move towards position
        transform.position =  Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        //stop moving when in targetposition
        
        if(transform.position == targetPosition)
        {
            isMoving = false;
            isTargetted = false;
        }
        
    }

    public void Target()
    {
        Debug.Log("it works");
        isTargetted = true;
    }
}
