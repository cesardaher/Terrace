using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampBox : MonoBehaviour
{
    public GameObject dialogueBox;
    public Camera currentCamera;
    public GameObject player;
    public Vector3 playerPos;
    public Vector3 boxFirstPos;
    public Vector3 boxZoomPos = new Vector3(3.5f, 11, 10);
    public int rightLimit = 1280;
    public int leftLimit = 640;

    void Start()
    {

        if (this.transform.localPosition.x < 0)
        {
            boxFirstPos = this.transform.localPosition;
            boxFirstPos.x *= -1;

        }
        else
        {
            boxFirstPos = this.transform.localPosition;
        }

         
    }

    void Update()
    {
        
        //normal gameplay checks
        if(!StoryManager.instance.cutscene)
        {
            if (ClickManager.instance.zoomCamera.enabled)
            {
                transform.localPosition = boxZoomPos;

            } else
            {
                transform.localPosition = boxFirstPos;
            }


            playerPos = currentCamera.WorldToScreenPoint(player.transform.position);

            if ((StateManager.instance.facingRight && playerPos.x > rightLimit) || (!StateManager.instance.facingRight && playerPos.x < rightLimit && playerPos.x > leftLimit))
            {
                //BOX TO THE LEFT
                if (transform.localPosition.x > 0)
                {
                    Vector3 tempPos = transform.localPosition;
                    
                    tempPos.x *= -1;

                    transform.localPosition = tempPos;

                }

            }
            else if ((StateManager.instance.facingRight && playerPos.x < rightLimit && playerPos.x > leftLimit) || (!StateManager.instance.facingRight && playerPos.x < leftLimit))
            {
                //BOX TO THE RIGHT
                if (transform.localPosition.x < 0)
                {
                    Vector3 tempPos = transform.localPosition;

                    tempPos.x *= -1;

                    transform.localPosition = tempPos;
                }
            }

            //get position from this object on world
            //convert it to screen
            //set it to the dialogue box on canvas
            Vector3 boxPos = currentCamera.WorldToScreenPoint(this.transform.position);
            dialogueBox.transform.position = boxPos;

        } //set specific positions when in cutscene 
        else
        {
            Vector3 boxPos = currentCamera.WorldToScreenPoint(this.transform.position);
            dialogueBox.transform.position = boxPos;
        }

    }
}
