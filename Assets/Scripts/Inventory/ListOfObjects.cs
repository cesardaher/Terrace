using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "List of Objects", menuName = "List")]
public class ListOfObjects : ScriptableObject
{
    public ObjDescription[] seeds;

    public ObjDescription[] plants;

    public ObjDescription[] tools;

    public GameObject[] plantObjects;

    public GameObject[] dryablePlants;
}
