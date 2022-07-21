using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{    
    public DialogueBase[] dialogues;
    public ClampBox clamp;
    public bool facingRight;

    public GameObject mainCanvas;

    public Vector3 boxZoomPos = new Vector3(0, 4.5f, 10);

    void OnMouseOver()
    {
        if (StateManager.instance.canClick)
            //turn on interaction canvas
            mainCanvas.SetActive(true);
    }

    void OnMouseExit()
    {
        if (StateManager.instance.canClick)
            //turn off interaction canvas
            mainCanvas.SetActive(false);
    }

    public IEnumerator TriggerDialogue()
    {
        StoryManager.instance.cutscene = true;

        //if player is to the right
        if(ClickManager.instance.player.transform.position.x > transform.position.x)
        {
            if(!facingRight)
            {
                FlipSprite();
            }

        } else
        {
            if (facingRight)
            {
                FlipSprite();
            }

        }
        //clamp.transform.position = boxZoomPos;
        DialogueManager.instance.EnqueueDialogue(dialogues[0]);
        mainCanvas.SetActive(false);

        while (StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }

        //end it
        StoryManager.instance.cutscene = false;
        StateManager.instance.canClick = true;
        ClickManager.instance.StartCoroutine(ClickManager.instance.DeZoomCameraMethod());

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x < 0)
        {
            if(!facingRight)
                facingRight = true;

        } else
        {
            if (facingRight)
                facingRight = false;
        }
        
    }

    void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        //Debug.Log("scale first:"+ scale);
        scale.x *= -1;
        //Debug.Log("scale second:"+ scale);

        transform.localScale = scale;
    }
}
