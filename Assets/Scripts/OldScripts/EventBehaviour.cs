using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBehaviour : ScriptableObject
{
    public void PassTime()
    {
        TimeManager.instance.CallPassTime();
      
    }

    public void ChangeStation(int direction)
    {
        ClickManager.instance.StartCoroutine(ClickManager.instance.ChangeStation(direction));

    }

    public void SitDown()
    {
        if (!StoryManager.instance.birdsAppeared)
        {
            StoryManager.instance.StartCoroutine(StoryManager.instance.ChairAndBirds());
        }
        else
            ClickManager.instance.StartCoroutine(ClickManager.instance.Sit());

    }

    public void CollectObject()
    {
        Debug.Log("event successful");
        Inventory.instance.CollectObject();
        ClickManager.instance.ReScan();

    }

    public void ClosePanel()
    {
        //turn on all buttons
        //maybe redundant
        DialogueManager.instance.continueButton.enabled = true;
        DialogueManager.instance.characterContinueButton.enabled = true;

        StateManager.instance.inDialogue = false;
        DialogueManager.instance.dialogueCanvas.SetActive(false);
        DialogueManager.instance.characterCanvas.SetActive(false);

    }

    public void TeaTime()
    {
        StoryManager.instance.StartCoroutine(StoryManager.instance.TeaTime());
    }

    public void ClosePlantMenu()
    {
        Inventory.instance.SeedUI.SetActive(false);
        StateManager.instance.canClick = true;
        ClickManager.instance.interactedObject.GetComponent<PotTrigger>().plantText.SetActive(false);
        ClickManager.instance.StartCoroutine(ClickManager.instance.DeZoomCameraMethod());
    }

    public void ClosePlantMenuNoZoom()
    {
        Inventory.instance.SeedUI.SetActive(false);
    }

    public void PlaceObject()
    {
        Inventory.instance.PlaceObject(ClickManager.instance.interactedObject, Inventory.instance.currentId);

    }

    public void MoveObject()
    {
        Inventory.instance.MoveObject(ClickManager.instance.interactedObject);
    }

    public void DeZoomCamera()
    {
        Debug.Log("event successful");
        References.instance.clickManager.StartCoroutine("DeZoomCamera");

    }

    public void RemovePlant()
    {
        ClickManager.instance.interactedObject.GetComponent<PotTrigger>().RemoveWeed();

    }

    public void Plant()
    {
        //remove seed from inventory
        //add child to pot (maybe get reference from clickmanager)

        //turn on seed inventory
        Inventory.instance.ShowSeedDescription(true, true);
        Inventory.instance.SeedInventory();

        //Inventory.instance.Plant(Inventory.instance.nextPlant, Inventory.instance.currentId);
    }

    public void ClosePlantCanvas()
    {
        ClickManager.instance.interactedObject.GetComponent<PotTrigger>().plantText.SetActive(false);
    }

    public void SeedDescription(bool next)
    {
        //show seed description without opening or closing inventory
        Inventory.instance.ShowSeedDescription(false, next);

    }

    public void Harvest()
    {
        Inventory.instance.HarvestPlant();
    }

}
