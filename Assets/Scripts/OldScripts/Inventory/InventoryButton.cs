using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public GameObject placeButton;

    public void SelectObject()
    {
        placeButton.SetActive(false);

        //assign current object as object to be placed
        int thisId = gameObject.GetComponent<InventorySlot>().slotId;
        Inventory.instance.currentId = thisId;

        //set current object's description
        Inventory.instance.ShowDescription();

        //if object is not a seed
        //get component by checking the id
        /*
        if (!(Inventory.instance.invSlots[thisId].GetComponent<ObjectInfo>() is SeedInfo))
        {
            placeButton.SetActive(true);
        }
        */
    }

    public void EnableSelection()
    {
        //enable selection mode
        //Debug.Log("was clicked");
        StateManager.instance.placeSelect = true;
        StateManager.instance.canClick = false;

    }
}
