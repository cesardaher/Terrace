using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public GameObject SeedUI;

    public GameObject placeButton;

    [Header("Moveable Object")]

    public GameObject moveableObject;

    [Header("Object info")]
    public Image objectIcon;

    public TextMeshProUGUI objectName;
    public TextMeshProUGUI objectInfo;
    public ObjectDescription emptyDescription;

    [Header("Seed info")]

    public Image seedIcon;
    public TextMeshProUGUI seedName;
    public TextMeshProUGUI seedInfo;

    [Header("Dialogues")]

    public DialogueBase placeDialogue;
    public DialogueBase moveDialogue;

    [Header("Next plant")]
    public GameObject nextPlant;

    [Header("Sounds")]
    public GameObject harvestSound;

    [Header("Slots")]
    public bool[] isFull;
    public GameObject[] canvasSlots;

    public GameObject[] invSlots;

    public ObjectDescription[] objectDescription;

    public GameObject targetSlot;

    public List<int> seeds;

    public int currentId;
    public int currentSeedId;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Fix this!" + gameObject.name);
        }
        else
        {
            instance = this;
        }

    }

    void Start()
    {

        for (int i = 0; i < canvasSlots.Length; i++){
            if(canvasSlots[i].GetComponent<InventorySlot>() != null)

                canvasSlots[i].GetComponent<InventorySlot>().slotId = i;            
        }

        //set inventory description for the first object in the list
        StartInventory();

        //set image alpha to 0
        var tempColor = objectIcon.color;
        tempColor.a = 0f;
        objectIcon.color = tempColor;

        /*
        //check for seeds and store them in an array
        for (int i = 0; i < invSlots.Length; i++)
        {
            if (invSlots[i].GetComponent<ObjectInfo>() is SeedInfo)
            {
                if (seeds.Length == 0)
                {
                    seeds[0] = i;
                } else
                {
                    seeds[seeds.Length] = i;
                }
            }
                
        }
        */
    }

    public void StartInventory()
    {
        for (int i = 0; i < canvasSlots.Length; i++)
        {

            if (canvasSlots[i].GetComponent<InventorySlot>() != null)
            {
                currentId = i;
                break;
            }
        }

        ShowDescription();
    }

    public void CollectObject()
    {
        //reassign object when you're collecting a dried plant
        //if(ClickManager.instance.interactedObject.tag == "Hanger")
        //{
        //    ClickManager.instance.interactedObject = ClickManager.instance.interactedObject.transform.GetChild(1).gameObject;
        //}

        for (int i = 0; i < canvasSlots.Length; i++)
        {
            Debug.Log("i: "+ i);
            // FIND NEXT EMPTY SLOT IN INVENTORY
            if (isFull[i] == false)
            {
                //ITEM CAN BE ADDED TO INVENTORY
                Debug.Log("can be added");
                //fill inventory slot
                isFull[i] = true;
                invSlots[i] = ClickManager.instance.interactedObject;

                canvasSlots[i].GetComponent<Button>().enabled = true;

                objectDescription[i] = ClickManager.instance.interactedObject.GetComponent<ObjectInfo>().description;

                //instantiate object icon
                Instantiate(ClickManager.instance.interactedObject.GetComponent<ObjectInfo>().itemIcon, canvasSlots[i].transform, false);

                //put object in inventory (hierarchy)
                ClickManager.instance.interactedObject.SetActive(false);
                //make exception for prefabssss
                if (ClickManager.instance.interactedObject.transform.parent != null)
                    ClickManager.instance.interactedObject.transform.parent = this.transform;
                else
                    Instantiate(ClickManager.instance.interactedObject, this.transform);


                //add seed to list
                if(ClickManager.instance.interactedObject.GetComponent<ObjectInfo>() is SeedInfo)
                {
                    seeds.Add(i);
                }          

                //break the loop    
                break;
            }
        }

        //TURN CANVAS OFF
        if (ClickManager.instance.interactedObject.tag == "Pot")
            ClickManager.instance.interactedObject.GetComponent<PotTrigger>().mainCanvas.SetActive(false);
        else if(ClickManager.instance.interactedObject.tag == "Key")
        {
            ClickManager.instance.interactedObject.GetComponent<CollectableTrigger>().mainCanvas.SetActive(false);
            if(!StoryManager.instance.firstSeeds)
            {
                StartCoroutine(InventoryHelp());
                //DialogueManager.instance.EnqueueDialogue(ClickManager.instance.interactedObject.GetComponent<CollectableTrigger>().collectDialogue);
                //return;
            }
            
        }            
        else if(ClickManager.instance.interactedObject.GetComponent<CollectableTrigger>() != null)
            ClickManager.instance.interactedObject.GetComponent<CollectableTrigger>().mainCanvas.SetActive(false);

        //DEZOOM
        if (ClickManager.instance.zoomCamera.enabled == true)
        {
            StartCoroutine(ClickManager.instance.DeZoomCameraMethod());
        }
    }

    public IEnumerator InventoryHelp()
    {
        ClickManager.instance.player.transform.Find("HelpCanvas").gameObject.SetActive(true);

        StoryManager.instance.firstSeeds = true;

        yield return new WaitForSeconds(3);

        ClickManager.instance.player.transform.Find("HelpCanvas").gameObject.SetActive(false);

    }

    public void MoveObject(GameObject targetSlot)
    {
        //put the object in the new slot
        //called from button
        moveableObject.transform.parent = targetSlot.transform;

        StateManager.instance.inventoryOn = false;
        StateManager.instance.placeSelect = false;
        StateManager.instance.moveSelect = false;
        StateManager.instance.canClick = true;
    }

    public void PlaceObject(GameObject targetSlot, int id)
    {
        //remove object from canvas
        canvasSlots[id].GetComponent<InventorySlot>().DropItem();
        canvasSlots[id].GetComponent<Button>().enabled = false;

        //disable canvas button

        //remove object from inventory
        isFull[id] = false;

        //activate and place object on desired slot
        invSlots[id].SetActive(true);
        invSlots[id].transform.parent = targetSlot.transform;

        //set position to 0
        invSlots[id].transform.localPosition = Vector2.zero;


        //empty inventory descriptions
        objectName.text = null;
        objectInfo.text = null;

        //set image description alpha to 0
        var tempColor = objectIcon.color;
        tempColor.a = 0f;
        objectIcon.color = tempColor;

        //turn off placement button
        placeButton.SetActive(false);

        //invSlots[id] = null;


        //ENABLE CLICKS
        //CLOSE INVENTORY
        //END OBJECT PLACING
        StateManager.instance.inventoryOn = false;
        StateManager.instance.placeSelect = false;
        StateManager.instance.canClick = true;
    }

    public void Plant()
    {
        //remove object from canvas
        canvasSlots[currentSeedId].GetComponent<InventorySlot>().DropItem();
        canvasSlots[currentSeedId].GetComponent<Button>().enabled = false;

        //remove object from inventory
        isFull[currentSeedId] = false;

        //activate and place object on desired slot
        //deactivate renderer

        Instantiate(nextPlant, ClickManager.instance.interactedObject.transform);

        //set position to 0
        invSlots[currentSeedId].transform.localPosition = Vector2.zero;

        if(invSlots[currentSeedId].GetComponent<ObjectInfo>().objectId == 9)
        {
            StoryManager.instance.plantedCalendula = true;
        } else if (invSlots[currentSeedId].GetComponent<ObjectInfo>().objectId == 8)
            StoryManager.instance.plantedBasil = true;

        //delete object and description
        invSlots[currentSeedId] = null;
        SceneProperties.instance.invIds[currentId] = 0;
        objectDescription[currentSeedId] = emptyDescription;

        //REMOVE FROM SEEDS
        foreach (int id in seeds)
        {
            if (currentSeedId == id)
            {
                seeds.Remove(id);
                break;
            }
        }

        if (!StoryManager.instance.hasPlanted)
            StoryManager.instance.hasPlanted = true;

        StartCoroutine(StoryManager.instance.BackLater(true));
        //ClickManager.instance.DeZoomCameraMethod();

    }

    public void HarvestPlant()
    {
        Debug.Log("was harvested");

        Plant currentPlant = ClickManager.instance.interactedObject.transform.GetChild(2).gameObject.GetComponent<Plant>();

        //get plant from pot's child
        GameObject newPlant = currentPlant.harvestable;

        for (int i = 0; i < canvasSlots.Length; i++)
        {

            // FIND NEXT EMPTY SLOT IN INVENTORY
            if (isFull[i] == false)
            {
                //ITEM CAN BE ADDED TO INVENTORY
                Debug.Log("can be added");
                //fill inventory slot
                isFull[i] = true;

                //get child from pot
                invSlots[i] = newPlant;

                canvasSlots[i].GetComponent<Button>().enabled = true;

                objectDescription[i] = newPlant.GetComponent<ObjectInfo>().description;

                //instantiate object icon
                Instantiate(newPlant.GetComponent<ObjectInfo>().itemIcon, canvasSlots[i].transform, false);
                //Instantiate(newPlant, Inventory.instance.transform);

                //destroy object
                //Destroy(ClickManager.instance.interactedObject.transform.GetChild(0).gameObject);

                //break the loop    
                break;
            }
        }

        if (ClickManager.instance.zoomCamera.enabled == true)
        {
            StartCoroutine(ClickManager.instance.DeZoomCameraMethod());
        }

        //play sound
        harvestSound.SetActive(true);
        harvestSound.SetActive(false);

        //turn off harvesting
        currentPlant.harvestCanvas.SetActive(false);
        currentPlant.transform.parent.GetComponent<PotTrigger>().circle.SetActive(true);
        currentPlant.hasHarvested = true;

        //get scene back to normal
        StateManager.instance.canClick = true;

    }

    public IEnumerator PlaceDialogue()
    {
        Debug.Log("Place object dialogue");

        ClickManager.instance.ZoomCameraMethod();

        //delay
        yield return new WaitForSeconds(ClickManager.instance.warningTime);

        TriggerDialogue(placeDialogue);
    }

    public IEnumerator MoveDialogue()
    {
        Debug.Log("Move object dialogue");

        ClickManager.instance.ZoomCameraMethod();

        //delay
        yield return new WaitForSeconds(ClickManager.instance.warningTime);

        TriggerDialogue(moveDialogue);
    }

    public void ShowDescription()
    {
        ObjectDescription description = objectDescription[currentId];

        objectIcon.sprite = description.objectIcon;
        objectName.text = description.objectName;

        //set description and remove random breaks
        string tempText = description.objectDescription.Replace("\r", "").Replace("\n", "");
        objectInfo.text = tempText;



        //if there is no object
        if(description.objectIcon == null)
        {
            Debug.Log("yayy");
            //set image alpha to 0
            var tempColor = objectIcon.color;
            tempColor.a = 0f;
            objectIcon.color = tempColor;

        }//if there is object
        else
        {
            //set image alpha to 1
            var tempColor = objectIcon.color;
            tempColor.a = 1f;
            objectIcon.color = tempColor;
        }

        if(description.placeable)
        {
            placeButton.SetActive(true);
        } else
        {
            placeButton.SetActive(false);

        }
        
    }

    //turn inventory on and off
    public void SeedInventory()
    {
        Debug.Log("seed inventory");

        if(SeedUI.activeSelf == true)
            SeedUI.SetActive(false);
        else
            SeedUI.SetActive(true);
    }

    public void ShowSeedDescription(bool open, bool next)
    {
        Debug.Log("opened seed menu");
        //when opening the inventory
        if(open)
        {
            //get the first item
            currentSeedId = seeds[0];

        } else
        {
            //if it's next button, adjust accordingly
            if (next)
            {
                if (currentSeedId == seeds[seeds.Count - 1])
                {
                    currentSeedId = seeds[0];

                }
                else
                {
                    currentSeedId = seeds[seeds.IndexOf(currentSeedId) + 1];
                }

            }             

            //if it's previous button, adjust accordingly
            else
            {
                if (currentSeedId == 0)
                {
                    currentSeedId = seeds[seeds.Count - 1];

                }
                else
                {
                    currentSeedId = seeds[seeds.IndexOf(currentSeedId) - 1];
                    
                }
            }
        }

        //show seed description
        ObjectDescription description = objectDescription[currentSeedId];

        seedIcon.sprite = description.objectIcon;

        //remove seeds from name
        string tempName = description.objectName.Replace(" Seeds", "").Replace(" seeds", "");
        seedName.text = tempName;
        seedInfo.text = description.objectDescription;

        nextPlant = invSlots[currentSeedId].GetComponent<SeedInfo>().plant;
    }

    public bool CheckHanger()
    {
        //search for dryable plants in inventory
        for (int i = 0; i < isFull.Length; i++)
        {
            if (isFull[i] == true)
            {
                //if object is a dryable plant
                if (invSlots[i].GetComponent<ObjectInfo>() is HarvestedPlantInfo)
                {
                    //ClickManager.instance.interactedObject.GetComponent<HangerScript>().nextPlant = invSlots[i];

                    return true;
                }
            }
        }

        return false;       

    }

    public void HangPlant()
    {  
        //search for dryable plants in inventory
        for (int i = 0; i < invSlots.Length; i++)
        {
            if (isFull[i] == true)
            {
                //if object is a dryable plant
                if(invSlots[i].GetComponent<ObjectInfo>() is HarvestedPlantInfo)
                {
                    if(invSlots[i].GetComponent<HarvestedPlantInfo>().isDryable)
                    {
                        //put the object to dry
                        GameObject tempObject = Instantiate(invSlots[i], ClickManager.instance.interactedObject.transform);
                        tempObject.SetActive(true);

                        //remove object from inventory
                        SceneProperties.instance.invIds[i] = 0;
                        invSlots[i] = null;

                        //remove object from canvas
                        canvasSlots[i].GetComponent<InventorySlot>().DropItem();
                        canvasSlots[i].GetComponent<Button>().enabled = false;

                        //remove description from canvas
                        objectDescription[i] = emptyDescription;

                        //remove object from inventory
                        isFull[i] = false;

                        break;
                    }                    

                }

            }

        }

        //remove zoom
        StartCoroutine(StoryManager.instance.BackLater(true));
        //ClickManager.instance.DeZoomCameraMethod();

    }

    public void TriggerDialogue(DialogueBase dialogue)
    {
        //Use to call dialogue!!
        DialogueManager.instance.EnqueueDialogue(dialogue);
    }

}
