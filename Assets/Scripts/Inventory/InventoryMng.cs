using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

public class InventoryMng : MonoBehaviour
{
    /* IDs
     * 
     * PAGES:
     * 0 - Seeds
     * 1 - Plants
     * 2 - Tools
     * 3 - Furniture
     * 
     * SEEDS:
     * 0 - Empty
     * 1 - Peppermint
     * 2 - Valerian
     * 3 - Basil
     * 4 - Calendula
     * 
     * TOOLS:
     * 0 - Empty
     * 1 - Shovel
     * 2 - Mortar & Pestle
     * 
     * FURNITURE:
     * 0 - Empty
     * 1 - Pot
     * 2 - Short Pot
     * 3 - Gray Pot
     * 
     * PLANTS:
     * 0 - Empty
     * 1 - Pepermint
     * 2 - Valerian
     * 3 - Calendula
     * 4 - Basil
     * 
     */

    public static InventoryMng instance;

    int pageSize = 6;
    int currentPage;

    [Header("Interface")]
    public GameObject plantMenu;
    public GameObject inventoryCanvas;
    public GameObject overlay;
    public Button inventoryButton;
    public GameObject invTutorial;
    public GameObject inv2Tutorial;
    public GameObject[] slotButtons;
    public GameObject[] pageButtons;

    [Header("Main Description")]
    //Inventory UI objects
    public Sprite defaultImage;
    public Image objectImage;
    public TextMeshProUGUI objectName;
    public TextMeshProUGUI objectInfo;

    [Header("Seed Description")]
    //Seed UI objects
    public Image seedImage;
    public TextMeshProUGUI seedName;
    public TextMeshProUGUI seedInfo;

    [Header("ID holder")]
    public bool seedEmpty;
    public List<int> seedsPage;
    public bool plantEmpty;
    public List<int> plantPage;
    public bool toolsEmpty;
    public List<int> toolsPage;
    public bool furnitureEmpty;
    public List<int> furniturePage;

    [Header("Planting Stuff")]
    public Pot currentPot;    
    public int currentSeed;

    [Header("List")]
    public ListOfObjects list;
    public ObjDescription[,] objects;
    //the "objects" matrix consists of [OBJECT TYPE ID, NUMBER IN LIST]

