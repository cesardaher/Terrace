using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public DialogueBase dialogue;

	public void TriggerDialogue(){

        //Use to call dialogue!!
		DialogueManager.instance.EnqueueDialogue(dialogue);
	}

	private void Update(){
		
		//for debugging
		/*
		
		if(Input.GetKeyDown(KeyCode.A))
		{
			TriggerDialogue();
		}
		*/
		
	}
}
