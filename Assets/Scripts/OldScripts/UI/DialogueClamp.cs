using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueClamp : MonoBehaviour
{
    public static CharacterDialogueHolder mainPlayer;

    private CharacterDialogueHolder currentChar;
    public CharacterDialogueHolder CurrentChar
    {
        get { return currentChar; }
        set
        {
            currentChar = value;

            Debug.Log("name of currentchar: " + value.name);

            transform.parent = value.transform;
            transform.position = value.boxNormalPos;

            Debug.Log("changed character");
        }
    }

    public DialogueUI dialogueUI;
    public GameObject dialogueBox;
    public Camera currentCamera;
    public Vector3 playerPos;
    public int rightLimit = 1280;
    public int leftLimit = 640;

    [Header("Characters")]
    public CharacterDialogueHolder player;
    public CharacterDialogueHolder birds;
    public CharacterDialogueHolder cat;
    public CharacterDialogueHolder friend;

    private void Awake()
    {
        CurrentChar = player;
        mainPlayer = player;
    }

    void Update()
    {

        if (ClickMng.instance.zoomCamera.Enabled)
        {
            transform.localPosition = CurrentChar.boxZoomPos;
        }
        else
        {
            transform.localPosition = CurrentChar.boxNormalPos;
        }

        //normal gameplay checks
        playerPos = currentCamera.WorldToScreenPoint(player.transform.position);

        if (ClickMng.instance.zoomCamera.Enabled)
        {
            if (StateMng.instance.facingRight)
            {

                //BOX TO THE LEFT
                if (transform.localPosition.x > 0)
                {
                    Vector3 tempPos = transform.localPosition;

                    tempPos.x *= -1;

                    transform.localPosition = tempPos;

                }

            }
            else
            {
                //BOX TO THE RIGHT
                if (transform.localPosition.x < 0)
                {
                    Vector3 tempPos = transform.localPosition;

                    tempPos.x *= -1;

                    transform.localPosition = tempPos;
                }
            }

        } else
        {
            if ((StateMng.instance.facingRight && playerPos.x > rightLimit) || (!StateMng.instance.facingRight && playerPos.x < rightLimit && playerPos.x > leftLimit))
            {
                //BOX TO THE LEFT
                if (transform.localPosition.x > 0)
                {
                    Vector3 tempPos = transform.localPosition;

                    tempPos.x *= -1;

                    transform.localPosition = tempPos;

                }

            }
            else if ((StateMng.instance.facingRight && playerPos.x < rightLimit && playerPos.x > leftLimit) || (!StateMng.instance.facingRight && playerPos.x < leftLimit))
            {
                //BOX TO THE RIGHT
                if (transform.localPosition.x < 0)
                {
                    Vector3 tempPos = transform.localPosition;

                    tempPos.x *= -1;

                    transform.localPosition = tempPos;
                }
            }


        }

        //get position from this object on world
        //convert it to screen
        //set it to the dialogue box on canvas
        Vector3 boxPos = currentCamera.WorldToScreenPoint(this.transform.position);
        dialogueBox.transform.position = boxPos;

    }

    public void CheckSpeaker()
    {
        string text = dialogueUI.GetCurrentLine();

        if (text.Contains("You:"))
        {
            CurrentChar = player;
            return;
        }

        if(text.Contains("Birds:"))
        {
            CurrentChar = birds;
            return;
        }

        if (text.Contains("Cat:"))
        {
            CurrentChar = cat;
            return;
        }

        if (text.Contains("Girlfriend:"))
        {
            CurrentChar = friend;
            return;
        }
    }

    public void ReturnToPlayer()
    {
        CurrentChar = player;
    }

} //class
