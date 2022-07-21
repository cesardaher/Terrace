using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class InteractableObject : MonoBehaviour
{
    public static InteractableObject currentObj { get;  private set; }
    public static DialogueRunner dialogueRunner;
    static List<InteractableObject> objectList = new List<InteractableObject>();
    public static bool canInteract = false;

    public Task currentInteraction;

    public Canvas objCanvas;

    GameObject hoveredGO;
    bool isHovered;


    protected virtual void Start()
    {
        dialogueRunner = (DialogueRunner)FindObjectOfType(typeof(DialogueRunner));
        objCanvas.gameObject.SetActive(false);

        //dialogueRunner.Add(yarnDialogue);
    }

    public void SetCurrentObj(bool set)
    {
        currentObj = set ? this : null;

        //THIS IS THE SAME AS:
        /*
        if (set)
            currentSeed = objectId;
        else
            currentSeed = 0;
        */
    }


    protected virtual void OnMouseOver()
    {
        if (StateMng.instance.CanClick && canInteract && !StateMng.instance.interacting)
            objCanvas.gameObject.SetActive(true); 
    }

    protected virtual void OnMouseExit()
    {
        if (objCanvas.gameObject.activeSelf && !StateMng.instance.interacting) 
            objCanvas.gameObject.SetActive(false);       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (StateMng.instance.CanClick)
            objCanvas.gameObject.SetActive(false);
    }

    public virtual IEnumerator InteractWithObject()
    {
        if(ClickMng.instance.currentMovement != null)
        {
            ClickMng.instance.destination.transform.position = ClickMng.instance.player.transform.position;
        }

        StateMng.instance.interacting = true; // start interaction
        StateMng.instance.CanClick = false; // disable clicking

        Debug.Log("started interaction");
        //check distance between player and object
        //if high, move to it
        /*
        if(ClickMng.instance.GetDistance() > ClickMng.instance.nearObjectDistance)
        {
            ClickMng.instance.TargetPos = currentObj.transform.position;

            while (ClickMng.instance.currentMovement.Running)
                yield return null;
        }*/

        ClickMng.instance.TargetPos = currentObj.transform.position;
        
        if(ClickMng.instance.currentMovement != null)
        {
            while (ClickMng.instance.currentMovement.Running)
                yield return null;
        }

        //make player face object
        ClickMng.instance.CheckSpriteFlip(currentObj.transform);

        yield return new WaitForSeconds(0.1f);
    }

    public void OnEnable() //to add object to list
    {
        objectList.Add(this);
    }

    private void OnDisable() //to remove object to list
    {
        objectList.Remove(this);
    }

    public static void StartDialogue(string node)
    {
        StateMng.instance.interacting = false;
        StateMng.instance.dialogueRunner.StartDialogue(node);
    }

    public static void DisableCanvas()
    {
        foreach(InteractableObject intObject in objectList)
        {
            if(intObject.objCanvas.gameObject.activeSelf)
                intObject.objCanvas.gameObject.SetActive(false);
        }

        if(currentObj is Pot)
        {
            ((Pot)currentObj).harvest.SetActive(false);
            ((Pot)currentObj).plantGroup.SetActive(false);
        } else if(currentObj is Collectible)
        {
            ((Collectible)currentObj).collectButton.SetActive(false);
        }

    }//to disable all canvases (useful for dialogues) 

    public void ToggleInteraction(bool on)
    {
        if(on)
        {
            StateMng.instance.interacting = true;
            return;
        }

        StateMng.instance.interacting = false;
    }
        
}