    [Header("FMOD")]
    [SerializeField]
    [FMODUnity.EventRef]
    private string collectSound;
    [SerializeField]
    [FMODUnity.EventRef]
    private string harvestSound;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Fix this!" + gameObject.name);
        else
            instance = this;
    }

    void Start()
    {
        //always have inventory off at the beginning
        inventoryCanvas.SetActive(false);

        if (StoryMng.instance.playedIntro)
            overlay.SetActive(true);
        else
            overlay.SetActive(false);
           
        InitializeObjectsList();

        CheckPageEmpty(0); //check for seeds
        CheckPageEmpty(1);
        CheckPageEmpty(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space pressed");

            if (!StateMng.instance.inventoryOn)
            {
                if(StateMng.instance.CanClick) //only open inventory if able to click
                {
                    //stop if doesn't have diary
                    if(!StoryMng.instance.gotDiary)
                    {
                        return;
                    }

                    StateMng.instance.CanClick = false;

                    //open inventory if inventory is off
                    OpenInventory();


                }
                else //if opening inventory for the first time, only time inventory can be opened without canClick
                {
                    
                    if (!StoryMng.instance.gotDiary && invTutorial.activeSelf)
                    {
                        invTutorial.SetActive(false); // close first tutorial
                        inv2Tutorial.SetActive(true); // open second tutorial
                         
                        OpenInventory();
                        
                        return;
                    }
                }

            }
            else
            {

                StateMng.instance.CanClick = true;

                //turn off inventory
                inventoryCanvas.SetActive(false);

                ExitInventory(); // for additional functions, i.e. story and state manager

            }

        }
    }

    public void CheckPageEmpty(int id)
    {
        //page in inventory and on list
        List<int> pageTemp = seedsPage;
        bool tempEmpty;
        currentPage = id;

        //assign INVENTORY page based on id
        switch (id)
        {
            case 0:
                pageTemp = seedsPage;
                break;
            case 1:
                pageTemp = plantPage;
                break;
            case 2:
                pageTemp = toolsPage;
                break;
        }

        /*
        for (int i = 0; i < pageTemp.Count; i++)
        {
            if (pageTemp[i] != 0)
            {
                tempEmpty = false;
            }
        }
        */
        if (pageTemp.Count == 0)
        {
            tempEmpty = true;
        }
        else tempEmpty = false;

        switch (id)
        {
            case 0:
                seedEmpty = tempEmpty;
                if(!seedEmpty)
                {
                    SeedDescription.currentSeed = 0; //set empty seed
                }
                break;
            case 1:
                plantEmpty = tempEmpty;
                break;
            case 2:
                toolsEmpty = tempEmpty;
                break;
        }

    }

    public void OpenInventory()
    {

        inventoryCanvas.SetActive(true);

        if(!StoryMng.instance.gotDiary) //open second tutorial if opening for the first time
        {
            inv2Tutorial.SetActive(true);
        }

        StateMng.instance.inventoryOn = true;
        ShowPage(0);

    }

    //the object variable pulls objects from the list and turns them into a 2D matrix
    public void InitializeObjectsList()
    {
        //initialize objects
        int maxLength = Mathf.Max(list.seeds.Length, list.plants.Length, list.tools.Length);

        objects = new ObjDescription[3, maxLength];
        //the "objects" matrix consists of [OBJECT TYPE ID, NUMBER IN LIST]
        for (int j = 0; j < maxLength; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    if (j < list.seeds.Length)
                        objects[i, j] = list.seeds[j];

                }
                else if (i == 1)
                {
                    if(j < list.plants.Length)
                        objects[i, j] = list.plants[j];

                }
                else if (i == 2)
                {
                    if (j < list.tools.Length)
                        objects[i, j] = list.tools[j];

                }

            }
        }

    }

    public void ChangePage(int id)
    {
        if(id != currentPage)
        {
            ShowPage(id);
        }       
    }
 
    public void ShowPage(int id)
    {
        //THE ID IS SET ON BUTTON OBJECT

        //page in inventory and on list
        List<int> pageTemp = seedsPage;
        currentPage = id;

        //assign INVENTORY page based on id
        switch (id)
        {
            case 0:
                pageTemp = seedsPage;
                break;
            case 1:
                pageTemp = plantPage;
                break;
            case 2:
                pageTemp = toolsPage;
                break;
        }

        //assign objects based on page array (getting them from the object list)
        for (int i = 0; i < pageSize; i++)
        {
            //Debug.Log("i = " + i);
            //Debug.Log("id = " + id);

            //if higher than list size, fill with empty object
            if(i >= pageTemp.Count)
            {
                slotButtons[i].GetComponent<InvSlotButton>().currentObject = objects[id, 0];

            } else // if not, continue normally
                slotButtons[i].GetComponent<InvSlotButton>().currentObject = objects[id, pageTemp[i]];

            slotButtons[i].GetComponent<InvSlotButton>().GetObject();
        }

        //reset description
        objectImage.sprite = defaultImage;
        objectName.text = null;
        objectInfo.text = null;

    }

    public int[] OrganizeSlots(int[] page)
    {
        int[] tempPage = new int[page.Length];

        //iterate through all the slots of temporary page
        for(int j = 0; j < tempPage.Length; j++)
        {
            //get values from main page and store them in temporary page, as long as they're not empty
            for (int i = 0; i < page.Length; i++)
            {
                //if slot is not empty, store it on tempPage;
                if (page[i] != 0)
                {
                    tempPage[j] = page[i];
                    break; //go to next j index/slot
                }
            }
        }

        return tempPage;
        
    }

    public void ShowPlantMenu()
    {
        plantMenu.SetActive(true);
        ShowSeedDescription(true, true);
    }

    public void ShowSeedDescription(bool open, bool next)
    {
        //get slot of current seed shown
        int currentSeedSlot;

        Debug.Log("opened seed menu");
        //when opening the inventory
        if (open)
        {
            //get the first item
            SeedDescription.currentSeed = seedsPage[0];
            currentSeedSlot = 0;

        }
        else
        {
            currentSeedSlot = seedsPage.FindIndex(x => x == SeedDescription.currentSeed);

            //if there's only one element, stop
            /*if (seedsPage.Count == 1)
            {
                return;
            }*/

            //if it's next button, adjust accordingly
            if (next)
            { 

                //if current seed is last on list, go back to beginning
                if (currentSeedSlot == seedsPage.Count - 1)
                {
                    SeedDescription.currentSeed = seedsPage[0];

                }
                else //if not, go next
                {
                    SeedDescription.currentSeed = seedsPage[seedsPage.IndexOf(SeedDescription.currentSeed) + 1];
                }

            }

            //if it's previous button, adjust accordingly
            else
            {
                //if current seed is first on list, go to last
                if (currentSeedSlot == 0)
                {
                    SeedDescription.currentSeed = seedsPage[seedsPage.Count - 1];

                }
                else
                {
                    SeedDescription.currentSeed = seedsPage[seedsPage.IndexOf(SeedDescription.currentSeed) - 1];

                }
            }
        }

        //show seed description
        ObjDescription description = objects[0, SeedDescription.currentSeed];

        seedImage.sprite = description.objectImage;

        //remove seeds from name
        string tempName = description.objectName.Replace(" Seeds", "").Replace(" seeds", "");
        seedName.text = tempName;
        seedInfo.text = description.objectInfo;
    }

    public void ExitInventory()
    {
        StateMng.instance.inventoryOn = false;

        if (!StoryMng.instance.gotDiary) //if closing for the first time
        {
            CloseTutorial(inv2Tutorial);
            StoryMng.instance.gotDiary = true;
            StoryMng.instance.moveTutorial.SetActive(true);
            InteractableObject.canInteract = true;
            Debug.Log("CAN INTERACT");
        }
    }

    public void PlaySound(bool harvest)
    {
        if(harvest) // if player is harvesting
        {
            FMODUnity.RuntimeManager.PlayOneShot(harvestSound, transform.position);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(collectSound, transform.position);
        }
        
    }

    public void CloseTutorial(GameObject tutorial)
    {
        if(tutorial.activeSelf)
        {
            tutorial.SetActive(false);
        }
    }

    public void CallFadeInventoryButton(bool on)
    {
        StartCoroutine(StoryMng.instance.FadeObject(inventoryButton.gameObject, 3f, on));
    }

} //class
