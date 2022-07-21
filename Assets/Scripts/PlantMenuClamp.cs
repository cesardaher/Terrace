using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMenuClamp : MonoBehaviour
{
    public GameObject plantMenu;
    public GameObject player;
    public Camera currentCamera;
    public Vector3 playerPos;
    public int rightLimit = 1280;
    public int leftLimit = 640;

    // Update is called once per frame
    void Update()
    {
        //normal gameplay checks
        playerPos = currentCamera.WorldToScreenPoint(player.transform.position);

        if ((StateMng.instance.facingRight && playerPos.x > rightLimit) || (!StateMng.instance.facingRight && playerPos.x < rightLimit && playerPos.x > leftLimit))
        {
            //BOX TO THE LEFT
            if (transform.localPosition.x > 0)
            {
                FlipBox();

            }

        }
        else if ((StateMng.instance.facingRight && playerPos.x < rightLimit && playerPos.x > leftLimit) || (!StateMng.instance.facingRight && playerPos.x < leftLimit))
        {
            //BOX TO THE RIGHT
            if (transform.localPosition.x < 0)
            {
                FlipBox();
            }
        }

        Vector3 boxPos = currentCamera.WorldToScreenPoint(this.transform.position);
        plantMenu.transform.position = boxPos;
    }

    void FlipBox()
    {
        Vector3 tempPos = transform.localPosition;

        tempPos.x *= -1;

        transform.localPosition = tempPos;
    }
}
