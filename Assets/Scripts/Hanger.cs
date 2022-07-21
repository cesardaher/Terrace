using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hanger : InteractableObject
{
    private bool full;
    public bool Full
    {
        get { return full; }
        set { 
            full = value;
            PositionCanvas();
        }
    }
    public DriedPlant driedPlant;
    public GameObject cloth;

    public Button hangButton;
    public Button removeButton;

    [Header("Sprite Positions")]
    public GameObject circle;
    public float gap = 0.5f;
    public float botPos;
    public float midPos;


    [Header("Plants list")]
    public List<GameObject> plantList;

    private static string canDryNode = "Hanger.CanDry";
    private static string cantDryNode = "Hanger.CantDry";
    private static string readyValerianNode = "Hanger.Valerian.Ready";
    private static string readyMintNode = "Hanger.Mint.Ready";
    private static string notReadyNode = "Hanger.NotReady";
    private static string mintLeaveItNode = "Hanger.Mint.LeaveIt";


    // Start is called before the first frame update
    protected override void Start()
    {
        objCanvas.gameObject.SetActive(false);
        PositionCanvas();
    }

    public override IEnumerator InteractWithObject()
    {
        //move to object if needed
        Task t = new Task(base.InteractWithObject());

        while (t.Running)
            yield return null;

        //pause normal gameplay
        StateMng.instance.CanClick = false;        

        // get hanger dialogue
        CheckFullHanger();
    }

    private void CheckFullHanger()
    {
        // if there's cloth enable removing it
        if (cloth.activeSelf)
        {
            removeButton.gameObject.SetActive(true);
            return;
        }

        // if it's not full, dialogues
        if (!full)
        {
            EmptyHangerDialogues();
            return;
        }

        // case 3: plant not dry
        if (!driedPlant.dried)
        {    
            StartDialogue(notReadyNode);
            return;
        }

        // case 4: plant is dry
        switch (driedPlant.id)
        {
            case 1:
                // case 5: harvested mint
                if (InventoryMng.instance.plantPage.Contains(6))
                {
                    StartDialogue(mintLeaveItNode);
                    break;
                }

                StartDialogue(readyMintNode);
                break;
            case 2:
                StartDialogue(readyValerianNode);
                break;
            default:
                Debug.LogError("Dried plant id non-existing.");
                break;
        }
 
    }

    private static void EmptyHangerDialogues()
    {
        //check if has plants to dry
        InventoryMng.instance.CheckPageEmpty(1);

        if (!InventoryMng.instance.plantEmpty)
        {
            // case 1: empty and has plant
            // ask to plant something

            StartDialogue(canDryNode);
            return;
        }

        // case 2: empty and has no plant
        // come back later

        StartDialogue(cantDryNode);
        
    }

    public void DryPlant(int id)
    {
        //remove plant from inventory
        InventoryMng.instance.plantPage.Remove(id);

        //add dried plant
        driedPlant = Instantiate(InventoryMng.instance.list.dryablePlants[id], transform).GetComponent<DriedPlant>();

        //update hanger
        Full = true;
    }

    public void PositionCanvas()
    {
        if(Full)
        {
            botPos = driedPlant.GetComponent<SpriteRenderer>().bounds.min.y;
            midPos = driedPlant.GetComponent<SpriteRenderer>().bounds.center.x;
            circle.transform.position = new Vector3(midPos, botPos - gap, circle.transform.position.z);

        } else if (cloth.activeSelf)
        {
            botPos = cloth.GetComponent<SpriteRenderer>().bounds.min.y;
            midPos = cloth.GetComponent<SpriteRenderer>().bounds.center.x;
            circle.transform.position = new Vector3(midPos, botPos - gap, circle.transform.position.z);
        }
        else
        {
            botPos = transform.parent.GetComponent<SpriteRenderer>().bounds.min.y;
            midPos = transform.GetComponent<Collider2D>().bounds.center.x;
            circle.transform.position = new Vector3(midPos, botPos - 0.5f, circle.transform.position.z);
        }
    }

    /*
    void OnMouseOver()
    {
        if (StateMng.instance.CanClick)
            //turn on interaction canvas
            objCanvas.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        if (StateMng.instance.CanClick)
            //turn off interaction canvas
            objCanvas.gameObject.SetActive(false);
    }*/
}
