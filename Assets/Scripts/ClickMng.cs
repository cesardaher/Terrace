using System.Collections;
using UnityEngine;
using Cinemachine;
using TMPro;
using Pathfinding;
using Yarn.Unity;
using UnityEngine.UI;

public class ClickMng : MonoBehaviour
{
    public static ClickMng instance;

    //positions
    Vector2 starterPos = new Vector2(-5f, -5.2f);
    Vector2 whiteChairPos = new Vector2(-8.5f, -4.5f);
    Vector2 brownChairPos = new Vector2(-12.8f, -4.6f);

    //mousehit
    bool isHovering;
    Collider2D hoveredObj;
    RaycastHit2D hit;
    Vector2 movement;
    Vector3 mousePos;
    Vector3 mousePosWorld;
    Vector2 mousePosWorld2D;
    [SerializeField]Collider2D clickBounds;
    private Vector2 targetPos;
    public Vector2 TargetPos
    {
        get
        {
            return targetPos;
        }
        set
        {
            targetPos = value;

            if (currentMovement != null)
            {
                currentMovement.Stop(); // stop current movement
            }

            if (!pathfinder.enabled) //enable pathfinder if it's false
                pathfinder.enabled = true;
            
            //move only if far away
            float distance = player.transform.position.x - targetPos.x;

            if (Mathf.Abs(distance) >= nearObjectDistance)
                currentMovement = new Task(MoveCharacter(targetPos));
        }
    }
    public Vector2 TargetPosExact
    {
        get
        {
            return targetPos;
        }
        set
        {
            targetPos = value;

            if(currentMovement != null) currentMovement.Stop(); // stop current movement

            if (!pathfinder.enabled) //enable pathfinder if it's false
                pathfinder.enabled = true;

            currentMovement = new Task(MoveCharacterExact(targetPos)); // move to around position
        }
    }

    public Task currentMovement;

    [Header("Camera")]
    public GameObject normalVirtualCamera;
    public GameObject zoomVirtualCamera;
    public GameObject zoomOutVirtualCamera;
    public VirtualCameraController normalCamera;
    public VirtualCameraController zoomCamera;
    public VirtualCameraController zoomOutCamera;
    //public bool zoomActive = false;

    [Header("Player Settings")]
    public GameObject player;
    public GameObject playerSprite;
    public Animator playerAnimator;

    [Header("Pathfinding")]
    public GameObject destination;
    public AIPath pathfinder;


    [Header("Obstacles")]
    public Collider2D upCollider;
    public Collider2D downCollider;
    public Collider2D leftCollider;
    public Collider2D rightCollider;

    [Header("Dialogue")]
    public DialogueUI dialogueUI;
    public Button dialogueButton;

    [Header("Interactions")]
    public float nearObjectDistance = 2f; // this is alligned to pathfinder on Start()
    public GameObject interactedObject;

