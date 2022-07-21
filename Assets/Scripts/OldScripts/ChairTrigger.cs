using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairTrigger : MonoBehaviour
{
    public int id;
    [Header("Canvas")]
    public GameObject mainCanvas;
    public GameObject sitButton;

    // Start is called before the first frame update
    void Start()
    {
        //turn off interaction canvas
        mainCanvas.SetActive(false);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("collided");
        
    }

    /*

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

    */

    public void ShowCanvas()
    {
        mainCanvas.SetActive(true);
    }
    public void CloseCanvas()
    {
        mainCanvas.SetActive(false);
    }

    public void Interact()
    {
        //turn on interaction canvas
        mainCanvas.SetActive(true);

        //turn on collect button
        sitButton.SetActive(true);

    }
}
