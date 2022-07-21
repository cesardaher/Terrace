using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueBase : ScriptableObject {

	[System.Serializable]
	public class Info 
    {
		public bool player;
	    public string name;

	    [TextArea(4, 8)]
	    public string text;

	}

	[Header ("Insert Dialog Info below")]

	public Info[] dialogueInfo;

}