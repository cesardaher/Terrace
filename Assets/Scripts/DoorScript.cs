using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : InteractableObject
{
    public string YarnStartNode { get { return yarnStartNode; } }
    [SerializeField] string yarnStartNode = "Door.Start";

    // Update is called once per frame
    public override IEnumerator InteractWithObject()
    {
        //move to object if needed
        Task t = new Task(base.InteractWithObject());

        while (t.Running)
            yield return null;

        Debug.Log("InteractWithObject over");

        StartDialogue(YarnStartNode);
    }

    protected override void OnMouseOver()
    {
        if (StateMng.instance.CanClick && canInteract && StoryMng.instance.hasPlanted)
            objCanvas.gameObject.SetActive(true);
    }
}
