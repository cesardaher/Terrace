using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriedPlant : InteractableObject
{
    static int timeState;
    public static int TimeState
    {
        get { return timeState; }
        set
        {
            timeState = value;

            foreach (DriedPlant plant in driedPlantList)
            {
                plant.UpdateDrought();
            }
        }
    }

    public static List<DriedPlant> driedPlantList = new List<DriedPlant>();

    private int initialTime;

    [Header("Plant Info")]
    public int id;
    public int drought;
    public bool dried;

    public Hanger myHanger;

    protected override void Start()
    {
        initialTime = TimeState;
        driedPlantList.Add(this);
        myHanger = transform.parent.GetComponent<Hanger>();
    }

    void UpdateDrought()
    {
        drought = (timeState - initialTime) / 2;

        if(drought >= 1)
        {
            dried = true;

            //update valerian story
            if (id == 2) StoryMng.instance.valerianDried = true;
        }
    }

    new void OnMouseOver()
    {
        if (StateMng.instance.CanClick && canInteract && !StateMng.instance.interacting)
            //turn on interaction canvas
            myHanger.objCanvas.gameObject.SetActive(true);
    }

    new void OnMouseExit()
    {
        if (myHanger.objCanvas.gameObject.activeSelf && !StateMng.instance.interacting)
            //turn off interaction canvas
            myHanger.objCanvas.gameObject.SetActive(false);
    }

}
