using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class StateMng : MonoBehaviour
{
    public static StateMng instance;

    public bool cutscene;

    [SerializeField]
    private bool canClick;
    public bool CanClick
    {
        get { return canClick; }
        set {
            canClick = value;

            if(canClick == true)
            {
                if(!inventoryButton.enabled && !isSitting)
                    inventoryButton.enabled = true;
            } else
            {
                if (inventoryButton.enabled)
                    inventoryButton.enabled = false;
            }
        }
    }
    public bool mouseMoving;
    public bool keyboardMoving;
    public bool facingRight;
    public bool isPaused;

    public bool zoomOut;
    public bool zoomIn;

    public bool inventoryOn;
    public bool inDialogue;
    public bool inOptions;
    public bool interacting;
    private bool isSitting;
    public bool IsSitting
    {
        get { return isSitting; }
        set
        {
            isSitting = value;

            if(isSitting)
            {
                DialogueClamp.mainPlayer.boxZoomPos = DialogueClamp.mainPlayer.sittingZoomInPos;
                DialogueClamp.mainPlayer.boxNormalPos = DialogueClamp.mainPlayer.normalSittingPos;
                return;
            }

            DialogueClamp.mainPlayer.boxZoomPos = DialogueClamp.mainPlayer.standingZoomInPos;
            DialogueClamp.mainPlayer.boxNormalPos = DialogueClamp.mainPlayer.normalStandingPos;
            return;

        }
    }

    public DialogueRunner dialogueRunner;
    [SerializeField]
    public Button inventoryButton;

    public GameObject pauseMenu;

    //create instance
    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Fix this!" + gameObject.name);
        else
            instance = this;
    }

    private void Start()
    {
        CanClick = true;        
    }

    void Update()
    {
        
        //disable clicking when inventory is on, or in dialogue
        if((inventoryOn || inDialogue || cutscene) && CanClick)
        { 
            CanClick = false;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateMenu();
        }
        
    }

    public void ToggleDialogue(bool on)
    {
        inDialogue = on;
    }

    public void ToggleOptions(bool on)
    {
        inOptions = on;
    }

    public void ToggleClicking(bool t)
    {
        CanClick = t;
    }

    public void ToggleCutscene(bool on)
    {
        cutscene = on;
    }

    public void EndCutscene()
    {
        if(!cutscene)
        {
            CanClick = true;
        }
    }

    public void ActivateMenu()
    {
        Time.timeScale = 0;

        //disable clicks
        CanClick = false;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;

        //enable clicks
        CanClick = true;
        pauseMenu.SetActive(false);
        isPaused = false;
    }
}
