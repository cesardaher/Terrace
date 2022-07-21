using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towel : InteractableObject
{
    public InteractableObject myHanger;

    new void OnMouseOver()
    {
        if (StateMng.instance.CanClick && canInteract && !StateMng.instance.interacting)
            //turn on interaction canvas
            myHanger.objCanvas.gameObject.SetActive(true);
    }

    new void OnMouseExit()
    {
        if (myHanger.objCanvas.gameObject.activeSelf && !StateMng.instance.interacting)
            //turn off interaction canvas
            myHanger.objCanvas.gameObject.SetActive(false);
    }
}
