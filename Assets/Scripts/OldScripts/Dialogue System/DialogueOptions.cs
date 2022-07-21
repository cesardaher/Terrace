using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialogue Option", menuName = "Options")]
public class DialogueOptions : DialogueBase
{
    //QUESTION DESCRIPTION
    [TextArea(2,5)]
    public string questionText;

    //QUESTION OPTIONS
    [System.Serializable]
    public class Options
    {
        public string buttonName;
        public string comment;
        public DialogueBase nextDialogue;
        public UnityEvent myEvent;

    }

    public bool player;
    public Options[] optionsInfo;

}
