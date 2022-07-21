using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerController : MonoBehaviour
{
/*
    public Transform target;
    
    public float speed = 0.5f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    public Rigidbody2D playerRb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        playerRb = GetComponent<Rigidbody2D>();

        //invoke once after a delay, then repeating with a set delay
        InvokeRepeating("UpdatePath", 0f, .5f);

        
    }
    
    void UpdatePath()
    {
        if(seeker.IsDone()){
            //only start a path when the previous is finished
            seeker.StartPath(playerRb.position, target.position, OnPathComplete);
        }
        
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null) return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;

        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - playerRb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        playerRb.AddForce(force);

        float distance = Vector2.Distance(playerRb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
    }
    */
}
