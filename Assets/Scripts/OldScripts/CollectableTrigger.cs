using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableTrigger : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject mainCanvas;
    public GameObject collectText;
    public GameObject plantText;
    public GameObject harvestText;

    [Header("Dialogues")]
    public DialogueBase collectDialogue;

    void Start()
    {
        //turn off interaction canvas
        mainCanvas.SetActive(false);
    }

    void OnMouseOver()
    {
        if (StateManager.instance.canClick)
            //turn on interaction canvas
            mainCanvas.SetActive(true);
    }

    void OnMouseExit()
    {
        if (StateManager.instance.canClick)
            //turn off interaction canvas
            mainCanvas.SetActive(false);
    }

	public void Interact()
	{
        StateManager.instance.canClick = false;
		//turn on interaction canvas
		mainCanvas.SetActive(true);

		//turn on collect button
		collectText.SetActive(true);

	}


	// Update is called once per frame
	void Update()
    {
        
    }
}
