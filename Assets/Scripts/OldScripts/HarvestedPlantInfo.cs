using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestedPlantInfo : ObjectInfo
{
    public bool isDryable;

    public GameObject parent;

    public int plantId;

    public int plantDrought = 0;
    public bool isDry;

    public ObjectDescription dryDescription;

    public int timeState;

    void Start()
    {
        timeState = TimeManager.instance.timeState;

        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //update plantdrought
        if (timeState < TimeManager.instance.timeState)
        {
            //update timestate for comparison
            timeState = TimeManager.instance.timeState;

            if (TimeManager.instance.weatherState != 1)
            {
                //update plant's growth
                plantDrought++;
            }            

            //reset hasharvested on checkpoints
            if (plantDrought == 2)
            {
                isDry = true;
            }

        }

        if (isDry && description != dryDescription)
        {
            description = dryDescription;
        }

        //if it's dried valerian
        if(isDry && plantId == 1)
        {
            if(!StoryManager.instance.hasDriedValerian)
                StoryManager.instance.hasDriedValerian = true;
        }

        if (isDry && plantId == 0)
        {
            if (!StoryManager.instance.hasDriedMint)
                StoryManager.instance.hasDriedMint = true;
        }


    }

    void OnMouseOver()
    {
        Debug.Log("mouse over");
        if (StateManager.instance.canClick)
            //turn on interaction canvas
            parent.GetComponent<HangerScript>().mainCanvas.SetActive(true);
    }

    void OnMouseExit()
    {
        if (StateManager.instance.canClick)
            //turn off interaction canvas
            parent.GetComponent<HangerScript>().mainCanvas.SetActive(false);
    }
}
