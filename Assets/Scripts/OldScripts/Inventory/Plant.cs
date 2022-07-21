using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [Header("Pot Info")]
    public GameObject myPot;
    public GameObject harvestCanvas;

    [Header("Plant Info")]
    public int plantId;
    public bool isHarvestable;
    public bool hasHarvested;
    public bool isGrown;
    public Animator plantAnimator;
    public Collider2D myCollider;


    public GameObject harvestable;
    public int plantGrowth = 0;

    public int timeState;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        //ChangeColliderSize();

        plantAnimator = gameObject.GetComponent<Animator>();
        myPot = transform.parent.gameObject;
        harvestCanvas = myPot.transform.GetChild(0).Find("Harvest").gameObject;
        
        if(plantGrowth < 3) //don't disable harvesting if loading save
            isHarvestable = false;
        
        timeState = TimeManager.instance.timeState;

        StartCoroutine(StartPlant());
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<SpriteRenderer>().sortingOrder != 3)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
            

        //ChangeColliderSize();

        if (plantGrowth == 3 && !plantAnimator.GetBool("isGrown"))
        {
            isGrown = true;
            hasHarvested = false;
            plantAnimator.SetBool("isGrown", true);
            plantAnimator.SetBool("hasHarvested", false);

        }

        // if plant's timeState is smaller than scene's, update plant's growth
        if (timeState < TimeManager.instance.timeState)
        {
            if (TimeManager.instance.timeState % 3 == 0 && StoryManager.instance.hasPlanted && !isGrown)
            {
                //update timestate for comparison
                timeState = TimeManager.instance.timeState;

                //update plant's growth
                plantGrowth = 3;

                plantAnimator.SetBool("hasHarvested", false);

                ChangeColliderSize();

                return;

            }

            //update timestate for comparison
            timeState = TimeManager.instance.timeState;

            //update plant's growth
            plantGrowth++;
            
            //reset hasharvested on checkpoints
            if(!isGrown)
            {
                hasHarvested = false;
                plantAnimator.SetBool("hasHarvested", false);

            }

        }      
        
    }

    IEnumerator StartPlant()
    {
        StateManager.instance.canClick = false;

        yield return new WaitForSeconds(2f);

        StateManager.instance.canClick = true;
    }

    void ChangeColliderSize()
    {
        //yield return new WaitForSeconds(0.5f);

        //Vector2 S = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.5f, 2.6f);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
    }

    void OnMouseOver()
    {
        if (StateManager.instance.canClick)
            //turn on interaction canvas
            myPot.GetComponent<PotTrigger>().mainCanvas.SetActive(true);
    }

    void OnMouseExit()
    {
        if (StateManager.instance.canClick)
            //turn off interaction canvas
            myPot.GetComponent<PotTrigger>().mainCanvas.SetActive(false);
    }
}