    //create static instance
    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Fix this!" + gameObject.name);
        else
            instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        nearObjectDistance = pathfinder.endReachedDistance; // allign nearobjectdistance to pathfinder            
    }

    void Update()
    {
        // turn off or on player's collider based on keyboard movement
        BypassObstacles();

        // toggle player animation with keyboard movement
        if(currentMovement != null && !currentMovement.Running && !StateMng.instance.inDialogue)
        {
            PlayerMovementAnimation();
        }

        hit = GetMousePosition(); // get mouse position

        if (StateMng.instance.CanClick && !StoryMng.instance.moveTutorial.activeSelf)
        {
            //click
            if (Input.GetMouseButtonDown(0) && hit.collider != null)
            {
                GetClick(hit);
                return;

            } //mouse click

            if (hit.collider != null) //if was not clicked i.e. hovered
            {
                //Debug.Log("got hit");
                HoverOverObject(hit);
                return;

            } // mouse hit, no click

        } // if canClick

        if (StateMng.instance.inDialogue)
        {
            //click
            if (Input.GetMouseButtonDown(0)) //click
            {
                // continue dialogue if not in options
                if (!StateMng.instance.inOptions)
                {
                    dialogueUI.MarkLineComplete();
                }
            }

        } // inDialogue

        
    }

    private void HoverOverObject(RaycastHit2D hit)
    {
        //assign hovered object
        Collider2D tempObj = hit.collider;

        if (tempObj.GetComponent<InteractableObject>() != null || tempObj.GetComponent<PlantObject>() != null || tempObj.GetComponent<Towel>()) //if object is interactable
        {
            if (hoveredObj == null) // if wasn't hovering any objects
            {
                hoveredObj = tempObj;
                isHovering = true;
                hoveredObj.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
            }
            else if (hoveredObj != tempObj) // if was hovering another object
            {
                // first turn off first object
                hoveredObj.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);

                //now turn on new object
                hoveredObj = tempObj;
                hoveredObj.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);

            }
            return;
        }
        
        if (isHovering && !StateMng.instance.interacting) // if was hovering before
        {
            if (hoveredObj != null)
            {
                hoveredObj.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
                isHovering = false;
                hoveredObj = null;
            }
        }
        
    }

    void GetClick(RaycastHit2D hit)
    {
        Debug.Log("hit obstacle: " + hit.collider.gameObject);

        //assign interacted object
        GameObject tempObj = hit.collider.gameObject;

        //set a new destination if it's ground and stop
        if (tempObj.tag == "Ground")
        {
            // prevent buggy buggy movement when changing movement position while already walking
            if(currentMovement != null  && currentMovement.Running)
            {
                destination.transform.position = player.transform.position;
            }

            // set destination to mouse click
            TargetPos = hit.point;
            return;
        }

        // EXCEPTION: diary can be collected when all others are non-interactable
        if (!InteractableObject.canInteract) // BLOCKS THE REST OF THE CODE
        {
            if (tempObj.tag == "Diary")
            {
                tempObj.GetComponent<InteractableObject>().SetCurrentObj(true);
                InteractableObject.currentObj.currentInteraction = new Task(((Diary)InteractableObject.currentObj).InteractWithObject());
            }

            return;
        }   

        if (tempObj.tag == "Plant" || tempObj.tag == "Dryable") // if clicked on the plant, get the pot or clothes line
        {
            tempObj = tempObj.transform.parent.gameObject;
        }

        // interact with objects
        if (tempObj.GetComponent<InteractableObject>() != null) 
        {
            tempObj.GetComponent<InteractableObject>().SetCurrentObj(true);

            switch (tempObj.tag)
            {
                case "Pot":

                    if(InteractableObject.currentObj is FlippedPot) //if it's a flipped pot
                    {
                        InteractableObject.currentObj.currentInteraction = new Task(((FlippedPot)InteractableObject.currentObj).InteractWithObject());
                    } else //if it's a normal pot
                        InteractableObject.currentObj.currentInteraction = new Task(((Pot)InteractableObject.currentObj).InteractWithObject());
                    break;

                case "Chair":
                    InteractableObject.currentObj.currentInteraction = new Task(((Chair)InteractableObject.currentObj).InteractWithObject());
                    break;

                case "Door":
                    if(StoryMng.instance.hasPlanted)
                        InteractableObject.currentObj.currentInteraction = new Task(((DoorScript)InteractableObject.currentObj).InteractWithObject());
                        break;

                case "Collectible":
                    InteractableObject.currentObj.currentInteraction = new Task(((Collectible)InteractableObject.currentObj).InteractWithObject());
                    break;

                case "Hanger":
                    InteractableObject.currentObj.currentInteraction = new Task(((Hanger)InteractableObject.currentObj).InteractWithObject());
                    break;
                case "Cat":
                    InteractableObject.currentObj.currentInteraction = new Task(((Cat)InteractableObject.currentObj).InteractWithObject());
                    break;
                case "Candle":
                    InteractableObject.currentObj.currentInteraction = new Task(((Candle)InteractableObject.currentObj).InteractWithObject());
                    break;
            }
        }

        return;
    }

    void PlayerMovementAnimation()
    {
        //turn on moving animation if moving with keyboard
        if (StateMng.instance.keyboardMoving && playerAnimator.GetBool("isMoving") == false)
        {
            playerAnimator.SetBool("isMoving", true);
            return;
        }

        if (!StateMng.instance.keyboardMoving && playerAnimator.GetBool("isMoving") == true)
        {
            Debug.Log("Stopped moving");
            playerAnimator.SetBool("isMoving", false);

            //reenable pathfinder whenever stops moving
            pathfinder.enabled = true;
        }
    }

    private void BypassObstacles()
    {
        // if moving with mouse, bypass obstacles
        if (StateMng.instance.mouseMoving)
        {
            if (!player.gameObject.GetComponent<Collider2D>().isTrigger)
            {
                player.gameObject.GetComponent<Collider2D>().isTrigger = true;
            }

            return;
        }

        // if moving with keyboard or not moving , don't bypass obstacles
        if (player.gameObject.GetComponent<Collider2D>().isTrigger)
        {
            player.gameObject.GetComponent<Collider2D>().isTrigger = false;
        }

    }

    public IEnumerator ZoomIn()
    {
        StateMng.instance.CanClick = false;
        StateMng.instance.zoomOut = false;

        zoomOutCamera.Enabled = false;
        normalCamera.Enabled = false;

        //go to zoomed camera
        zoomCamera.Enabled = true;

        yield return new WaitForSeconds(1.5f);

        /*
        if (!StoryManager.instance.cutscene)
            StateManager.instance.canClick = true;*/

    }

    public IEnumerator ZoomIn(System.Action onComplete)
    {
        StateMng.instance.CanClick = false;
        StateMng.instance.zoomOut = false;

        zoomOutCamera.Enabled = false;
        normalCamera.Enabled = false;

        //go to zoomed camera
        zoomCamera.Enabled = true;

        yield return new WaitForSeconds(1.5f);

        onComplete();

    }

    public IEnumerator ZoomOut()
    {
        StateMng.instance.CanClick = false;
        StateMng.instance.zoomOut = true;

        zoomCamera.Enabled = false;
        normalCamera.Enabled = false;

        //go to zoomed out camera
        zoomOutCamera.Enabled = true;

        yield return new WaitForSeconds(1.5f);

        /*
        if (!StoryManager.instance.cutscene)
            StateManager.instance.canClick = true;*/

    }

    public IEnumerator ZoomOut(System.Action onComplete)
    {
        StateMng.instance.CanClick = false;
        StateMng.instance.zoomOut = true;

        zoomCamera.Enabled = false;
        normalCamera.Enabled = false;

        //go to zoomed out camera
        zoomOutCamera.Enabled = true;

        yield return new WaitForSeconds(1.5f);

        onComplete();

    }

    public IEnumerator NormalZoom()
    {
        StateMng.instance.CanClick = false;
        StateMng.instance.zoomOut = false;

        zoomCamera.Enabled = false;
        zoomOutCamera.Enabled = false;

        //go to zoomed out camera
        normalCamera.Enabled = true;

        yield return new WaitForSeconds(1.5f);

    }
    public IEnumerator NormalZoom(System.Action onComplete)
    {
        StateMng.instance.CanClick = false;
        StateMng.instance.zoomOut = false;

        zoomCamera.Enabled = false;
        zoomOutCamera.Enabled = false;

        //go to zoomed out camera
        normalCamera.Enabled = true;

        yield return new WaitForSeconds(1.5f);

        onComplete();

    }

    public void ReScan()
    {
        Debug.Log("scanned");
        GraphUpdateObject guo;
        if (clickBounds.bounds != null)
        {
            guo = new GraphUpdateObject(clickBounds.bounds);
            // Set some settings
            //guo.updatePhysics = true;
            AstarPath.active.UpdateGraphs(guo);
        }
            
    }

    //move character to starter position
    public IEnumerator SpawnPlayer()
    {
        // make player stand up
        if (playerAnimator.GetBool("isSitting")) playerAnimator.SetBool("isSitting", false);

        // move player to starter position
        TargetPosExact = starterPos;

        //wait until movement coroutine stopped
        while(StateMng.instance.mouseMoving)
        {
            yield return null;
        }

        if (playerSprite.transform.localScale.x < 0)
        {
            SpriteFlip();
        }
    }

    public IEnumerator MoveCharacter(Vector2 pos)
    {
        if(StateMng.instance.IsSitting)
        {
            Task t = new Task(GetUp());

            while (t.Running)
            {
                yield return null;
            }
        }
        
        pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.Stop;

        StateMng.instance.mouseMoving = true;
        destination.transform.position = pos;

        //flip sprite if needed
        CheckSpriteFlip(destination.transform);

        //start animation
        playerAnimator.SetBool("isMoving", true);

        //update distance between player and object in the X-Axis
        float playerObjDistanceX = Mathf.Abs(player.transform.position.x - destination.transform.position.x);

        //check for distance until in the correct position
        while(playerObjDistanceX >= nearObjectDistance)
        {
            playerObjDistanceX = Mathf.Abs(player.transform.position.x - destination.transform.position.x);
            yield return null;
        }

        Debug.Log("stopped movement");
        //stop movement
        playerAnimator.SetBool("isMoving", false);
        StateMng.instance.mouseMoving = false;

    }

    public IEnumerator MoveCharacterExact(Vector2 pos)
    {
        pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.ContinueToExactDestination;

        StateMng.instance.mouseMoving = true;
        destination.transform.position = pos;

        // flip sprite if needed
        CheckSpriteFlip(destination.transform);

        // start animation
        playerAnimator.SetBool("isMoving", true);

        // update distance between player and object in the X-Axis
        float playerObjDistanceX = Mathf.Abs(player.transform.position.x - destination.transform.position.x);

        //check for distance until in the correct position
        while (playerObjDistanceX >= 0.1)
        {
            playerObjDistanceX = Mathf.Abs(player.transform.position.x - destination.transform.position.x);
            yield return null;
        }

        // stop movement
        playerAnimator.SetBool("isMoving", false);
        StateMng.instance.mouseMoving = false;

    }

    public IEnumerator MoveCharacterExact(Vector2 pos, System.Action onComplete)
    {
        pathfinder.whenCloseToDestination = Pathfinding.CloseToDestinationMode.ContinueToExactDestination;

        StateMng.instance.mouseMoving = true;
        destination.transform.position = pos;

        //flip sprite if needed
        CheckSpriteFlip(destination.transform);

        // start animation
        playerAnimator.SetBool("isMoving", true);

        //update distance between player and object in the X-Axis
        float playerObjDistanceX = Mathf.Abs(player.transform.position.x - destination.transform.position.x);

        //check for distance until in the correct position
        while (playerObjDistanceX >= 0.01)
        {
            playerObjDistanceX = Mathf.Abs(player.transform.position.x - destination.transform.position.x);
            yield return null;
        }

        StateMng.instance.mouseMoving = false;

        onComplete();

        // stop movement
        playerAnimator.SetBool("isMoving", false);
        StateMng.instance.mouseMoving = false;

    } //version for dialogues

    public void CallSitChair(bool white)
    {
        StartCoroutine("SitScene", white);
    }

    public IEnumerator SitChair(bool white)
    {
        //disable collider to enable sitting
        //set up correct position
        upCollider.enabled = false;
        ReScan();        

        if (white)
            TargetPosExact = whiteChairPos;
        else
            TargetPosExact = brownChairPos;

        //wait until character stopped moving
        while(currentMovement.Running)
        {
            yield return null;
        }

        StopMovement();

        //flip sprite based on chair
        if(white)
        {
            if (StateMng.instance.facingRight)
            {
                SpriteFlip();
            }
        } else
        {
            if (!StateMng.instance.facingRight)
            {
                SpriteFlip();
            }
        }

        playerAnimator.SetBool("isSitting", true);
    }

    public IEnumerator SitScene(bool white)
    {
        StateMng.instance.IsSitting = true;

        Task t = new Task(SitChair(white));

        while (t.Running)
        {
            yield return null;
        }

        t = new Task(ZoomOut()); //zoom out
        
        //fade inventory button
        InventoryMng.instance.inventoryButton.enabled = false;
        InventoryMng.instance.CallFadeInventoryButton(false);

        while (t.Running)
        {
            yield return null;
        }

        //if it's at night, dialogue
        if(TimeMng.instance.weatherState == 2)
        {
            StoryMng.instance.NightDialogue();
            yield break;
        }

        StateMng.instance.CanClick = true;

    }

    public IEnumerator GetUp()
    {
        StateMng.instance.IsSitting = false;

        Task t = new Task(NormalZoom()); //zoom out
        InventoryMng.instance.CallFadeInventoryButton(true); //make diary button appear
        InventoryMng.instance.inventoryButton.enabled = true; //turn on button

        while (t.Running)
        {
            yield return null;
        }

        playerAnimator.SetBool("isSitting", false);

        yield return new WaitForSeconds(0.5f);
        
        upCollider.enabled = true; //reenable up collider
        ReScan();

        StateMng.instance.CanClick = true; //this is needed because of zoom out
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

    //this function checks if sprite needs to be flipped based on the target destination
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

        if (playerSprite.transform.position.x > transform.position.x && StateMng.instance.facingRight || playerSprite.transform.position.x < transform.position.x && !StateMng.instance.facingRight)
        {
            SpriteFlip();
        }

    }

    public void SpriteFlip()
    {
        //flip
        StateMng.instance.facingRight = !StateMng.instance.facingRight;

        Vector3 scale = playerSprite.transform.localScale;
        //Debug.Log("scale first:"+ scale);
        scale.x *= -1;
        //Debug.Log("scale second:"+ scale);

        playerSprite.transform.localScale = scale;
    }

    public float GetDistance()
    {
        //define distance between player and object
        float playerObjDistance = Vector2.Distance(player.transform.position, InteractableObject.currentObj.transform.position);
        //float playerObjDistance = player.transform.position.x - InteractableObject.currentObj.transform.position.x;
        Debug.Log("GetDistance: " + playerObjDistance);

        return Mathf.Abs(playerObjDistance);
    }


    //might be useless
    public void StopMovement()
    {
        //change state
        StateMng.instance.mouseMoving = false;

        //stop animation
        playerAnimator.SetBool("isMoving", false);

    }
}
