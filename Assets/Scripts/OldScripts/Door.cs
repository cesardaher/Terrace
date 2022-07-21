using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject mainCanvas;

    void OnMouseOver()
    {
        Debug.Log("mouse over");

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


    // Start is called before the first frame update
    void Start()
    {
        mainCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (ClickManager.instance.interactedObject == this)
        {
            mainCanvas.SetActive(false);

        }
        
    }
}
