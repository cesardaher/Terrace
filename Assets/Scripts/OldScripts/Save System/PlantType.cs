using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantType
{
    Peppermint,
    Valerian
}

public class PlantData
{
    public string id;

    public PlantType plantType;

    public bool isHarvestable;
    public bool hasHarvested;
    public bool isGrown;

    public int plantGrowth;
}
