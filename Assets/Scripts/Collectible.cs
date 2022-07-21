using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : InteractableObject
{
    public int id; // object
    public int idType; // type of object
    public GameObject collectButton;

    public override IEnumerator InteractWithObject()
    {
        //move to object if needed
        Task t = new Task(base.InteractWithObject());

        while (t.Running)
            yield return null;

        //pause normal gameplay
        StateMng.instance.CanClick = false;

        collectButton.SetActive(true);
    }

    public void CollectObject()
    {
        if(idType == 0)
        {
            InventoryMng.instance.seedsPage.Add(id);
            InventoryMng.instance.seedEmpty = false;
        } else if(idType == 1)
        {
            InventoryMng.instance.plantPage.Add(id);
            InventoryMng.instance.plantEmpty = false;
        } else if(idType == 2)
        {
            InventoryMng.instance.toolsPage.Add(id);
            InventoryMng.instance.toolsEmpty = false;
        }

        SoundList.instance.CollectSound(gameObject);

        StateMng.instance.CanClick = true;

        Destroy(gameObject);
        
    }

    protected override void OnMouseOver()
    {
        if (StateMng.instance.CanClick && StoryMng.instance.sawSparrows && canInteract) // needs to have talked to sparrows first
            objCanvas.gameObject.SetActive(true);
    }

}
