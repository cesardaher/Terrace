using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : InteractableObject
{
    private string catNormalNode = "Cat.Normal";

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator InteractWithObject()
    {
        //move to object if needed
        Task t = new Task(base.InteractWithObject());

        while (t.Running)
            yield return null;

        objCanvas.gameObject.SetActive(false);

        //pause normal gameplay
        StateMng.instance.CanClick = false;
       

        StartDialogue(catNormalNode);        
    }
}
