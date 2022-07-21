using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class StoryMng : MonoBehaviour
{
    public static StoryMng instance;

    [SerializeField] DialogueRunner dialogueRunner;
    public DialogueList dialogueList;

    [Header("Story booleans")]
    public bool playedIntro = false;
    public bool gotDiary;
    public bool sawPlanks;
    public bool sawSparrows;
    public bool hasPlanted;
    public bool plantGrew;
    public bool gotShovel;

    [Header("Valerian")]
    public bool valerianGrew;
    public bool valerianDried;
    public bool metCat;
    public bool metCatAgain;

    [Header("Mint")]
    public bool mintGrew;
    public bool mintDried;
    public bool metFriend;

    [Header("Story objects")]
    public GameObject diary;
    public GameObject moveTutorial;
    public GameObject sparrows;
    public GameObject mintSeeds;
    public GameObject valerianSeeds;
    public Animator sparrowAnim;
    public GameObject title;
    public CatMove cat;
    public GameObject mysterySeed;
    public ObjMove friend;
    public Chair brownChair;
    public GameObject teaSet;
    
    [Header("Player Stuff")]
    public Vector3 originalZoomPos;
    public DialogueClamp clamp;

    [Header("Intro nodes")]
    public string introNode = "Intro.Intro";
    public string plankNode = "Intro.Plank";
    public string sparrowNode = "Intro.Sparrows";

    [Header("Cat nodes")]
    public string catStartNode = "Cat.Start";
    public string catReturnNode = "Cat.Return";

    [Header("Friend nodes")]
    public string friendStartNode = "Girlfriend.Start";
    public string friendReturnNode = "Girlfriend.Return";

    [Header("Chair Night nodes")]
    public string chairNightNode = "Chair.NightThought";
    public List<int> nightThoughtsUsedList;
    int nightNodeMax = 9;

    [Header("FMOD")]
    [SerializeField]
    [FMODUnity.EventRef]
    private string sparrowSound;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Fix this!" + gameObject.name);
        else
            instance = this;
    }

    //startpos = (-5, -5, -8);
    // Start is called before the first frame update
    void Start()
    {
        originalZoomPos = clamp.CurrentChar.boxZoomPos;

        if (!playedIntro)
            StartCoroutine("IntroScene");
        else
        {
            // diary related stuff required for skipping first scene
            gotDiary = true;
            diary.SetActive(false);
            InventoryMng.instance.overlay.SetActive(true); // add inventory to overlay

            // allow object intaraction
            InteractableObject.canInteract = true;

            //allow candle to flicker
            FlickeringLight.canFlicker = true;
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            StartDialogue(introNode);
        }

        if(playedIntro)
        {
            if(gotDiary)
            {
                if (!sawPlanks)
                {
                    if (ClickMng.instance.player.transform.position.x > 6) // 6 is the position in which scene is triggered
                    {
                        StartCoroutine("PlankScene");

                    }
                }
                else
                {
                    if (!sawSparrows)
                    {
                        if (ClickMng.instance.player.transform.position.x < -2.5f) //position in which scene is triggered
                        {
                            StartCoroutine("SparrowScene");
                        }
                    }
                }

            } //gotDiary
            
        } //playedIntro
        

    }

    IEnumerator IntroScene()
    {
        TimeMng.instance.SetNight();

        //fade in
        //zoom camera
        ClickMng.instance.StartCoroutine("ZoomIn");
        TimeMng.instance.globalLight.intensity = 0;
        TimeMng.instance.StartCoroutine("FadeIn", 1.5f);

        //start player at correct position
        ClickMng.instance.destination.transform.position = new Vector2(-8.5f, -4.5f);
        ClickMng.instance.player.transform.position = new Vector2(-8.5f, -4.5f);
        ClickMng.instance.SpriteFlip();

        //sit
        ClickMng.instance.playerAnimator.SetBool("isSitting", true);
        StateMng.instance.IsSitting = true;

        yield return new WaitForSeconds(1.5f);

        //start dialogue
        StartDialogue(introNode);

        playedIntro = true;
    }

    public IEnumerator MeetFriend()
    {
        metFriend = true;

        // turn on friend
        friend.gameObject.SetActive(true);
        
        // make firend move in
        Task t = new Task(friend.FriendMove(true));

        while (t.Running)
        {
            yield return null;
        }

        //zoom in
        t = new Task(ClickMng.instance.ZoomIn());
        while (t.Running)
            yield return null;

        StartDialogue(friendStartNode);
    }

    public IEnumerator PrepareTeaScene(System.Action onComplete)
    {
        // move player towards chair and sit
        Task t = new Task(ClickMng.instance.SitChair(true));

        // move friend and then sit
        friend.transform.position = new Vector2(-12.55f, -5f);
        friend.animator.SetBool("isSitting", true);

        // get dialogue holder
        var friendDialogueHolder = friend.GetComponent<CharacterDialogueHolder>();
        friendDialogueHolder.boxZoomPos = friendDialogueHolder.sittingZoomInPos;

        // show tea
        teaSet.SetActive(true);

        while (t.Running)
        {
            yield return null;
        }

        onComplete();
    }

    public IEnumerator MeetCat()
    {
        metCat = true;

        // turn on cat
        cat.gameObject.SetActive(true);

        // make cat move and continue when done
        Task t = new Task(cat.CatMove(true));
        while (t.Running)
        {
            yield return null;
        }

        //zoom in
        t = new Task(ClickMng.instance.ZoomIn());
        while (t.Running)
            yield return null;

        // move player and wait until in place
        ClickMng.instance.TargetPosExact = new Vector2(-2.5f, -6f);
        while(ClickMng.instance.currentMovement.Running)
        {
            yield return null;
        }

        ClickMng.instance.ReScan();

        StartDialogue(catStartNode);

    }

    public IEnumerator MeetCatAgain()
    {
        metCatAgain = true;

        // turn on cat
        cat.gameObject.SetActive(true);
        cat.transform.localScale = new Vector3(1, cat.transform.localScale.y, cat.transform.localScale.z);
        cat.transform.position = cat.endPos;

        // make cat move and continue when done
        /*
        Task t = new Task(cat.Move(true));
        while (t.Running)
        {
            yield return null;
        }*/

        //zoom in
        Task t = new Task(ClickMng.instance.ZoomIn());
        while (t.Running)
            yield return null;

        // move player and wait until in place
        ClickMng.instance.TargetPosExact = new Vector2(-2.5f, -6f);
        while (ClickMng.instance.currentMovement.Running)
        {
            yield return null;
        }

        ClickMng.instance.ReScan();

        StartDialogue(catReturnNode);

    }

    public void ShowCat()
    {
        cat.gameObject.SetActive(true);
        cat.transform.localScale = new Vector3(1, cat.transform.localScale.y, cat.transform.localScale.z);
        cat.transform.position = cat.endPos;
        cat.animator.SetBool("isSitting", true);
    }

    IEnumerator PlankScene()
    { 
        sawPlanks = true;

        Vector2 targetPos = new Vector2(10f, -5.2f); //set up correct position for scene

        Task t = new Task(PrepareScene(targetPos));

        while (t.Running) // start scene after stopped moving 
            yield return null;

        t = new Task(ClickMng.instance.ZoomIn());

        while (t.Running)
            yield return null;

        StartDialogue(plankNode); //start dialogue

    }

    IEnumerator SparrowScene()
    {
        sawSparrows = true;

        Vector2 targetPos = new Vector2(-5f, -6f);

        Task t = new Task(PrepareScene(targetPos));

        while(t.Running) // start scene after stopped moving 
            yield return null;

        t = new Task(ClickMng.instance.ZoomIn());

        while (t.Running)
            yield return null;

        StartDialogue(sparrowNode);

        InteractableObject.DisableCanvas(); //disable all interaction canvases
    }

    public void NightDialogue()
    { 
        // generate random index
        int randomNumber = Random.Range(1, 10);

        if(nightThoughtsUsedList.Count == 0)
        {
            // add it to the list of used nodes
            nightThoughtsUsedList.Add(randomNumber);
        } else
        {
            // retry until you get a new number
            while (nightThoughtsUsedList.Contains(randomNumber))
            {
                randomNumber = Random.Range(1, 10);
            }

        }

        //if list is full, empty it
        if(nightThoughtsUsedList.Count == nightNodeMax)
        {
            nightThoughtsUsedList.Clear();
        }

        StartDialogue(chairNightNode + randomNumber);
    }

    public void StartDialogue(string node)
    {
        dialogueRunner.StartDialogue(node);
        //StateMng.instance.dialogRunner.StartDialogue();
    }

    public void CallFadeDiary()
    {
        //fade away diary
        StartCoroutine(FadeObject(diary, 1.5f, false));
    }

    public IEnumerator FadeObject(GameObject obj, float duration, bool on)
    {
        float startTime = Time.time;
        float starterValue;
        float endValue;

        // on means fade in
        if (on)
        {
            starterValue = 0; 
            endValue = 1;
        }
        else //off means fade out
        {
            starterValue = 1; 
            endValue = 0;
        }

        Color objColor;
        Color endColor;

        if (obj.GetComponent<MaskableGraphic>() != null)
        {
            MaskableGraphic sprite = obj.GetComponent<MaskableGraphic>();
            endColor = sprite.color;
            objColor = endColor;

            objColor.a = starterValue;
            endColor.a = endValue;

            float time = 0;

            //lerp color for desired amount of time
            while (time < duration)
            {
                sprite.color = Color.Lerp(objColor, endColor, time/duration);

                time += Time.deltaTime;
                yield return null;
            }

        }
        else
        {
            SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
            endColor = sprite.color;
            objColor = endColor;

            objColor.a = starterValue;
            endColor.a = endValue;

            float time = 0;

            //lerp color for desired amount of time
            while (time < duration)
            {
                sprite.color = Color.Lerp(objColor, endColor, time/duration);

                time += Time.deltaTime;
                yield return null;
            }
        }
        
        
        //destroy diary
        if(obj == diary)
            Destroy(obj);

    }

    public IEnumerator SparrowSounds()
    {
        // show sparrows
        sparrows.SetActive(true);

        // make them play sounds
        SoundList.instance.PlaySound(SoundList.instance.sparrowSound, sparrows);

        yield return null;
    }

    // disables interaction canvases
    // stops interaction coroutines
    // disables clicking
    // moves to correct position
    public IEnumerator PrepareScene(Vector2 targetPos) 
    {
        if (InteractableObject.currentObj != null) // if moving towards an object, stop coroutine
        {
            if(InteractableObject.currentObj.currentInteraction != null) InteractableObject.currentObj.currentInteraction.Stop();
            StateMng.instance.interacting = false;
        }

        InteractableObject.DisableCanvas(); // disable all interaction canvases

        StateMng.instance.CanClick = false; // disable clicking

        // stop mnovement
        if (StateMng.instance.keyboardMoving) StateMng.instance.keyboardMoving = false; 
        else if (StateMng.instance.mouseMoving) StateMng.instance.mouseMoving = false;

        ClickMng.instance.TargetPosExact = targetPos; // move to correct position

        while (ClickMng.instance.currentMovement.Running)
            yield return null;
    }

}
