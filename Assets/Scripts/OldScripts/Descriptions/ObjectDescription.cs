using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDescription : ScriptableObject
{
	//OLD, OBSOLETE
	//[System.Serializable]
		public bool placeable;

		public Sprite objectIcon;

		public string objectName;

		[TextArea(4, 8)]
		public string objectDescription;

}
