using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Seed Description", menuName = "Seed Description")]
public class SeedDescription : ObjDescription
{
    public static int currentSeed;
    public GameObject plant;

    /*
    public void SetCurrentSeed(bool set)
    {
        currentSeed = set ? objectId : 0;

        
    }
    */
}
