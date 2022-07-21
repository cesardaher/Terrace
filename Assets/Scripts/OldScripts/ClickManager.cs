using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
using Pathfinding;

public class ClickManager : MonoBehaviour
{
    public static ClickManager instance;

    //mouse hit variables
    Vector2 movement;
    Vector3 mousePos;
    Vector3 mousePosWorld;
    Vector2 mousePosWorld2D;
    RaycastHit2D hit;
    public Collider2D clickBounds;

    [Header("Camera")]
    public GameObject currentVirtualCamera;
    public GameObject newVirtualCamera;
    public GameObject zoomOutVirtualCamera;
    public CinemachineVirtualCamera currentCamera;
    public CinemachineVirtualCamera zoomCamera;
    public CinemachineVirtualCamera zoomOutCamera;
    //public bool zoomActive = false;

    [Header("Player Settings")]
    public GameObject player;
    public GameObject playerSprite;
    public Animator playerAnimator;
    public Vector3 spawnPos = new Vector3(-5, -5, -8);
    public Vector2 targetPos;
    public float movSpeed;
    public AIPath pathfinder;
    public GameObject destination;
    public Rigidbody2D rb;

    [Header("Interaction")]
    public float nearObjectDistance = 1.5f;
    public GameObject interactedObject;

    [Header("Place Object")]
    public Renderer[] slot;
    public GameObject targetSlot;
    
    [Header("Dialogue Canvas")]
    public float warningTime;
    public GameObject textBox;

    [Header("Inventory")]
    public Inventory inventory;
    public GameObject inventoryCanvas;

    [Header("Planting")]
    public GameObject plantMenu;

