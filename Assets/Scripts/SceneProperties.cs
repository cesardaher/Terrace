using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class SceneProperties : MonoBehaviour
{
    public static SceneProperties instance;

    //timemanager
    public int weatherState;
    public int timeState;

    //storymanager booleans
    public bool hasPlanted;
    public bool plantGrew;
    public bool hasDriedMint;
    public bool hasDriedValerian;
    public bool birdsAppeared;
    public bool firstSeeds;
    public bool metFriend;
    public bool metCat;
    public bool gotShovel;
    public bool plantedCalendula;
    public bool plantedBasil;
    public bool seenCredits;

    //seeds on ground
    public bool seed1;
    public bool seed2;

    //main plant
    public GameObject mainPlant;
    public int mainPlantId;

    //second plant
    public GameObject secondPlant;

    public GameObject peppermintPlant;
    public bool peppermintIsHarvestable;
    public bool peppermintHasHarvested;
    public bool peppermintIsGrown;
    public int peppermintPlantGrowth;

    public GameObject valerianPlant;
    public bool valerianIsHarvestable;
    public bool valerianHasHarvested;
    public bool valerianIsGrown;
    public int valerianPlantGrowth;

    public GameObject calendulaPlant;
    public bool calendulaIsHarvestable;
    public bool calendulaHasHarvested;
    public bool calendulaIsGrown;
    public int calendulaPlantGrowth;

    public GameObject basilPlant;
    public bool basilIsHarvestable;
    public bool basilHasHarvested;
    public bool basilIsGrown;
    public int basilPlantGrowth;

    //left hanger
    public GameObject leftHanger;
    public bool leftFull;
    public int plantId;
    public int plantDrought = 0;
    public bool isDry;
    public int LeftTimeState;

    //right hanger
    public GameObject rightHanger;
    public bool rightFull;
    public int rightPlantId;
    public int rightPlantDrought = 0;
    public bool rightIsDry;
    public int RightTimeState;

    //new vars
    public int[] interactedPots;

    public int[] invIds;

    public ObjectDescription empty;

    public GameObject seedObject1;
    public GameObject seedObject2;

    public GameObject[] collectables;
    public GameObject[] gamePots;

    public GameObject peppermint;
    public GameObject harvestedPeppermint;
    public GameObject valerian;
    public GameObject harvestedValerian;
    public GameObject calendula;
    public GameObject basil;
    public GameObject deadPlant1;
    public GameObject deadPlant2;
    public GameObject deadPlant3;

    private void Awake()
    {
        if (instance != null)
            UnityEngine.Debug.LogWarning("Fix this!" + gameObject.name);
        else
            instance = this;
    }

    private void Start()
    {
        invIds = new int[15];
        interactedPots = new int[6];

        if (TimeManager.newGame)
        {
            //initialize inventory
            invIds = new int[15];
            for (int i = 0; i < invIds.Length; i++)
            {
                invIds[i] = 0;
            }

            //initialize interacted pots
            interactedPots = new int[6];
            for (int i = 0; i < interactedPots.Length; i++)
            {
                UnityEngine.Debug.Log("index: " + i);
                interactedPots[i] = 10;
            }
        }        

    }

    private void Update()
    {   
        //save system debugging options
        /*
        if (Input.GetKeyDown("o"))
        { //If you press Q
            GetScene();
        }

        if (Input.GetKeyDown("p"))
        { //If you press Q
            SpawnScene();
        }

        if (Input.GetKeyDown("s"))
        { //If you press S
            SaveScene();
        }

        if(Input.GetKeyDown("l"))
        {//If you press L
            LoadScene();

        }
        */
    }

    public void SaveScene()
    {
        UnityEngine.Debug.Log("scene saved");
        SaveSystem.SaveScene(this);
    }

    public bool LoadScene()
    {

        SceneData data = SaveSystem.LoadScene();

        if (data == null)
            return false;

        weatherState = data.weatherState;
        timeState = data.timeState;

        hasPlanted = data.hasPlanted;
        plantGrew = data.plantGrew;
        hasDriedValerian = data.hasDriedValerian;
        hasDriedMint = data.hasDriedMint;
        plantedBasil = data.plantedBasil;
        plantedCalendula = data.plantedCalendula;
        seenCredits = data.seenCredits;

        birdsAppeared = data.birdsAppeared;
        firstSeeds = data.firstSeeds;
        metFriend = data.metFriend;
        metCat = data.metCat;
        gotShovel = data.gotShovel;

        //seeds on ground
        seed1 = data.seed1;
        seed2 = data.seed2;

        //main plant 
        mainPlantId = data.mainPlantId;

        peppermintIsHarvestable = data.peppermintIsHarvestable;
        peppermintHasHarvested = data.peppermintHasHarvested;
        peppermintIsGrown = data.peppermintIsGrown;
        peppermintPlantGrowth = data.peppermintPlantGrowth;

        valerianIsHarvestable = data.valerianIsHarvestable;
        valerianHasHarvested = data.valerianHasHarvested;
        valerianIsGrown = data.valerianIsGrown;
        valerianPlantGrowth = data.valerianPlantGrowth;

        calendulaIsHarvestable = data.calendulaIsHarvestable;
        calendulaHasHarvested = data.calendulaHasHarvested;
        calendulaIsGrown = data.calendulaIsGrown;
        calendulaPlantGrowth = data.calendulaPlantGrowth;

        basilIsHarvestable = data.basilIsHarvestable;
        basilHasHarvested = data.basilHasHarvested;
        basilIsGrown = data.basilIsGrown;
        basilPlantGrowth = data.basilPlantGrowth;

        //left hanger
        leftFull = data.leftFull;
        plantId = data.plantId;
        plantDrought = data.plantDrought;
        isDry = data.isDry;
        LeftTimeState = data.LeftTimeState;

        //right hanger
        rightFull = data.rightFull;
        rightPlantId = data.rightPlantId;
        rightPlantDrought = data.rightPlantDrought;
        rightIsDry = data.rightIsDry;
        RightTimeState = data.RightTimeState;

        interactedPots = data.interactedPots;

        invIds = data.invIds;

        return true;

    }

    public void GetScene()
    {
        GetTimeManager();

        GetStoryManager();

        GetPlantState();

        GetSeeds();

        //FindInteractedPots();

        GetHangerState();

        GetInventory();
    }

    public void SpawnScene()
    {
        SetTimeManager();

        SetStoryManager();

        SetPlantState();

        SetSeeds();

        SetHangerState();

        SpawnCat();

        StartInventory();
    }

    public void GetSeeds()
    {
        if(seedObject1.activeSelf)
        {
            UnityEngine.Debug.Log("seed 1 active");
            seed1 = true;
        } else
        {
            seed1 = false;
        }

        if (seedObject2.activeSelf)
        {
            UnityEngine.Debug.Log("seed 2 active");
            seed2 = true;
        } else
        {
            seed2 = false;
        }

    }

    public void SetSeeds()
    {
        if(seed1)
        {
            UnityEngine.Debug.Log("find seed 1");
            seedObject1.SetActive(true);

            Color newColor = new Color(1, 1, 1, 1);
            seedObject1.GetComponent<SpriteRenderer>().color = newColor;
        }

        if (seed2)
        {
            UnityEngine.Debug.Log("find seed 2");
            seedObject2.SetActive(true);

            Color newColor = new Color(1, 1, 1, 1);
            seedObject2.GetComponent<SpriteRenderer>().color = newColor;
        }

        ClickManager.instance.ReScan();
    }

    public void StartInventory()
    {
        //collect all objects in inventory
        for(int i = 0; i < invIds.Length; i++)
        {
            if(invIds[i] != 0)
            {
                GameObject tempObject = Instantiate(collectables[invIds[i]], Inventory.instance.transform);

                ClickManager.instance.interactedObject = tempObject;
                Inventory.instance.CollectObject();
            }
        }
    }

    public void GetInventory()
    {
        //check all items in inventory
        for (int i = 0; i < Inventory.instance.isFull.Length; i++)
        {
            if(Inventory.instance.isFull[i])
            {
                int id = Inventory.instance.invSlots[i].GetComponent<ObjectInfo>().objectId;

                invIds[i] = id;
            }
        }


    }

    public void GetHangerState()
    {
        leftHanger = GameObject.Find("HangerLeft");
        rightHanger = GameObject.Find("HangerRight");

        //if there is a plant there
        if (leftHanger.GetComponent<HangerScript>().full)
        {
            leftFull = true;
            HarvestedPlantInfo leftPlant = leftHanger.transform.GetChild(1).GetComponent<HarvestedPlantInfo>();

            plantId = leftPlant.plantId;
            plantDrought = leftPlant.plantDrought;
            isDry = leftPlant.isDry;

            LeftTimeState = leftPlant.timeState;

        }

        if (rightHanger.GetComponent<HangerScript>().full)
        {
            rightFull = true;
            HarvestedPlantInfo rightPlant = rightHanger.transform.GetChild(1).GetComponent<HarvestedPlantInfo>();

            rightPlantId = rightPlant.plantId;
            rightPlantDrought = rightPlant.plantDrought;
            rightIsDry = rightPlant.isDry;

            RightTimeState = rightPlant.timeState;
        }
    }

    public void SetHangerState()
    {
        if(leftFull)
        {
            // if it's mint
            if(plantId == 0)
            {
                CreateHarvested(harvestedPeppermint, true);

            }
            else
            {
                CreateHarvested(harvestedValerian, true);

            }
            
        }

        if (rightFull)
        {
            // if it's mint
            if (rightPlantId == 0)
            {
                CreateHarvested(harvestedPeppermint, false);

            }
            else
            {
                CreateHarvested(harvestedValerian, false);

            }

        }
    }

    public HarvestedPlantInfo CreateHarvested(GameObject prefab, bool left)
    {
        Transform parent;

        if(left)
        {
            parent = leftHanger.transform;

        } else
        {
            parent = rightHanger.transform;
        }

        GameObject newHarvested = Instantiate(prefab, parent) as GameObject;

        HarvestedPlantInfo plantSettings = newHarvested.GetComponent<HarvestedPlantInfo>();

        if(left)
        {
            plantSettings.plantId = plantId;
            plantSettings.plantDrought = plantDrought;
            plantSettings.isDry = isDry;
            plantSettings.timeState = LeftTimeState;

        } else
        {
            plantSettings.plantId = rightPlantId;
            plantSettings.plantDrought = rightPlantDrought;
            plantSettings.isDry = rightIsDry;
            plantSettings.timeState = RightTimeState;

        }

        return plantSettings;
    }

    public void GetTimeManager()
    {
        weatherState = TimeManager.instance.weatherState;
        timeState = TimeManager.instance.timeState;
    }

    public void SetTimeManager()
    {
        TimeManager.instance.weatherState = weatherState;
        TimeManager.instance.timeState = timeState;

        TimeManager.instance.ChangeProfileCheat();
    }

    public void GetStoryManager()
    {
        hasPlanted = StoryManager.instance.hasPlanted;
        plantGrew = StoryManager.instance.plantGrew;
        hasDriedMint = StoryManager.instance.hasDriedMint;
        hasDriedValerian = StoryManager.instance.hasDriedValerian;
        birdsAppeared = StoryManager.instance.birdsAppeared;
        firstSeeds = StoryManager.instance.firstSeeds;
        metFriend = StoryManager.instance.metFriend;
        metCat = StoryManager.instance.metCat;
        gotShovel = StoryManager.instance.gotShovel;
        plantedBasil = StoryManager.instance.plantedBasil;
        plantedCalendula = StoryManager.instance.plantedCalendula;
        seenCredits = StoryManager.instance.seenCredits;
    }

    public void SetStoryManager()
    {
        StoryManager.instance.hasPlanted = hasPlanted;
        StoryManager.instance.plantGrew = plantGrew;
        StoryManager.instance.hasDriedMint = hasDriedMint;
        StoryManager.instance.hasDriedValerian = hasDriedValerian;
        StoryManager.instance.birdsAppeared = birdsAppeared;
        StoryManager.instance.firstSeeds = firstSeeds;
        StoryManager.instance.metFriend = metFriend;
        StoryManager.instance.metCat = metCat;
        StoryManager.instance.gotShovel = gotShovel;
        StoryManager.instance.plantedBasil = plantedBasil;
        StoryManager.instance.plantedCalendula = plantedCalendula;
    }

    public void GetPlantState()
    {
        if (hasPlanted)
        {
            mainPlant = GameObject.Find("EmptyPot").transform.GetChild(2).gameObject;
            
            //get the id of the main plant
            mainPlantId = mainPlant.GetComponent<Plant>().plantId;
            
            //get plant's info
            //GetEachPlant(mainPlant.GetComponent<Plant>());
        }

        FindInteractedPots();
    }

    public void GetEachPlant(Plant plant)
    {
        plantId = plant.plantId;

        if(plantId == 0)
        {
            peppermintIsHarvestable = plant.isHarvestable;
            peppermintHasHarvested = plant.hasHarvested;
            peppermintIsGrown = plant.isGrown;
            peppermintPlantGrowth = plant.plantGrowth;

        } else if (plantId == 1)
        {
            valerianIsHarvestable = plant.isHarvestable;
            valerianHasHarvested = plant.hasHarvested;
            valerianIsGrown = plant.isGrown;
            valerianPlantGrowth = plant.plantGrowth;
        }
        else if (plantId == 2)
        {
            calendulaIsHarvestable = plant.isHarvestable;
            calendulaHasHarvested = plant.hasHarvested;
            calendulaIsGrown = plant.isGrown;
            calendulaPlantGrowth = plant.plantGrowth;
        }
        else if (plantId == 3)
        {
            basilIsHarvestable = plant.isHarvestable;
            basilHasHarvested = plant.hasHarvested;
            basilIsGrown = plant.isGrown;
            basilPlantGrowth = plant.plantGrowth;
        }

    }

    public void FindInteractedPots()
    {
        for (int i = 0; i < gamePots.Length; i++)
        {
            UnityEngine.Debug.Log("my index:" + i);
            if (gamePots[i].transform.childCount == 3 && gamePots[i].transform.GetChild(2).gameObject.tag == "Plant") // if there is a living plant
            {
                Plant nextplant = gamePots[i].transform.GetChild(2).GetComponent<Plant>();

                //put plant id in the interacted pot
                interactedPots[gamePots[i].GetComponent<PotTrigger>().potId] = nextplant.plantId;

                GetEachPlant(nextplant);

            } else if(gamePots[i].transform.childCount == 3 && gamePots[i].transform.GetChild(2).gameObject.tag == "Dead Plant")
            {
                //put plant id in the interacted pot
                interactedPots[gamePots[i].GetComponent<PotTrigger>().potId] = -1;

            }
            else if (gamePots[i].transform.childCount == 2) // if pot is empty
            {
                //put plant id in the interacted pot
                interactedPots[gamePots[i].GetComponent<PotTrigger>().potId] = -2;

            }

        }

    }

    public void SetPlantState()
    {
        if(!hasPlanted)
        {
            return;

        } else
        {

            //spawn first plant
            CreatePlant(true);

            CreatePlant(false);
        }
    }

    /*
    public bool FindSecondPlant()
    {
        foreach(GameObject pot in gamePots)
        {
            if(pot.transform.childCount == 3)
            {
                if(pot.transform.GetChild(2).GetComponent<Plant>() != null)
                {
                    secondPotId = pot.GetComponent<PotTrigger>().potId;
                    secondPlant = pot.transform.GetChild(2).gameObject;
                }
                
            }
        }

        return false;
    }
    */

    public void SpawnCat()
    {
        if(metCat)
        {
            StoryManager.instance.cat.SetActive(true);
            StoryManager.instance.cat.transform.position = StoryManager.instance.catPos;
            StoryManager.instance.catAnimator.Play("catSit");
        }
    }

    public void CreatePlant(bool main)
    {
        Transform parent;

        if(main)
        {
            GameObject tempNextPlant;

            //if plant is mint
            if (mainPlantId == 0)
                tempNextPlant = peppermint;

            else
                tempNextPlant = valerian;


            parent = GameObject.Find("EmptyPot").transform;

            SpawnPlant(parent, tempNextPlant);

        } else //remove this condition
        {
            for (int i = 1; i < interactedPots.Length; i++)
            {
                UnityEngine.Debug.Log("interactedPot: " + i);

                if(interactedPots[i] != 10) //if current pot was interacted
                {
                    UnityEngine.Debug.Log("this pot was interacted");

                    //get pot
                    parent = gamePots[i].transform;

                    //destroy dead plant
                    Destroy(parent.GetChild(2).gameObject);

                    //destroy dead plant
                    Destroy(parent.GetChild(2).gameObject);

                    GameObject nextPlant = peppermint;

                    if(interactedPots[i] != -2) //if it's not empty
                    {
                        switch (interactedPots[i])
                        {
                            case -1:
                                switch (i)
                                {
                                    case 1:
                                        nextPlant = deadPlant1;
                                        break;
                                    case 2:
                                        nextPlant = deadPlant2;
                                        break;
                                    case 3:
                                        nextPlant = deadPlant3;
                                        break;
                                    case 4:
                                        nextPlant = deadPlant1;
                                        break;
                                    case 5:
                                        nextPlant = deadPlant3;
                                        break;
                                }
                                break;
                            case 0:
                                nextPlant = peppermint;
                                break;
                            case 1:
                                nextPlant = valerian;
                                break;
                            case 2:
                                nextPlant = calendula;
                                break;
                            case 3:
                                nextPlant = basil;
                                break;

                        }

                        UnityEngine.Debug.Log("spawned: " + i);
                        SpawnPlant(parent, nextPlant);

                    }                
                                       

                }
            }

        }
    }

    public Plant SpawnPlant(Transform parent, GameObject plant)
    {
        GameObject newPlant = Instantiate(plant, parent);

        if (plant.tag == "DeadPlant")
        {
            return null;
        }

        Plant plantSettings = newPlant.GetComponent<Plant>();        

        if (plant == peppermint)
        {
            plantSettings.plantGrowth = peppermintPlantGrowth;

            //initialize plant settings
            plantSettings.isHarvestable = peppermintIsHarvestable;
            plantSettings.hasHarvested = peppermintHasHarvested;
            if (peppermintHasHarvested)
                plantSettings.GetComponent<Animator>().SetBool("hasHarvested", true);

            plantSettings.isGrown = peppermintIsGrown;
            if (plantSettings.plantGrowth >= 3 && !plantSettings.isGrown)
            {
                plantSettings.isGrown = !plantSettings.isGrown;
            }

            if (plantSettings.isGrown)
            {
                plantSettings.GetComponent<Animator>().SetBool("isGrown", true);
                plantSettings.GetComponent<Animator>().Play("MintIdle");
                newPlant.GetComponent<StudioEventEmitter>().enabled = false;
            }

        } else if (plant == valerian)
        {
            plantSettings.plantGrowth = valerianPlantGrowth;

            //initialize plant settings
            plantSettings.isHarvestable = valerianIsHarvestable;
            plantSettings.hasHarvested = valerianHasHarvested;
            if (valerianHasHarvested)
                plantSettings.GetComponent<Animator>().SetBool("hasHarvested", true);

            plantSettings.isGrown = valerianIsGrown;
            if (plantSettings.plantGrowth >= 3 && !plantSettings.isGrown)
            {
                plantSettings.isGrown = !plantSettings.isGrown;
            }

            if (plantSettings.isGrown)
            {
                plantSettings.GetComponent<Animator>().SetBool("isGrown", true);
                plantSettings.GetComponent<Animator>().Play("MintIdle");
                newPlant.GetComponent<StudioEventEmitter>().enabled = false;
            }

        }
        else if (plant == calendula)
        {
            plantSettings.plantGrowth = calendulaPlantGrowth;

            //initialize plant settings
            plantSettings.isHarvestable = calendulaIsHarvestable;
            plantSettings.hasHarvested = calendulaHasHarvested;
            if (calendulaHasHarvested)
                plantSettings.GetComponent<Animator>().SetBool("hasHarvested", true);

            plantSettings.isGrown = calendulaIsGrown;
            if (plantSettings.plantGrowth >= 3 && !plantSettings.isGrown)
            {
                plantSettings.isGrown = !plantSettings.isGrown;
            }

            if (plantSettings.isGrown)
            {
                plantSettings.GetComponent<Animator>().SetBool("isGrown", true);
                plantSettings.GetComponent<Animator>().Play("CalendulaIdle");
                newPlant.GetComponent<StudioEventEmitter>().enabled = false;
            }

        }
        else if (plant == basil)
        {
            plantSettings.plantGrowth = basilPlantGrowth;

            //initialize plant settings
            plantSettings.isHarvestable = basilIsHarvestable;
            plantSettings.hasHarvested = basilHasHarvested;
            if (basilHasHarvested)
                plantSettings.GetComponent<Animator>().SetBool("hasHarvested", true);

            plantSettings.isGrown = basilIsGrown;
            if (plantSettings.plantGrowth >= 3 && !plantSettings.isGrown)
            {
                plantSettings.isGrown = !plantSettings.isGrown;
            }

            if (plantSettings.isGrown)
            {
                plantSettings.GetComponent<Animator>().SetBool("isGrown", true);
                plantSettings.GetComponent<Animator>().Play("BasilIdle");
                newPlant.GetComponent<StudioEventEmitter>().enabled = false;
            }

        } 

        /*
         plantSettings.plantGrowth = plantGrowth;

         //initialize plant settings
         plantSettings.isHarvestable = isHarvestable;
         plantSettings.hasHarvested = hasHarvested;
         if (hasHarvested)
             plantSettings.GetComponent<Animator>().SetBool("hasHarvested", true);


         plantSettings.isGrown = isGrown;
         if (plantSettings.plantGrowth >= 3 && !isGrown)
         {
             isGrown = !isGrown;
         }

         if (isGrown)
         {
             plantSettings.GetComponent<Animator>().SetBool("isGrown", true);
             plantSettings.GetComponent<Animator>().Play("MintIdle");
             newPlant.GetComponent<StudioEventEmitter>().enabled = false;
         }
        */

        return plantSettings;
    }

}
