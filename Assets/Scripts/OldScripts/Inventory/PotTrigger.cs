using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotTrigger : MonoBehaviour
{
	public int potId;

	public DialogueBase plantOrCollect;
	public DialogueBase collect;
	public DialogueBase harvest;
	public DialogueBase remove;
	public DialogueBase wait;
	public DialogueBase enough;
	public DialogueBase special;
	private bool hasSeeds = false;

	[Header("Canvas")]
	public GameObject mainCanvas;
	public GameObject circle;
	public GameObject removeText;
	public GameObject collectText;
	public GameObject moveText;
	public GameObject plantText;
	public GameObject harvestText;

	void Start()
	{
		//turn off interaction canvas
		mainCanvas.SetActive(false);
	}

	void OnMouseOver()
	{
		if(StateManager.instance.canClick)
			//turn on interaction canvas
			mainCanvas.SetActive(true);
	}

	void OnMouseExit()
	{
		if(StateManager.instance.canClick)
			//turn off interaction canvas
			mainCanvas.SetActive(false);
	}

	public void RemoveWeed()
	{
		Destroy(transform.GetChild(2).gameObject);

		StartCoroutine(ClickManager.instance.DeZoomCameraMethod());

		//StartCoroutine(StoryManager.instance.BackLater(false));
    }

	public void Interact()
    {
		StateManager.instance.canClick = false;
		Debug.Log("interacted with pot");
		Debug.Log(transform.childCount);
		//turn on interaction canvas
		mainCanvas.SetActive(true);

		//if empty
		if (transform.childCount == 2)
		{

			//if has seeds
			for (int i = 0; i < Inventory.instance.invSlots.Length; i++)
			{
				if (Inventory.instance.invSlots[i] != null && (Inventory.instance.invSlots[i].GetComponent<ObjectInfo>() is SeedInfo))
				{
					Inventory.instance.currentId = i;
					//Inventory.instance.nextPlant = Inventory.instance.invSlots[i].GetComponent<SeedInfo>().plant;
					hasSeeds = true;
					break;
				}
			}

			if (hasSeeds)
            {
				plantText.SetActive(true);
				circle.SetActive(false);
			}	

			else
            {
				Debug.Log("dialogue with pot");
				//basic: collect
				//moveText.SetActive(true);
				DialogueManager.instance.EnqueueDialogue(special);
				mainCanvas.SetActive(false);

			}			

		} 
		//if not empty
		else if (transform.childCount > 2)
		{
			GameObject plant = transform.GetChild(2).gameObject;

			if (plant.tag == "Plant")
				if (plant.GetComponent<Plant>().isGrown && !plant.GetComponent<Plant>().hasHarvested)
                {
					circle.SetActive(false);
					harvestText.SetActive(true);
				}					
				else if (plant.GetComponent<Plant>().hasHarvested)
                {
					DialogueManager.instance.EnqueueDialogue(enough);
					mainCanvas.SetActive(false);

				} else
                {
					DialogueManager.instance.EnqueueDialogue(wait);
					mainCanvas.SetActive(false);

				}
					

			else if (transform.GetChild(2).tag == "Dead Plant")
			{
				if(StoryManager.instance.gotShovel)
                {
					circle.SetActive(true);
					removeText.SetActive(true);
					//ned to turn back on
					collectText.SetActive(false);

				} else
                {
					//needs to add shovel interaction
					DialogueManager.instance.EnqueueDialogue(remove);
					mainCanvas.SetActive(false);

				}
				
			}

		}
	}
}