    [Header("Floor Stations")]
    public GameObject Floor1;
    public GameObject Floor2;
    public GameObject Floor3;
    public GameObject leftButton;
    public GameObject rightButton;
    public int currentStation;


    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Fix this!" + gameObject.name);
        else
            instance = this;     

    }

    void Start()
    {

        //initialize player position
        StartCoroutine(SpawnPlayer());

        //initialize inventory and turn off canvas
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        inventoryCanvas.SetActive(false);

        //get cameras
        currentCamera = currentVirtualCamera.GetComponent<CinemachineVirtualCamera>();
        zoomCamera = newVirtualCamera.GetComponent<CinemachineVirtualCamera>();
        zoomOutCamera = zoomOutVirtualCamera.GetComponent<CinemachineVirtualCamera>();

        //set destination as player's position
        destination.transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //inventory interaction
        if (StateManager.instance.inventoryOn)
        {
            if (StateManager.instance.placeSelect)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    hit = GetMousePosition();

                    if (hit.collider != null)
                    {
                        //assign interacted object
                        interactedObject = hit.collider.gameObject;

                        if (interactedObject.tag == "Slot" && !interactedObject.GetComponent<ObjectSlot>().hasChild)
                        {

                            //define distance between player and object
                            float playerObjDistance = GetDistance();

                            // if object is far, go to object
                            if (playerObjDistance > nearObjectDistance)
                            {
                                GoToObject(interactedObject);
                                //CONTINUES IN FIXEDUPDATE
                            }
                            else
                            {
                                if(StateManager.instance.moveSelect)
                                {
                                    //MOVE OBJECT
                                    StartCoroutine(Inventory.instance.MoveDialogue());

                                }
                                //PLACE OBJECT
                                inventory.StartCoroutine(inventory.PlaceDialogue());
                                //inventory.PlaceObject(interactedObject, inventory.currentId);

                            }

                        } else if(interactedObject.tag == "Ground")
                        {

                            //I want the player to be able to change directions only if they're not going towards a specific object
                            //canClick = false;

                            //SET TARGET POSITION AND GET PLAYER MOVING (FixedUpdate)
                            targetPos = hit.point;

                            //PLAYER IS NOW MOVING
                            StateManager.instance.isMoving = true;

                            //CHANGE DESTINATION POSITION
                            destination.transform.position = targetPos;

                            Debug.Log("is moving");

                        }
                    }
                }

            }
        }

        else // pay attention to this

        //normal clicking
        if(StateManager.instance.canClick && !StateManager.instance.inDialogue)
        {

            if (Input.GetMouseButtonDown(0))
            {
                hit = GetMousePosition();

                //StartCoroutine(PreventFurtherClicks());

                if(hit.collider != null)
                {
                    if(playerAnimator.GetBool("isSitting"))
                    {
                        StartCoroutine(DeZoomCameraMethod());
                        playerAnimator.SetBool("isSitting", false);
                    }

                    //ASSIGN INTERACTED OBJECT OBJECT
                    interactedObject = hit.collider.gameObject;

                    CheckSpriteFlip(interactedObject.transform);

                    //CHECK WHICH OBJECT
                    if (interactedObject.tag == "Ground")
                    {
                        //I want the player to be able to change directions only if they're not going towards a specific object
                        //canClick = false;

                        //SET TARGET POSITION AND GET PLAYER MOVING (FixedUpdate)
                        targetPos = hit.point;

                        //PLAYER IS NOW MOVING
                        StateManager.instance.isMoving = true;

                        //CHANGE DESTINATION POSITION
                        destination.transform.position = targetPos;

                        Debug.Log("is moving");

                        //face sprite to the correct direction
                        CheckSpriteFlip(destination.transform);

                        //CONTINUES IN FIXEDUPDATE

                    } else if (interactedObject.tag == "Key" || interactedObject.tag == "Pot" || interactedObject.tag == "Character" || interactedObject.tag == "Door" || interactedObject.tag == "Chair" || interactedObject.tag == "Hanger")
                    {

                        //make player face object
                        CheckSpriteFlip(interactedObject.transform);

                        //define distance between player and object
                        float playerObjDistance = GetDistance();

                        // if object is far, go to object
                        if (playerObjDistance > nearObjectDistance) {

                            GoToObject(interactedObject);
                            //CONTINUES IN FIXEDUPDATE
                        }
                        else
                        {
                            //if player is already near the object, start interaction
                            StartCoroutine(InteractWithObject()); 
                        }



                    } else if(interactedObject.tag == "Plant")
                    {
                        interactedObject = interactedObject.GetComponent<Plant>().myPot;

                        //make player face object
                        CheckSpriteFlip(interactedObject.transform);

                        //define distance between player and object
                        float playerObjDistance = GetDistance();

                        // if object is far, go to object
                        if (playerObjDistance > nearObjectDistance)
                        {

                            GoToObject(interactedObject);
                            //CONTINUES IN FIXEDUPDATE
                        }
                        else
                        {
                            //if player is already near the object, start interaction
                            StartCoroutine(InteractWithObject());
                        }


                    } else if(interactedObject.tag == "Dryable")
                    {
                        interactedObject = interactedObject.GetComponent<HarvestedPlantInfo>().parent;

                        //make player face object
                        CheckSpriteFlip(interactedObject.transform);

                        //define distance between player and object
                        float playerObjDistance = GetDistance();

                        // if object is far, go to object
                        if (playerObjDistance > nearObjectDistance)
                        {

                            GoToObject(interactedObject);
                            //CONTINUES IN FIXEDUPDATE
                        }
                        else
                        {
                            //if player is already near the object, start interaction
                            StartCoroutine(InteractWithObject());
                        }

                    }
                }                
            }
            /*
            if (Input.GetKeyDown("a"))
            { //If you press A
                player.GetComponent<DialogueTrigger>().TriggerDialogue(); //play start dialogue
            }
            /*
            if (Input.GetKeyDown("s"))
            { //If you press S
                ReScan();
            }
            */

            if(currentStation == 1)
            {
                leftButton.SetActive(false);
                rightButton.SetActive(true);

            } else if(currentStation == 3)
            {
                leftButton.SetActive(true);
                rightButton.SetActive(false);
            } else if(currentStation == 2)
            {
                leftButton.SetActive(true);
                rightButton.SetActive(true);

            }

            if (Input.GetKeyDown(KeyCode.Space))
            { //If you press P

                // if player isnot moving
                if (!StateManager.instance.isMoving)
                {
                    ToggleInventory();
                    return;
                }                   
                
            }

        }

        if (StateManager.instance.inventoryOn)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            { //If you press space

                ToggleInventory();

            }

        }

    }

    void FixedUpdate() {
        
        //if clicked, move player
        if(StateManager.instance.isMoving)
        {
            //face sprite to the correct direction
            CheckSpriteFlip(destination.transform);

            //move the player and activate animation
            playerAnimator.SetBool("isMoving", true);

            //update distance between player and object
            float playerObjDistanceX = player.transform.position.x - destination.transform.position.x;

            //get the positive value
            if (playerObjDistanceX < 0)
                playerObjDistanceX *= -1;

            if(pathfinder.whenCloseToDestination == Pathfinding.CloseToDestinationMode.Stop) {

                //if destination is reached, stop moving
                if (playerObjDistanceX <= nearObjectDistance)
                {
                    Debug.Log("stop player2");
                    Debug.Log(playerObjDistanceX);

                    //if player is going to an object, interact
                    if (StateManager.instance.goToObject)
                        if (StateManager.instance.placeSelect)
                        {
                            if (StateManager.instance.moveSelect)
                            {
                                StartCoroutine(Inventory.instance.MoveDialogue());
                            }

                            //PLACE OBJECT
                            //SET DIALOGUE
                            StartCoroutine(inventory.PlaceDialogue());
                            //inventory.PlaceObject(interactedObject, inventory.currentId);

                        }
                        else
                            //if not placing object, interact
                            StartCoroutine(InteractWithObject());
                    else
                    {
                        //this if statement enables player to move while in placement mode
                        if (!StateManager.instance.placeSelect)
                            StateManager.instance.canClick = true;
                    }

                    StopMovement();
                }


            }              

        }
        
    }

    public IEnumerator SpawnPlayer()
    {
        //set player in the first position
        destination.transform.position = spawnPos;

        //get pathfinder to go to exact destination
        pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.ContinueToExactDestination;

        float dist = Vector2.Distance(player.transform.position, destination.transform.position);

        //wait until player is in position
        while (dist > 0.01)
        {
            dist = Vector2.Distance(player.transform.position, destination.transform.position);
            yield return null;
        }

        //get pathfinder to go to stop
        pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.Stop;

        if (playerSprite.transform.localScale.x < 0)
        {
            SpriteFlip();
        }
    }

    public void ReScan()
    {
        Debug.Log("scanned");
        var guo = new GraphUpdateObject(clickBounds.bounds);
        // Set some settings
        //guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
    }

    public IEnumerator ChangeStation(int direction)
    {
        //disable clicking again just to be sure
        StateManager.instance.canClick = false;

        Debug.Log("pathfinder changed");
        //get pathfinder to go to exact destination
        pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.ContinueToExactDestination;

        //false
        StateManager.instance.isMoving = true;

        //to the left
        if(direction == 0)
        {
            //if in the middle
            if(currentStation == 2)
            {
                destination.transform.position = Floor1.transform.position;
                currentStation = 1;

            } else if (currentStation == 3)
            {
                destination.transform.position = Floor2.transform.position;
                currentStation = 2;

            }

        } else if (direction == 1) //to the right
        {
            //if in the middle
            if (currentStation == 2)
            {
                destination.transform.position = Floor3.transform.position;
                currentStation = 3;

            }
            else if (currentStation == 1)
            {
                destination.transform.position = Floor2.transform.position;
                currentStation = 2;

            }

        }

        //wait until player is in position
        while (Mathf.Abs(player.transform.position.x - destination.transform.position.x) > 0.001)
        {
            yield return null;
        }

        Debug.Log("small distance");

        //stop movement
        StopMovement();

        //enable clicking
        StateManager.instance.canClick = true;

        //reset pathfinder
        pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.Stop;

    }

    public IEnumerator Sit()
    {
        //disable clicking again just to be sure
        StateManager.instance.canClick = false;

        //get pathfinder to go to exact destination
        pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.ContinueToExactDestination;

        //false
        StateManager.instance.isMoving = true;

        //go to chair's position
        destination.transform.position = interactedObject.transform.position;

        //wait until player is in position
        while(player.transform.position.x != destination.transform.position.x)
        {
            yield return null;
        }

        //stop movement
        StopMovement();

        //flip
        if (interactedObject.GetComponent<ChairTrigger>().id == 0)
        {
            StoryManager.instance.currentChair = 0;

            if(!StateManager.instance.facingRight)
            {
                SpriteFlip();
            }

        } else if (interactedObject.GetComponent<ChairTrigger>().id == 1)
        {
            StoryManager.instance.currentChair = 1;

            if (StateManager.instance.facingRight)
            {
                SpriteFlip();
            }

        }

        playerAnimator.SetBool("isSitting", true);

        yield return new WaitForSeconds(0.5f);

        //when arrived in position
        StartCoroutine(ZoomOut());

        yield return new WaitForSeconds(3);

        if (StoryManager.instance.cutscene)
            StoryManager.instance.next = true;

        //if it's the first night
        else if(TimeManager.instance.timeState == 5)
        {

            //box positions
            Vector3 playerBoxPos = new Vector3(2.5f, 12, 10);
            Vector3 otherBoxPos = new Vector3(-1.7f, 12f, 10);

            if (StoryManager.instance.currentChair == 0)
            {
                StoryManager.instance.playerBox.transform.localPosition = otherBoxPos;

            }
            else if (StoryManager.instance.currentChair == 1)
            {
                StoryManager.instance.playerBox.transform.localPosition = playerBoxPos;
            }

            DialogueManager.instance.EnqueueDialogue(StoryManager.instance.firstDayDialogues[5]);
        }
    }

    public void ToggleInventory()
    {
        Debug.Log("P was pressed");
        if (StateManager.instance.inventoryOn)
        {
            //Debug.Log("Place Objects was turned off");
            //placeObjectWarning.text = "P: PLACE OBJECTS OFF";

            StateManager.instance.inventoryOn = false;

            //this if statement enables player to move while in placement mode
            if (!StateManager.instance.placeSelect)
                StateManager.instance.canClick = true;

            inventoryCanvas.SetActive(false);
        }
        else if (!StateManager.instance.inventoryOn)
        {
            //Debug.Log("Place Objects was turned on");
            //placeObjectWarning.text = "P: PLACE OBJECTS ON";
            StateManager.instance.canClick = false;
            StateManager.instance.inventoryOn = true;
            Inventory.instance.StartInventory();
            //disable notification
            ClickManager.instance.player.transform.Find("HelpCanvas").gameObject.SetActive(false);
            inventoryCanvas.SetActive(true);
        }

    }

    void GoToObject(GameObject interactedObject)
    {
        //PREVENT CLICKING UNTIL REACHED
        StateManager.instance.canClick = false;

        // new destination is the interacted object
        destination.transform.position = interactedObject.transform.position;

        // player is now moving
        StateManager.instance.isMoving = true;

        // player is going to a specific object, NOT the ground
        StateManager.instance.goToObject = true;

        // face sprite to the correct direction
        Debug.Log("flip checked");
        CheckSpriteFlip(destination.transform);
    }

    RaycastHit2D GetMousePosition()
    {
        mousePos = Input.mousePosition;

        //convert screen coordinates to world coordinates
        mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);

        //convert mouse position in world to 2D
        mousePosWorld2D = new Vector2(mousePosWorld.x, mousePosWorld.y);

        //Raycast 2D => save hit
        RaycastHit2D returnPosition = Physics2D.Raycast(mousePosWorld2D, Vector2.zero);

        return returnPosition;

    }

    float GetDistance(){
        //define distance between player and object
        float playerObjDistance = player.transform.position.x - interactedObject.transform.position.x;

        //get the module
        if(playerObjDistance < 0)
        {
            playerObjDistance *= -1;
        }
        
        return playerObjDistance;
    }

    public void CheckSpriteFlip(Transform transform)
    {
        Debug.Log("flip checked");

        float dist = player.transform.position.x - transform.position.x;
        if (dist < 0)
            dist *= -1;

        if (dist < 0.3)
        {
            return;
        }

        if (playerSprite.transform.position.x > transform.position.x && StateManager.instance.facingRight || playerSprite.transform.position.x < transform.position.x && !StateManager.instance.facingRight)
        {
            SpriteFlip();
        }
        
    }

    public void SpriteFlip()
    {
        //flip
        StateManager.instance.facingRight = !StateManager.instance.facingRight;

        Vector3 scale = playerSprite.transform.localScale;
        //Debug.Log("scale first:"+ scale);
        scale.x *= -1;
        //Debug.Log("scale second:"+ scale);

        playerSprite.transform.localScale = scale;
    }


    public void ZoomCameraMethod(){
        StartCoroutine(ZoomCameraCoroutine());
        /*
        //zoom into character
        zoomCamera.enabled = true;
        zoomCamera.m_Follow = player.transform;
        currentCamera.enabled = false;

        */

    }

    public IEnumerator ZoomCameraCoroutine()
    {
        StateManager.instance.canClick = false;
        StateManager.instance.zoomOut = false;
        zoomOutCamera.enabled = false;
        currentCamera.enabled = false;

        //go to zoomed camera
        zoomCamera.enabled = true;

        yield return new WaitForSeconds(1.5f);

        /*
        if (!StoryManager.instance.cutscene)
            StateManager.instance.canClick = true;*/

    }

    public void ZoomCameraObject()
    {
        //zoom into character
        zoomCamera.enabled = true;
        zoomCamera.m_Follow = interactedObject.transform;
        currentCamera.enabled = false;
    }

    public IEnumerator DeZoomCamera()
    {
        zoomCamera.m_Follow = player.transform;

        //return to regular camera
        zoomCamera.enabled = false;
        currentCamera.enabled = true;

        yield return new WaitForSeconds(warningTime);

        StateManager.instance.canClick = true;

    }

    public IEnumerator DeZoomCameraMethod(){
        
        StateManager.instance.zoomOut = false;
        zoomOutCamera.enabled = false;
        zoomCamera.enabled = false;

        //return to regular camera
        currentCamera.enabled = true;

        yield return new WaitForSeconds(1.5f);

        if(!StoryManager.instance.cutscene)
            StateManager.instance.canClick = true;
    }

    public IEnumerator ZoomOut()
    {
        StateManager.instance.canClick = false;
        StateManager.instance.zoomOut = true;

        //return to regular camera
        currentCamera.enabled = false;
        zoomCamera.enabled = false;
        zoomOutCamera.enabled = true;

        yield return new WaitForSeconds(3);

        if(!StoryManager.instance.cutscene)
        {
            StateManager.instance.canClick = true;

            //get pathfinder to go to stop in near distance
            pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.Stop;
        }
            

    }

    public void StopMovement()
    {
        Debug.Log("Stopped movement");
        //go back to idle animation
        playerAnimator.SetBool("isMoving", false);

        //reset destination
        //destination.transform.position = player.transform.position;        

        StateManager.instance.isMoving = false;
        StateManager.instance.goToObject = false;

    }

    public IEnumerator InteractWithObject() {

        Debug.Log("interacted with object");

        //disable clicking
        StateManager.instance.canClick = false;

        //not going to object
        StateManager.instance.goToObject = false;

        ZoomCameraMethod();        

        //delay
        yield return new WaitForSeconds(warningTime);

        if (interactedObject.tag == "Pot")
        {
            interactedObject.GetComponent<PotTrigger>().Interact();

        }
        else if (interactedObject.tag == "Character")
        {
            StartCoroutine(interactedObject.GetComponent<CharacterInteraction>().TriggerDialogue());

        }
        else if (interactedObject.tag == "Door")
        {
            interactedObject.GetComponent<Door>().mainCanvas.SetActive(false);

            if (StoryManager.instance.birdsAppeared)
                DialogueManager.instance.EnqueueDialogue(interactedObject.GetComponent<DialogueHolder>().storyDialogues[0]);
            else
                DialogueManager.instance.EnqueueDialogue(interactedObject.GetComponent<DialogueHolder>().storyDialogues[1]);

        }
        else if (interactedObject.tag == "Chair")
        {
            interactedObject.GetComponent<ChairTrigger>().Interact();

        } else if (interactedObject.tag == "Hanger")
        {
            interactedObject.GetComponent<HangerScript>().Interact();

        } else
            interactedObject.GetComponent<CollectableTrigger>().Interact();        

        //by the end of the dialogue, DeZoomCameraMethod() will be called from the Dialogue Manager

    }

    private void OnDrawGizmos() {

        if(targetSlot)
        {
            Vector2 playerObjDistance = player.transform.position - interactedObject.transform.position;

            Gizmos.DrawWireSphere(targetSlot.transform.position, 1.5f);
        }
        
    }

    public void EnableClicks()
    {
        StateManager.instance.canClick = true;

    }

    public IEnumerator PreventFurtherClicks()
    {
        StateManager.instance.canClick = false;

        yield return new WaitForSeconds(0.3f);

        StateManager.instance.canClick = true;
    }

    //IEnumerator should used only if intended to be based on timing
    /*
    IEnumerator ZoomCamera()
    {
        //zoom into character
        newCamera.enabled = true;
        currentCamera.enabled = false;

        yield return new WaitForSeconds(warningTime);

        //return to regular camera
        newCamera.enabled = false;
        currentCamera.enabled = true;

    }
    */
}

