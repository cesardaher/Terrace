using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    //timemanager
    public int weatherState;
    public int timeState;

    //storymanager booleans
    public bool hasPlanted;
    public bool plantGrew;
    public bool hasDriedMint;
    public bool hasDriedValerian;
    public bool birdsAppeared;
    public bool firstSeeds;
    public bool metFriend;
    public bool metCat;
    public bool gotShovel;
    public bool plantedCalendula;
    public bool plantedBasil;
    public bool seenCredits;

    //seeds on ground
    public bool seed1;
    public bool seed2;

    //main plant 
    public int mainPlantId;


    public bool peppermintIsHarvestable;
    public bool peppermintHasHarvested;
    public bool peppermintIsGrown;
    public int peppermintPlantGrowth;

    public bool valerianIsHarvestable;
    public bool valerianHasHarvested;
    public bool valerianIsGrown;
    public int valerianPlantGrowth;

    public bool calendulaIsHarvestable;
    public bool calendulaHasHarvested;
    public bool calendulaIsGrown;
    public int calendulaPlantGrowth;

    public bool basilIsHarvestable;
    public bool basilHasHarvested;
    public bool basilIsGrown;
    public int basilPlantGrowth;

    //left hanger
    public bool leftFull;
    public int plantId;
    public int plantDrought = 0;
    public bool isDry;
    public int LeftTimeState;

    //right hanger
    public bool rightFull;
    public int rightPlantId;
    public int rightPlantDrought = 0;
    public bool rightIsDry;
    public int RightTimeState;
    
    //inventory
    public int[] invIds;

    //pots
    public int[] interactedPots;

    public SceneData(SceneProperties properties)
    {
        weatherState = properties.weatherState;
        timeState = properties.timeState;

        hasPlanted = properties.hasPlanted;
        plantGrew = properties.plantGrew;
        hasDriedValerian = properties.hasDriedValerian;
        hasDriedMint = properties.hasDriedMint;

        birdsAppeared = properties.birdsAppeared;
        firstSeeds = properties.firstSeeds;
        metFriend = properties.metFriend;
        metCat = properties.metCat;
        gotShovel = properties.gotShovel;
        plantedCalendula = properties.plantedCalendula;
        plantedBasil = properties.plantedBasil;
        seenCredits = properties.seenCredits;

        //seeds on ground
        seed1 = properties.seed1;
        seed2 = properties.seed2;

        //main plant 
        mainPlantId = properties.mainPlantId;

        peppermintIsHarvestable = properties.peppermintIsHarvestable;
        peppermintHasHarvested = properties.peppermintHasHarvested;
        peppermintIsGrown = properties.peppermintIsGrown;
        peppermintPlantGrowth = properties.peppermintPlantGrowth;

        valerianIsHarvestable = properties.valerianIsHarvestable;
        valerianHasHarvested = properties.valerianHasHarvested;
        valerianIsGrown = properties.valerianIsGrown;
        valerianPlantGrowth = properties.valerianPlantGrowth;

        calendulaIsHarvestable = properties.calendulaIsHarvestable;
        calendulaHasHarvested = properties.calendulaHasHarvested;
        calendulaIsGrown = properties.calendulaIsGrown;
        calendulaPlantGrowth = properties.calendulaPlantGrowth;

        basilIsHarvestable = properties.basilIsHarvestable;
        basilHasHarvested = properties.basilHasHarvested;
        basilIsGrown = properties.basilIsGrown;
        basilPlantGrowth = properties.basilPlantGrowth;

        //left hanger
        leftFull = properties.leftFull;
        plantId = properties.plantId;
         plantDrought = properties.plantDrought;
        isDry = properties.isDry;
        LeftTimeState = properties.LeftTimeState;

        //right hanger
        rightFull = properties.rightFull;
        rightPlantId = properties.rightPlantId;
        rightPlantDrought = properties.rightPlantDrought;
        rightIsDry = properties.rightIsDry;
        RightTimeState = properties.RightTimeState;

        //inventory
        invIds = properties.invIds;

        //pots
        interactedPots = properties.interactedPots;

}

}
