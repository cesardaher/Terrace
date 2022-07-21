using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSlot : MonoBehaviour
{   
    public ClickManager clickManager;
    public Inventory inventory;
    public Renderer spriteRenderer;

    public bool hasChild;
    public bool emptyInventory;

    // Start is called before the first frame update
    void Start()
    {
        //get component's sprite renderer and turn it off
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
              
    }

    public void EnableRenderer(){
        //enable renderer CALLED FROM GAME MANAGER
        spriteRenderer.enabled = true;
        spriteRenderer.enabled = true;
    }

    public void PlaceObject()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {
        //check if has child
        //check if it's false, so it's not constantly changing the value
        if(!hasChild && transform.childCount > 1)
        {
            hasChild = true;
        } else if (hasChild && transform.childCount == 1)
        {
            hasChild = false;
        }

        //turn off sprite renderer if has child
        if(hasChild && spriteRenderer.enabled){
            spriteRenderer.enabled = false;
        }

        //if in place mode and is empty child, turn visibility on        
        if(StateManager.instance.placeSelect && !hasChild)
        {
            if(!spriteRenderer.enabled){
                Debug.Log("turned on");
                spriteRenderer.enabled = true;
            }

        }

        //if place mode turned off, disable visibility
        if(!StateManager.instance.placeSelect && spriteRenderer.enabled)
        {
            spriteRenderer.enabled = false;     
        }

    }
}
