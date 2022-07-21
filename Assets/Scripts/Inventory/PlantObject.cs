using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlantObject : MonoBehaviour
{
    static int timeState;
    public static int TimeState
    { 
        get { return timeState; }
        set 
        {
            timeState = value;

            foreach(PlantObject plant in plantList)
            {
                plant.UpdatePlantGrowth();
            }
        }
    }
    private int initialTime;

    public static List<PlantObject> plantList = new List<PlantObject>();

    /*
     * Plant Growth Stages:
     * 0: Seedling
     * 2: Grown
     * 
     */

    [Header("Pot Info")]
    public Pot myPot;

    [Header("Plant Info")]
    public int id;
    [SerializeField]
    bool dead;
    public bool hasHarvested;
    public bool isGrown;
    public Animator plantAnimator;
    public Collider2D myCollider;

    public int plantGrowth = 0;

    [Header("FMOD")]
    [SerializeField]
    [FMODUnity.EventRef]
    private string plantSound;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartPlant");
    }

    // Update is called once per frame
    void Update()
    { 
        //make sure plant is always on correct sorting layer
        if (GetComponent<SpriteRenderer>().sortingOrder != 3)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }

        if(plantGrowth == 1 && !isGrown && !dead) // when plant growth reaches 2
        {
            isGrown = true;
            plantAnimator.SetBool("isGrown", true);
            plantAnimator.SetBool("hasHarvested", false);

            // update story manager's states
            if (!StoryMng.instance.plantGrew) StoryMng.instance.plantGrew = true;

            switch (id)
            {
                case 1:
                    if (!StoryMng.instance.mintGrew) StoryMng.instance.mintGrew = true;
                    break;
                case 2:
                    if(!StoryMng.instance.valerianGrew) StoryMng.instance.valerianGrew = true;
                    break;
                default:
                    Debug.LogError("Grown plant has a wrong ID.");
                    break;
            }

            ChangeColliderSize();
            StartCoroutine(myPot.AdjustCanvasPos()); //adjust canvasPos to new plant size
        }
    }

    //pause game while plant is appearing
    IEnumerator StartPlant()
    {
        StateMng.instance.CanClick = false;

        if(id == 0)
        {
            dead = true;
        }

        initialTime = timeState; // set up initial time state
        plantList.Add(this); // add plant to plant list
        myPot = transform.parent.gameObject.GetComponent<Pot>(); // set pot
        StartCoroutine(myPot.AdjustCanvasPos()); //adjust canvasPos

        if(!dead) //if it's not a dead plant
            PlaySound(plantSound);

        yield return new WaitForSeconds(2f); 

        StateMng.instance.CanClick = true;
    }

    public void Harvest()
    {
        hasHarvested = true;

        InventoryMng.instance.PlaySound(true);

        InventoryMng.instance.plantPage.Add(id);

        myPot.harvest.SetActive(false); //disable harvest button

        StateMng.instance.CanClick = true;

    }

    void ChangeColliderSize() //changes collider's size to the size of new plant
    {
        //yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.5f, 2.6f);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
    }

    
    //These two methods enable extending pot's collider 
    void OnMouseOver()
    {
        if (StateMng.instance.CanClick && InteractableObject.canInteract && !StateMng.instance.interacting)
            //turn on interaction canvas
            myPot.objCanvas.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        if (myPot.objCanvas.gameObject.activeSelf && !StateMng.instance.interacting)
            //turn off interaction canvas
            myPot.objCanvas.gameObject.SetActive(false);
    }

    void UpdatePlantGrowth()
    {
        plantGrowth = (timeState - initialTime)/2;
    }

    private void OnDestroy()
    {
        myPot.EmptyPot();
        if(myPot.isActiveAndEnabled)
        {
            myPot.StartCoroutine(myPot.AdjustCanvasPos()); //adjust canvasPos
        }
        plantList.Remove(this);
    }

    void PlaySound(string aSound)
    {
        FMODUnity.RuntimeManager.PlayOneShot(aSound, transform.position);
    }
}
