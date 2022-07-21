using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    [Header("General")]
    public bool canClick = true;
    public bool zoomOut = false;
    public bool isPaused = false;

    [Header("Movement")]
    public bool keyboardMoving = false;
    public bool isMoving = false;
    public bool facingRight = true;
    public bool goToObject = false;

    [Header("Inventory")]
    public bool inventoryOn = false;
    //maybe remove?
    public bool hasCollected = false;
    public bool placeSelect = false;
    public bool moveSelect = false;

    [Header("Dialogue")]
    public DialogueRunner dialogRunner;
    public bool inDialogue = false;

    public GameObject PauseMenu;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Fix this!" + gameObject.name);
        }
        else
        {
            instance = this;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused && !PauseMenu.activeSelf)
        {
            ActivateMenu();
        }

        else if(!isPaused && PauseMenu.activeSelf)
        {
            DeactivateMenu();
        }
    }

    public void PlaceSelectOn()
    {
        inventoryOn = true;
        placeSelect = true;

    }

    public void MoveSelectOn()
    {
        inventoryOn = true;
        placeSelect = true;
        moveSelect = true;

        Inventory.instance.moveableObject = ClickManager.instance.interactedObject;
    }

    public void ActivateMenu()
    {
        Time.timeScale = 0;

        //disable clicks
        canClick = false;
        PauseMenu.SetActive(true);
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;

        //enable clicks
        canClick = true;
        PauseMenu.SetActive(false);
        isPaused = false;
    }
}
