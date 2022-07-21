using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlippedPot : InteractableObject
{
    public static string flippedNode = "Pot.Flipped";

    // buttons
    public Button pickupButton;
    public Button exitButton;

    public override IEnumerator InteractWithObject()
    {
        //move to object if needed
        Task t = new Task(base.InteractWithObject());

        while (t.Running)
            yield return null;

        //pause normal gameplay
        StateMng.instance.CanClick = false;
        
        // if it's first scene, dialogue
        if(TimeMng.instance.TimeState == 0)
        {
            StartDialogue(flippedNode);
            yield break;
        }

        // if not, enable flipping
        pickupButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
