using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UnityEventHandler : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent eventHandler;
    public DialogueBase myDialogue;

    //this is what happens when you click on button
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        eventHandler.Invoke();
        DialogueManager.instance.CloseOptions();
        
        //if there is a next dialogue, start it
        if(myDialogue != null)
        {
            StateManager.instance.inDialogue = false;
            Debug.Log("dialogue is not null");
            DialogueManager.instance.EnqueueDialogue(myDialogue);
        }
    }
}
