using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    //declare player and targer
    public GameObject target;
    public GameObject player;
    public Vector3 mousePos;

    void Start()
    {
        //set target position as player position
        target = GameObject.Find("Target");
        player = GameObject.Find("Player");
        target.transform.position = player.transform.position;

    }

    void Update()
    {
    
    }
    

    private void OnMouseDown() 
    {

        Debug.Log("mouseclicked");

        mousePos = Input.mousePosition;


        //set target to clicked position within collider
        //does not work if there is other collider on top
        target.transform.position = Camera.main.ScreenToWorldPoint(mousePos);        
        
    }
    
}
