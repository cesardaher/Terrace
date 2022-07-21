using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;

    [Header("General")]
    public bool cutscene;
    public bool next;
    public Animator playerAnimator;
    public GameObject playerBox;
    public GameObject otherBox;
    public DialogueBase backLater;
    public DialogueBase weedLater;
    public bool seenCredits;

    [Header("Plants")]
    public bool plantGrew;
    public bool hasDriedMint;
    public bool hasDriedValerian;
    public bool plantedCalendula;
    public bool plantedBasil;

    [Header("First day")]
    public bool birdsAppeared;
    public bool firstSeeds;
    public bool hasPlanted;
    public int currentChair;
    public GameObject seed1;
    public GameObject seed2;
    public GameObject sandwich;
    public GameObject shovel;
    public DialogueBase[] firstDayDialogues;

    [Header("Second day scene")]
    public DialogueBase firstHarvest;

    [Header("Night scene")]
    public bool metFriend;
    public DialogueBase teaDialogue;
    public GameObject friend;

    [Header("Cat scene")]
    public bool metCat;
    public DialogueBase catDialogue;
    public GameObject catClampBox;

    [Header("Neighbour")]
    public bool gotShovel;
    public GameObject neighbour;
    public GameObject neighbourClampBox;
    public Animator neighbourAnim;
    public DialogueBase neighbourDialogue;

    [Header("Objects")]
    public GameObject cat;
    public GameObject birds;
    public GameObject chairWhite;
    public GameObject teaSet;
    public GameObject sparrowSounds;

    [Header("End")]
    public DialogueBase finalDialogue;

    [Header("Animation")]
    public Animator birdsAnimator;
    public Animator catAnimator;

    [Header("CreditsCanvas")]
    public bool creditsOpen;
    public GameObject credits;
    public GameObject creditsContinue;
    public GameObject creditsThanks;
    public GameObject creditsNames;
    public GameObject creditsBG;


    [Header ("Positions")]
    public Vector3 catPos = new Vector3(1.5f, -6f, -6);

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Fix this!" + gameObject.name);
        else
            instance = this;

        birds.SetActive(false);
        sparrowSounds.SetActive(false);

    }

    private void Update()
    {
        
        if (Input.GetKeyDown("q"))
        { //If you press Q
            StartCoroutine(SkipBeginning());
        }

        /*

        if (Input.GetKeyDown("w"))
        { //If you press W
            StartCoroutine(TeaTime());
        }


        if (Input.GetKeyDown("e"))
        { //If you press E
            StartCoroutine(MeetCat());
        }

        if (Input.GetKeyDown("r"))
        { //If you press R
            StartCoroutine(MeetNeighbour());
        }

        //while in cutscene, disable clicking
        if (cutscene)
        {
            StateManager.instance.canClick = false;
        }
         */
        /*
        if (Input.GetKeyDown("y"))
        { //If you press R
            StartCoroutine(ShowCredits());
        }*/

        //close credits
        if (Input.GetKeyDown(KeyCode.Space) && creditsOpen)
        {
            creditsOpen = false;            
        }
       
    }

    public IEnumerator ShowCredits()
    {
        cutscene = true;

        DialogueManager.instance.EnqueueDialogue(finalDialogue);
        while (StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }

        credits.SetActive(true);
        creditsOpen = true;        

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime/ 0.5f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));
            Color blackColor = new Color(0, 0, 0, Mathf.Lerp(0, 0.8f, t));
            creditsBG.GetComponent<Image>().color = blackColor;
            creditsThanks.GetComponent<TextMeshProUGUI>().color = newColor;
            creditsContinue.GetComponent<TextMeshProUGUI>().color = newColor;

            for (int i = 0; i < 3; i++)
            {
                creditsNames.transform.GetChild(i).GetComponent<TextMeshProUGUI>().color = newColor;
            }

            yield return null;
        }

        Debug.Log("finished credits");

        while (creditsOpen) { yield return new WaitForSeconds(0.1f); }

        yield return StartCoroutine(StopCredits());

        
    }

    public IEnumerator StopCredits()
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime/ 0.5f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
            Color blackColor = new Color(0, 0, 0, Mathf.Lerp(0.8f, 0, t));
            creditsBG.GetComponent<Image>().color = blackColor;
            creditsThanks.GetComponent<TextMeshProUGUI>().color = newColor;
            creditsContinue.GetComponent<TextMeshProUGUI>().color = newColor;

            for (int i = 0; i < 3; i++)
            {
                creditsNames.transform.GetChild(i).GetComponent<TextMeshProUGUI>().color = newColor;
            }

            yield return null;
        }

        credits.SetActive(false);
        creditsOpen = false;

        cutscene = false;
        StartCoroutine(ClickManager.instance.DeZoomCameraMethod());
        StateManager.instance.canClick = true;

    }

    public IEnumerator MeetNeighbour()
    {
        //set boxes positions
        Vector3 playerBoxPos = new Vector3(1f, 12.5f, 10);
        Vector3 otherBoxPos = new Vector3(0f, 10f, 10);

        cutscene = true;
        next = false;

        //set box
        otherBox = neighbourClampBox;
        //set position
        playerBox.transform.localPosition = playerBoxPos;
        otherBox.transform.localPosition = otherBoxPos;

        //turn off cat's box
        if (StoryManager.instance.metCat)
            catClampBox.SetActive(false);

        //start neighbour
        neighbour.SetActive(true);
        neighbour.GetComponent<ObjectMovement>().moveIn = true;
        otherBox.transform.localPosition = otherBoxPos;
        neighbourAnim.SetBool("isWalking", true);
        while (neighbour.GetComponent<ObjectMovement>().moveIn) { yield return new WaitForSeconds(0.1f); }
        neighbourAnim.SetBool("isWalking", false);

        //move character
        ClickManager.instance.pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.ContinueToExactDestination;
        ClickManager.instance.destination.transform.position = new Vector3(-7.5f, -6.5f, -8f);
        StateManager.instance.isMoving = true;

        //stop movement
        while (ClickManager.instance.player.transform.position.x != ClickManager.instance.destination.transform.position.x) { yield return new WaitForSeconds(0.1f); }
        ClickManager.instance.StopMovement();
        ClickManager.instance.CheckSpriteFlip(neighbour.transform);
        ClickManager.instance.CheckSpriteFlip(neighbour.transform);

        //talk to neighbour
        DialogueManager.instance.EnqueueDialogue(neighbourDialogue);
        while (StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }

        FMODUnity.RuntimeManager.PlayOneShot("event:/NewObjectCollection_Sound");

        //get shovel
        ClickManager.instance.interactedObject = shovel;
        Inventory.instance.CollectObject();

        //get calendula seeds
        ClickManager.instance.interactedObject = SceneProperties.instance.collectables[9];
        Inventory.instance.CollectObject();

        Debug.Log("neighbour move out");
        //make neighbour move out
        Vector3 tempScale = neighbour.transform.localScale;
        tempScale.x *= -1;
        neighbour.transform.localScale = tempScale;
        neighbour.GetComponent<ObjectMovement>().moveOut = true;
        neighbourAnim.SetBool("isWalking", true);
        yield return new WaitForSeconds(3);

        //go back to normal
        gotShovel = true;
        cutscene = false;
        StateManager.instance.canClick = true;

        //get pathfinder to go to stop in near distance
        ClickManager.instance.pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.Stop;
        
        //disable neighbour
        while (neighbour.GetComponent<ObjectMovement>().moveOut) { yield return new WaitForSeconds(0.1f); }

        //turn on cat's box
        if (StoryManager.instance.metCat)
            catClampBox.SetActive(true);

        neighbour.SetActive(false);


        

    }

    public IEnumerator ChairAndBirds()
    {

        //box positions
        Vector3 playerBoxPos = new Vector3(2.5f, 12, 10);
        Vector3 otherBoxPos = new Vector3(-1.7f, 12f, 10);

        cutscene = true;
        next = false;

        //sit
        StartCoroutine(ClickManager.instance.Sit());

        //wait for cue
        while (!next) { yield return new WaitForSeconds(0.1f); }
        next = true;

        //set dialogue box position based on chair
        if (currentChair == 0)
        {
            playerBox.transform.localPosition = otherBoxPos;

        }
        else if (currentChair == 1)
        {
            playerBox.transform.localPosition = playerBoxPos;
        }

        //turn on sandwich
        sandwich.SetActive(true);

        //make sandwich appear
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.5f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));
            sandwich.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }

        //START MONOLOGUE
        DialogueManager.instance.EnqueueDialogue(firstDayDialogues[1]);
        while (StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }
        //ClickManager.instance.StartCoroutine("DeZoomCamera");

        //disable canClick
        StateManager.instance.canClick = false;

        //FADEOUT

        StartCoroutine(TimeManager.instance.FadeOut(3));

        //wait for cue
        while (!next) { yield return new WaitForSeconds(0.1f); }

        yield return new WaitForSeconds(5);

        sparrowSounds.SetActive(true);

        //turn off sandwich
        sandwich.SetActive(false);

        //REMOVE SANDWICH FROM INVENTORY
        Inventory.instance.isFull[0] = false;
        Inventory.instance.invSlots[0] = null;
        Inventory.instance.objectDescription[0] = Inventory.instance.emptyDescription;
        //remove object from canvas
        Inventory.instance.canvasSlots[0].GetComponent<InventorySlot>().DropItem();
        Inventory.instance.canvasSlots[0].GetComponent<Button>().enabled = false;

        //make birds appear
        birds.SetActive(true);

        //FADE IN
        StartCoroutine(TimeManager.instance.FadeIn(3));

        yield return new WaitForSeconds(3);

        //birb sound

        //DIALOGUE SCARE BIRB
        DialogueManager.instance.EnqueueDialogue(firstDayDialogues[2]);
        while (StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }
        //ClickManager.instance.StartCoroutine("DeZoomCamera");

        //birb fly away
        birdsAnimator.SetBool("flyAway", true);
        sparrowSounds.SetActive(false);

        //DIALOGUE SCARE BIRB

        //wait until both of them are out
        yield return new WaitForSeconds(4);

        Debug.Log("birbs away");

        DialogueManager.instance.EnqueueDialogue(firstDayDialogues[3]);
        while (StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }
        //ClickManager.instance.StartCoroutine("DeZoomCamera");

        //yield return new WaitForSeconds(1.5f);

        //birbs return with two seeds
        birdsAnimator.SetBool("flyAway", false);
        birdsAnimator.SetBool("comeBack", true);

        yield return new WaitForSeconds(7);
        sparrowSounds.SetActive(true);

        //make seeds appear
        seed1.SetActive(true);
        seed2.SetActive(true);

        ClickManager.instance.ReScan();

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.5f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));
            seed1.GetComponent<SpriteRenderer>().color = newColor;
            seed2.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }

        yield return new WaitForSeconds(6);

        //remove birbs
        birds.SetActive(false);
        sparrowSounds.SetActive(false);

        //move character
        ClickManager.instance.destination.transform.position = new Vector3(-4.2f, -6f, -8f);
        playerAnimator.SetBool("isSitting", false);
        StateManager.instance.isMoving = true;

        //stop movement
        while (ClickManager.instance.player.transform.position.x != ClickManager.instance.destination.transform.position.x) { yield return new WaitForSeconds(0.1f); }
        ClickManager.instance.StopMovement();
        
        //set dialogue box position
        playerBox.transform.localPosition = new Vector3(1.4f, 14.4f, 10f);

        //react to the seeds
        DialogueManager.instance.EnqueueDialogue(firstDayDialogues[4]);
        while (StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }
        ClickManager.instance.StartCoroutine("DeZoomCamera");

        //end cutscene
        cutscene = false;
        birdsAppeared = true;
        StateManager.instance.canClick = true;

        //get pathfinder to go to stop in near distance
        ClickManager.instance.pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.Stop;
    }

    public IEnumerator SkipBeginning()
    {
        birdsAppeared = true;

        //REMOVE SANDWICH FROM INVENTORY
        Inventory.instance.isFull[0] = false;
        Inventory.instance.invSlots[0] = null;
        Inventory.instance.objectDescription[0] = Inventory.instance.emptyDescription;
        //remove object from canvas
        Inventory.instance.canvasSlots[0].GetComponent<InventorySlot>().DropItem();
        Inventory.instance.canvasSlots[0].GetComponent<Button>().enabled = false;

        //make seeds appear
        seed1.SetActive(true);
        seed2.SetActive(true);
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.5f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));
            seed1.GetComponent<SpriteRenderer>().color = newColor;
            seed2.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }

    }

    //maybe remove this
    public IEnumerator IdleNight()
    {
        //box positions
        Vector3 playerBoxPos = new Vector3(2.5f, 12, 10);
        Vector3 otherBoxPos = new Vector3(-1.7f, 12f, 10);

        cutscene = true;
        next = false;

        //sit
        StartCoroutine(ClickManager.instance.Sit());

        if (currentChair == 0)
        {
            playerBox.transform.localPosition = otherBoxPos;

        }
        else if (currentChair == 1)
        {
            playerBox.transform.localPosition = playerBoxPos;
        }

        //if it's the first night
        else if (TimeManager.instance.timeState == 5)
        {
            DialogueManager.instance.EnqueueDialogue(firstDayDialogues[5]);
        }

        yield return null;
    }

    public IEnumerator TeaTime()
    {
        //set otherbox as friend's
        otherBox = friend.transform.GetChild(0).gameObject;

        //box positions
        Vector3 playerBoxPos = new Vector3(2.5f, 12, 10);
        Vector3 otherBoxPos = new Vector3(1.7f, 6.2f, 10);

        playerBox.transform.localPosition = playerBoxPos;
        otherBox.transform.localPosition = otherBoxPos;

        cutscene = true;
        next = false;

        //fade out
        StartCoroutine(TimeManager.instance.FadeOut(3));

        while (!next) { yield return new WaitForSeconds(0.1f); }
        next = false;

        //FOR TESTING, REMOVE THIS IN FINAL VERSION
        ClickManager.instance.interactedObject = chairWhite;
        
        //yield return new WaitForSeconds(2);

        //sit while fading out
        StartCoroutine(ClickManager.instance.Sit());

        //make objects appear in scene
        teaSet.SetActive(true);

        while (!next) { yield return new WaitForSeconds(0.1f); }
        next = false;

        //fade in
        StartCoroutine(TimeManager.instance.FadeIn(3));

        while (!next) { yield return new WaitForSeconds(0.1f); }
        next = false;

        DialogueManager.instance.EnqueueDialogue(teaDialogue);

        while (StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }

        ClickManager.instance.interactedObject = SceneProperties.instance.collectables[8];
        Inventory.instance.CollectObject();
        FMODUnity.RuntimeManager.PlayOneShot("event:/NewObjectCollection_Sound");

        //start passing the time
        StartCoroutine(TimeManager.instance.PassTime());

        StartCoroutine(ClickManager.instance.DeZoomCameraMethod());

        yield return new WaitForSeconds(3f);
        
        // make objects disappear in scene
        teaSet.SetActive(false);

        metFriend = true;
        cutscene = false;

        //make player stand
        ClickManager.instance.playerAnimator.SetBool("isSitting", false);

        //reset box positions
        playerBox.transform.localPosition = playerBox.GetComponent<ClampBox>().boxZoomPos;
        otherBox.transform.localPosition = otherBox.GetComponent<ClampBox>().boxFirstPos;

        //fade out
        //StartCoroutine(TimeManager.instance.FadeOut(3));

        //while (!next) { yield return new WaitForSeconds(0.1f); }
        //next = false;

        //fade in
        //StartCoroutine(TimeManager.instance.FadeIn(3));


        //wait for cue
        

    }

    public IEnumerator MeetCat()
    {

        Debug.Log("met cat");

        //start cutscene
        cutscene = true;
        next = false;

        //enable cat
        cat.SetActive(true);

        //set cat's positions
        Vector3 playerBoxPos = new Vector3(-1f, 12.5f, 10);
        Vector3 otherBoxPos = new Vector3(1f, 6.2f, 10);
        Vector3 catInitialPos = new Vector3(11f, -6f, -6);

        //put cat in correct position
        cat.transform.position = catInitialPos;

        //set otherbox as cat's
        otherBox = catClampBox;
        //set position
        playerBox.transform.localPosition = playerBoxPos;
        otherBox.transform.localPosition = otherBoxPos;
        //set cat's position
        cat.GetComponent<ObjectMovement>().endPos = catPos;

        //move cat
        cat.GetComponent<ObjectMovement>().endPos = catPos;
        cat.GetComponent<ObjectMovement>().moveIn = true;
        while(cat.GetComponent<ObjectMovement>().moveIn) { yield return new WaitForSeconds(0.1f); }

        catAnimator.SetBool("isSitting", true);

        ClickManager.instance.ReScan();

        //move character
        ClickManager.instance.pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.ContinueToExactDestination;
        ClickManager.instance.destination.transform.position = new Vector3(-1.5f, -5.8f, -8f);
        StateManager.instance.isMoving = true;

        //stop movement
        while (ClickManager.instance.player.transform.position.x - ClickManager.instance.destination.transform.position.x > 0.01f || ClickManager.instance.player.transform.position.x - ClickManager.instance.destination.transform.position.x < -0.01f) { yield return new WaitForSeconds(0.1f); }
        ClickManager.instance.StopMovement();

        yield return new WaitForSeconds(0.1f);
        //react to the cat
        DialogueManager.instance.EnqueueDialogue(catDialogue);
        while (StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }

        //end cutscene
        cutscene = false;
        metCat = true;
        StateManager.instance.canClick = true;

        //get pathfinder to go to stop in near distance
        ClickManager.instance.pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.Stop;

    }


    public IEnumerator BackLater(bool plant)
    {
        cutscene = true;

        yield return new WaitForSeconds(1.5f);

        if(plant)
        {
            DialogueManager.instance.EnqueueDialogue(backLater);

        } else
        {
            DialogueManager.instance.EnqueueDialogue(weedLater);
        }

        while (StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }

        StartCoroutine(TimeManager.instance.PassTime());

        cutscene = false;
    }
}
 