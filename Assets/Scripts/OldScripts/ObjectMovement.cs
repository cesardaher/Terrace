using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public Vector3 myPos;
    public Vector3 endPos;
    public Vector3[] posList;

    public float speed = 10f;

    public bool moveOut;
    public bool moveIn;

    // Start is called before the first frame update
    void Start()
    {
        myPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveOut)
        {
            transform.position = Vector3.MoveTowards(transform.position, myPos, Time.deltaTime * speed);

            float dist = Vector3.Distance(transform.position, myPos);

            if(dist < 1)
                moveOut = false;
            
        }    
        
        if(moveIn)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * speed);

            float dist = Vector3.Distance(transform.position, endPos);
            Debug.Log(dist);

            if(dist < 1)
                moveIn = false;

        }
    }
}
