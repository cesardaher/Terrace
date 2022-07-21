using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diary : InteractableObject
{
    public string YarnStartNode { get { return yarnStartNode; } }
    [SerializeField] string yarnStartNode = "Door.Start";

    public GameObject pickUpButton;
    public GameObject overlay;
    public GameObject invTutorial;

    // Update is called once per frame
    public override IEnumerator InteractWithObject()
    {
        //move to object if needed
        Task t = new Task(base.InteractWithObject());

        while (t.Running)
            yield return null;

        Debug.Log("InteractWithObject over");

        StateMng.instance.CanClick = false;

        //show pickup button
        pickUpButton.SetActive(true);
    }

    protected override void OnMouseExit()
    {
        if (objCanvas.gameObject.activeSelf && !StateMng.instance.interacting && canInteract)
            objCanvas.gameObject.SetActive(false);
    }
}
