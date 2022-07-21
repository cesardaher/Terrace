using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Pot : InteractableObject
{
    public GameObject circle;
    public GameObject plantGroup;
    public GameObject harvest;
    public int myPlant { get; private set; }
    public PlantObject plantObject { get; private set; }
    public bool empty;

    [Header("Dialogue Stuff")]
    [SerializeField] static YarnProgram yarnDialogue;
    public static string YarnStartNode { get { return yarnStartNode; } }
    [SerializeField] static string yarnStartNode = "Pot.Start";
    private static string deadPlantNode = "Pot.DeadPlant";
    private static string noSeedsNode = "Pot.NoSeeds";
    private static string seedlingNode = "Pot.Seedling";
    private static string cantharvestNode = "Pot.CantHarvest";

    //sprite positions for plants
    float topPos;
    float botPos;
    float gap = 0.5f;

    protected override void Start()
    {
        base.Start();
        foreach(Transform child in transform) // check if pot has a plant 
        {
            if(child.GetComponent<PlantObject>() != null)
            {
                empty = false;
                plantObject = child.GetComponent<PlantObject>();
                myPlant = plantObject.id;
                StartCoroutine("AdjustCanvasPos");
                return;
            }
        }
    }

    public override IEnumerator InteractWithObject()
    {
        //move to object if needed
        Task t = new Task(base.InteractWithObject());

        while (t.Running)
            yield return null;

        //pause normal gameplay
        StateMng.instance.CanClick = false;

        //turn off initial circle
        circle.SetActive(false);

        if(!empty) // pot has plant
        {
            if(myPlant == 0) // case 1: dead plant
            {
                Debug.Log("dead plant found");
                StartDialogue(deadPlantNode);
            }
            else if(!plantObject.isGrown) // case 2: seedling
            {
                StartDialogue(seedlingNode);
            }
            else if (plantObject.isGrown) // case 3: grown plant
            {
                if(!plantObject.hasHarvested) // 3.1 if wasn't harvested yet
                {
                    //harvest
                    Debug.Log("plant can be harvested");
                    circle.SetActive(true);
                    harvest.SetActive(true);
                }
                else // 3.2 was harvested
                {
                    circle.SetActive(false);
                    StartDialogue(cantharvestNode);
                }
            }
        }
        else // pot is empty
        {
            InventoryMng.instance.CheckPageEmpty(0); // check if there are seeds

            if(InventoryMng.instance.seedEmpty) // case 4: player has no seeds to plant
            {
                Debug.Log("seeds empty");
                StartDialogue(noSeedsNode);
            }
            else // case 5: player has seeds (PLANT)
            {
                plantGroup.SetActive(true);
            }
        }

        //objCanvas.gameObject.SetActive(false);
        //circle.SetActive(true);
    }

    public void Plant()
    {
        //store plant ID
        int thisPlantId = SeedDescription.currentSeed;

        //delete seed from inventory
        foreach (int id in InventoryMng.instance.seedsPage)
        {
            if (thisPlantId == id)
            {
                InventoryMng.instance.seedsPage.Remove(id);
                InventoryMng.instance.CheckPageEmpty(0); //check if seeds page is empty;
                break;
            }
        }

        //get new plant
        GameObject newPlant = InventoryMng.instance.list.plantObjects[thisPlantId];

        //instantiate it on pot
        plantObject = Instantiate(newPlant, transform).GetComponent<PlantObject>();

        //change pot's states
        empty = false;
        myPlant = thisPlantId;

        //change Story state
        if (!StoryMng.instance.hasPlanted) StoryMng.instance.hasPlanted = true;

        // end interaction
        StateMng.instance.CanClick = true;
        StateMng.instance.interacting = false;
    }

    public void Harvest() // CALL PLANT
    {
        plantObject.Harvest();
    }

    public void EmptyPot()
    {
        empty = true;
    }

    //TODO: MAYBE REMOVE THIS SINCE INTERACTION IS MAINLY THROUGH MOUSE
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (StateMng.instance.CanClick)
        {
            objCanvas.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Return) && objCanvas.gameObject.activeSelf)
            {
                InteractWithObject();
            }
        }

    }

    public IEnumerator AdjustCanvasPos()
    {
        if(!empty) // if there is a plant
        {
            yield return new WaitForSeconds(2f);               
            
             //center to plant bound
            //Debug.Log("topPos:" + topPos);

            if(myPlant == 0)
            {
                topPos = plantObject.GetComponent<SpriteRenderer>().bounds.max.y; //max y post for sprite
                circle.transform.position = new Vector3(circle.transform.position.x, topPos + gap, circle.transform.position.y);
                yield break;
            }

            if(plantObject.plantAnimator.GetBool("isGrown"))
            {
                if(myPlant == 1)
                {
                    topPos = plantObject.GetComponent<SpriteRenderer>().bounds.max.y; //max y post for sprite
                    circle.transform.position = new Vector3(circle.transform.position.x, topPos - gap, circle.transform.position.y);
                    yield break;
                }

                if (myPlant == 2)
                {
                    topPos = plantObject.GetComponent<SpriteRenderer>().bounds.max.y; //max y post for sprite
                    circle.transform.position = new Vector3(circle.transform.position.x, topPos + gap, circle.transform.position.y);
                    yield break;
                }
            }
            
            /*
            else 
            {
                topPos = GetComponent<SpriteRenderer>().bounds.center.y + plantObject.GetComponent<SpriteRenderer>().bounds.extents.y;
                circle.transform.position = new Vector3(circle.transform.position.x, topPos + gap, circle.transform.position.y);
            }*/
            

        } else // if just pot, use pot's dimensions
        {
            topPos = GetComponent<SpriteRenderer>().bounds.max.y; //center to bound 
            circle.transform.position = new Vector3(circle.transform.position.x, topPos + gap, circle.transform.position.y);
            plantGroup.transform.position = new Vector3(circle.transform.position.x, topPos, circle.transform.position.y);

            yield return null;
        }
    }
}
