using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public ClickManager clickManager;

    public static DialogueManager instance;

    public DialogueBase currentDialogue;

    [Header("Dialogue Canvas")]
    //set canvas for DialogueBox
    public GameObject dialogueCanvas;    

    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;

    public Button continueButton;

    [Header("Dialogue Seconday Canvas")]
    //set canvas for DialogueBo
    public GameObject other;
    public GameObject characterCanvas;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterText;

    public Button characterContinueButton;


    [Header("Dialogue Options")]

    public GameObject dialogueOptionsUI;
    public bool isDialogueOption;
    public TextMeshProUGUI questionText;
    public GameObject[] optionButtons;
    private int optionsAmount;

    [Header("Dialogue Options 2")]

    public GameObject characterOptionsUI;
    public TextMeshProUGUI characterQuestionText;
    public GameObject[] characterOptionButtons;

    [Header("Dialogue Settings")]
    //Define text delay
    public float delay;

    //Get text from dialog
    public Queue<DialogueBase.Info> dialogueInfo;
    
    //running dialogue settings
    public bool done;

    private bool isCurrentlyTyping;
    private string completeText;

    private void Awake()
    {
		if(instance != null)
        {
			Debug.LogWarning("Fix this!" + gameObject.name);
		} 
        else 
        {
			instance = this;
		}

	}

    private void Update()
    {
        // if player is in dialogue
        if (StateManager.instance.inDialogue)
        {
            if(StateManager.instance.canClick)
            {
                StateManager.instance.canClick = false;
            }

            if(!characterOptionsUI.activeSelf && !dialogueOptionsUI.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                { //If you press Space

                    DequeueDialog();
                    return;
                }
            }          
        }

        //continue dialogue
        

        //disable clicking button if typing
        /*
        if(isCurrentlyTyping)
        {
            continueButton.enabled = false;
            characterContinueButton.enabled = false;
        }*/
    }

    void Start()
    {
        //dialogue is not done
        done = false;

        //get dialogue info into queue
		dialogueInfo = new Queue<DialogueBase.Info>();

        //turn off dialogue canvas
        dialogueCanvas.SetActive(false);
        dialogueOptionsUI.SetActive(false);

        //turn off dialogue canvas for other character
        characterCanvas.SetActive(false);
        characterOptionsUI.SetActive(false);

        //turn off dialogue names
        //remove this in the future
        characterName.gameObject.SetActive(false);
        dialogueName.gameObject.SetActive(false);


    }

    public void EnqueueDialogue(DialogueBase db)
    {
        Debug.Log("dialogue started");

        //disable clicking
        StateManager.instance.canClick = false;

        currentDialogue = db;

        //prevent dialogue from starting when already in a dialogue
        if(StateManager.instance.inDialogue) 
        {
            //Debug.Log("no dialogue");
            return;
        }

        StateManager.instance.inDialogue = true;

        Debug.Log("enqueued dialogue");

        //maybe need to change this to make adaptive to character?
        dialogueCanvas.SetActive(true);

        //turn on continue buttons
        DialogueManager.instance.continueButton.enabled = true;
        DialogueManager.instance.characterContinueButton.enabled = true;

        //clear dialogue info
        dialogueInfo.Clear();
        

        if(db is DialogueOptions)
        {
            //get info from question
            isDialogueOption = true;
            DialogueOptions dialogueOptions = db as DialogueOptions;
            optionsAmount = dialogueOptions.optionsInfo.Length;

            //appear question in appropriate character's dialogue
            if (dialogueOptions.player)
            {
                string tempText = dialogueOptions.questionText.Replace("\r", "").Replace("\n", "");
                questionText.text = tempText;

            }
            else
            {
                string tempText = dialogueOptions.questionText.Replace("\r", "").Replace("\n", "");

                characterQuestionText.text = tempText;
            }
                


            //turn off all buttons
            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionButtons[i].SetActive(false);
                characterOptionButtons[i].SetActive(false);
            }
                        
            for (int i = 0; i < optionsAmount; i++)
            {
                if(dialogueOptions.player)
                {
                    //turn on buttons
                    optionButtons[i].SetActive(true);
                    //set correct text
                    optionButtons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = dialogueOptions.optionsInfo[i].buttonName;
                    //set events
                    UnityEventHandler myEventHandler = optionButtons[i].GetComponent<UnityEventHandler>();

                    if (myEventHandler == null) Debug.Log("MY EVENT IS NULL");
                    if (myEventHandler.eventHandler == null) Debug.Log("CASE TWO");
                    myEventHandler.eventHandler = dialogueOptions.optionsInfo[i].myEvent;

                    //if there is a next dialogue, set it
                    if (dialogueOptions.optionsInfo[i].nextDialogue != null)
                    {
                        //send it to event handler
                        myEventHandler.myDialogue = dialogueOptions.optionsInfo[i].nextDialogue;
                    }
                    else
                    {
                        //otherwise set it as null
                        myEventHandler.myDialogue = null;
                    }

                } else
                {
                    Debug.Log("is not player");
                    //turn on buttons
                    characterOptionButtons[i].SetActive(true);
                    //set correct text
                    characterOptionButtons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = dialogueOptions.optionsInfo[i].buttonName;
                    //set events
                    UnityEventHandler myEventHandler = characterOptionButtons[i].GetComponent<UnityEventHandler>();

                    if (myEventHandler == null) Debug.Log("MY EVENT IS NULL");
                    if (myEventHandler.eventHandler == null) Debug.Log("CASE TWO");
                    myEventHandler.eventHandler = dialogueOptions.optionsInfo[i].myEvent;

                    //if there is a next dialogue, set it
                    if (dialogueOptions.optionsInfo[i].nextDialogue != null)
                    {
                        //send it to event handler
                        myEventHandler.myDialogue = dialogueOptions.optionsInfo[i].nextDialogue;
                    }
                    else
                    {
                        //otherwise set it as null
                        myEventHandler.myDialogue = null;
                    }
                }           
            }

            
        } else
        {

            isDialogueOption = false;

        }        

        //enqueue dialogue texts        
		foreach(DialogueBase.Info info in db.dialogueInfo){
			dialogueInfo.Enqueue(info);
		}
        
        //call dequeue 
        //actually make text appear
		DequeueDialog();
	}

    public void DequeueDialog()
    {

        if (isCurrentlyTyping)
        {
			//CompleteText();
			//StopAllCoroutines();
			//isCurrentlyTyping = false;

            //stop rest of function
            return;
		}
        
        //if there is no dialogue, just finish
		if(dialogueInfo.Count == 0)
        {
            OptionsLogic(currentDialogue);

            return;
		}

        DialogueBase.Info info = dialogueInfo.Dequeue();

        if (info.player)
        {
            Debug.Log("is player 2");
            characterCanvas.SetActive(false);
            dialogueCanvas.SetActive(true);

            completeText = info.text;

            //dialogueName.text = info.name;
            //dialogueText.text = info.text;

            dialogueText.text = "";
            StartCoroutine(TypeText(info));

        } else if(!info.player)
        {
            Debug.Log("is not player 2");
            dialogueCanvas.SetActive(false);
            characterCanvas.SetActive(true);

            completeText = info.text;

            //characterName.text = info.name;
            //characterText.text = info.text;

            characterText.text = "";
            StartCoroutine(TypeText(info));

        } else
        {
            Debug.Log("is player 3");
            characterCanvas.SetActive(false);
            dialogueCanvas.SetActive(true);

            completeText = info.text;

            //dialogueName.text = info.name;
            //dialogueText.text = info.text;

            dialogueText.text = "";
            StartCoroutine(TypeText(info));

        }

	}

	//Each character appears
	IEnumerator TypeText(DialogueBase.Info info)
    {

		isCurrentlyTyping = true;

        string tempText = info.text.Replace("\r", "").Replace("\n", "");

        foreach (char c in tempText.ToCharArray())
		{
			yield return new WaitForSeconds(delay);

            if (info.player)
                dialogueText.text += c;
            else if (!info.player)
                characterText.text += c;
            else
                dialogueText.text += c;
        }

		isCurrentlyTyping = false;

	}

    private void CompleteText()
    {
        string tempText = completeText.Replace("\r", "").Replace("\n", "");
        dialogueText.text = tempText;
	}

    public void EndDialog()
    {
		
        dialogueCanvas.SetActive(false);
	}

    private void OptionsLogic(DialogueBase currentDialogue)
    {
        if (currentDialogue is DialogueOptions)
        {
            //turn current dialogue into options
            DialogueOptions currentDialogueOptions = currentDialogue as DialogueOptions;

            if (currentDialogueOptions.player)
            {
                //turn off other character's canvas
                characterCanvas.SetActive(false);
                characterOptionsUI.SetActive(false);
                Debug.Log("is dialogue option: player");

                //turn on player canvas
                dialogueCanvas.SetActive(true);
                dialogueOptionsUI.SetActive(true);
            }
            else
            {
                //turn off player canvas
                dialogueCanvas.SetActive(false);
                dialogueOptionsUI.SetActive(false);

                //turn on other character's canvas
                characterCanvas.SetActive(true);
                characterOptionsUI.SetActive(true);
            }


            //turn off all buttons
            continueButton.enabled = false;
            characterContinueButton.enabled = false;

        } else
        {
            //turn off all dialogue boxes
            dialogueCanvas.SetActive(false);
            characterCanvas.SetActive(false);
            
            if(!StoryManager.instance.cutscene)
                ClickManager.instance.StartCoroutine(ClickManager.instance.DeZoomCameraMethod());

            done = true;

            StateManager.instance.inDialogue = false;

            StateManager.instance.placeSelect = false;

            //removed because it conflicts with dezoom
            //StateManager.instance.canClick = true;
        }
    }

    public void CloseOptions()
    {
        //close the correct person's box
        if(dialogueOptionsUI.activeSelf == true)
        {
            dialogueCanvas.SetActive(false);
            dialogueOptionsUI.SetActive(false);
        }
        else if (characterOptionsUI.activeSelf == true)
        {
            characterCanvas.SetActive(false);
            characterOptionsUI.SetActive(false);

        }
            
    }

    public void CloseDialogue()
    {
        dialogueCanvas.SetActive(false);
    }

}
