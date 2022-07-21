using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueList : MonoBehaviour
{
    public static DialogueList instance;
    public DialogueRunner dialogueRunner;

    public YarnProgram introDialogue;
    public YarnProgram potDialogue;
    public YarnProgram doorDialogue;
    public YarnProgram hangerDialogue;
    public YarnProgram catDialogue;
    public YarnProgram friendDialogue;
    public YarnProgram chairDialogue;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Fix this!" + gameObject.name);
        }
        else
            instance = this;

        dialogueRunner.Add(introDialogue);
        dialogueRunner.Add(potDialogue);
        dialogueRunner.Add(doorDialogue);
        dialogueRunner.Add(hangerDialogue);
        dialogueRunner.Add(catDialogue);
        dialogueRunner.Add(friendDialogue);
        dialogueRunner.Add(chairDialogue);
    }

}
