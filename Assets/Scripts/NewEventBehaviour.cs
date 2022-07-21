using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[CreateAssetMenu(fileName = "New Event", menuName = "Event")]
public class NewEventBehaviour : ScriptableObject
{
    public void ShowPlantMenu()
    {
        //turn on seed inventory
        //open = true; next = true;

        InventoryMng.instance.plantMenu.SetActive(true);
        InventoryMng.instance.ShowSeedDescription(true, true);
    }

    public void SeedDescription(bool next)
    {
        //show seed description without opening or closing inventory
        InventoryMng.instance.ShowSeedDescription(false, next);

    }

    public void ClosePlantMenu()
    {
        StateMng.instance.CanClick = true;
    }

    public void PlantPlant()
    {
        Pot pot = (Pot)InteractableObject.currentObj;
        pot.Plant();
    }

    //false: stop movement
    //true: enable movement
    public void ToggleMovement(bool move)
    {
        StateMng.instance.CanClick = move;
    }

    
    public void ShowPotDialogue()
    {
        InteractableObject.StartDialogue(Pot.YarnStartNode);
    }

    public void ResetIntCanvas()
    {
        if(InteractableObject.currentObj != null)
        {
            InteractableObject tempObj = InteractableObject.currentObj;

            //reset canvas
            InteractableObject.currentObj.objCanvas.gameObject.SetActive(false);
            if (tempObj is Pot)
                ((Pot)tempObj).circle.SetActive(true);

        }
        
    }

    public void ReOpenDialogue(GameObject dialogueBox)
    {
        if(!dialogueBox.activeSelf) //if dialogue box is closed, open
        {
            dialogueBox.SetActive(true);
        }
    }

}
