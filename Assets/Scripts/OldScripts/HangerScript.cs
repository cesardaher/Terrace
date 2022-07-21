using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangerScript : MonoBehaviour
{
    public bool full;
    public GameObject driedPlant;

    //public GameObject nextPlant;

    [Header("Canvas")]
    public GameObject mainCanvas;
    public GameObject halfCircle;
    public GameObject button;
    public GameObject collectButton;

    [Header("Sprite Positions")]
    public float botPos;

    public DialogueBase[] hangerDialogues;

    // Start is called before the first frame update
    void Start()
    {
        mainCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //check if left is full
        if(transform.childCount == 2 && full == false)
        {
            full = true;

        } else if (transform.childCount == 1 && full == true)
        {
            full = false;
        }

        if(full && driedPlant == null)
        {
            driedPlant = transform.GetChild(1).gameObject;
            //var copy = driedPlant.GetCopyOf(driedPlant.GetComponent<Collider2D>);
        }
    }

    public void Interact()
    {
        StateManager.instance.canClick = false;

        //if hanger is not full
        if(!full)
        {
            //if there is a dryable plant
            if (Inventory.instance.CheckHanger() == true && full == false)
            {
                //open up full canvas
                mainCanvas.SetActive(true);

                //disable collect button just in case
                collectButton.SetActive(false);

                //enable button
                button.SetActive(true);

            }
            else
            {
                //give error message
                TriggerDialogue(hangerDialogues[0]);

                //disable canvas
                mainCanvas.SetActive(false);
            }

        }
        //if there is a plant there
        else
        { 
            //if it's dried
            if(driedPlant.GetComponent<HarvestedPlantInfo>().isDry == true)
            {
                //if it's mint
                if(driedPlant.GetComponent<HarvestedPlantInfo>().plantId == 0)
                {
                    //if didn't meet friend yet
                    if (!StoryManager.instance.metFriend)
                    {
                        //if it's during the night or evening
                        if(TimeManager.instance.weatherState == 2 || TimeManager.instance.weatherState ==3)
                        {
                            //prompt invite friend
                            TriggerDialogue(hangerDialogues[2]);

                            //disable canvas
                            mainCanvas.SetActive(false);

                        } else //if it's other time of the day
                        {
                            //tell to come later
                            TriggerDialogue(hangerDialogues[3]);

                            //disable canvas
                            mainCanvas.SetActive(false);

                        }

                    } //if already met friend
                    else
                    {
                        //placeholder dialogue
                        TriggerDialogue(hangerDialogues[3]);

                        //disable canvas
                        mainCanvas.SetActive(false);

                    }
                
                //if it's valerian
                } else if (driedPlant.GetComponent<HarvestedPlantInfo>().plantId == 1)
                {
                    // give error message
                    TriggerDialogue(hangerDialogues[4]);

                    //disable canvas
                    mainCanvas.SetActive(false);
                }
                

            } //if it's not dried yet
            else
            {
                //give error message
                TriggerDialogue(hangerDialogues[1]);

                //disable canvas
                mainCanvas.SetActive(false);
            }

        }
        
    }

    public void TriggerDialogue(DialogueBase db)
    {
        DialogueManager.instance.EnqueueDialogue(db);
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

}
