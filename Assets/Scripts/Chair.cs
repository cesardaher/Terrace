using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chair : InteractableObject
{
    //positions for brown chair
    Vector3 initialPos = new Vector3(0.3f, -1.38f, 4f);
    Vector3 initialRot = new Vector3(0, 0, 270);

    Vector3 standPos = new Vector3(0, 0, 4f);
    Vector3 standRot = new Vector3(0, 0, 0);

    Vector2 sittingPos = new Vector2(-0.45f, -0.3f); // for white chair, facing left

    public int id; // 0 brown 1 white

    public GameObject circle;
    public Button sitButton;
    public Button pickupButton;
    public Button exitButton;

    bool standing;
    public bool Standing
    {
        get { return standing; }
        set
        {
            standing = value;

            if(id == 0) //only applies for brown chair
            {
                ChangeState(value);
           
            }
            
        }
    }

    private void ChangeState(bool up)
    {
        if(up)
        {
            Debug.Log("standing set");

            //set up standing positions
            transform.localPosition = standPos;
            transform.localRotation = Quaternion.Euler(standRot);

            //change canvas position
            Vector3 tempPos = objCanvas.transform.localPosition;
            tempPos.x = -0.5f;
            tempPos.y = 2.5f;
            objCanvas.transform.localPosition = tempPos;

            //change X position
            var exit_rectTransform = exitButton.GetComponent<RectTransform>();
            Vector3 tempXPos = exit_rectTransform.anchoredPosition;
            tempXPos.x = 159;
            tempXPos.y = -754;
            exit_rectTransform.anchoredPosition = tempXPos;

            return;
        }

        //set up standing positions
        transform.localPosition = initialPos;
        transform.localRotation = Quaternion.Euler(initialRot);

    }

    protected override void Start()
    {
        base.Start();
        if (id == 1) Standing = true;
    }

    public override IEnumerator InteractWithObject()
    {
        //move to object if needed
        Task t = new Task(base.InteractWithObject());

        while (t.Running)
            yield return null;

        //pause normal gameplay
        StateMng.instance.CanClick = false;

        //turn on sit down and exit buttons
        if (Standing)
        {
            sitButton.gameObject.SetActive(true);
        }
        else
        {
            //on the first scene, just show dialogue
            if(TimeMng.instance.TimeState == 0)
            {
                StartDialogue(FlippedPot.flippedNode);
                yield break; ;
            }

            //after first scene, let pick up
            pickupButton.gameObject.SetActive(true);
        }

        exitButton.gameObject.SetActive(true);

    }

    public void NightThoughts()
    {
    }

    protected override void OnMouseOver()
    {
        if (StateMng.instance.CanClick && canInteract && !StateMng.instance.interacting && !StateMng.instance.IsSitting)
            objCanvas.gameObject.SetActive(true);
    }
}
