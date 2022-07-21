using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Description", menuName = "Description")]
public class ObjDescription : ScriptableObject
{
    public int objType;
    public int objectId;
    public Sprite objectIcon;
    public Sprite objectImage;

	public string objectName;

	[TextArea(4, 8)]
	public string objectInfo;
}
