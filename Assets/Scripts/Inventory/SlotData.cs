using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotData
{
    //object IDs
    //order in page
    public int slotOrder;

    //type of object
    public int objType;

    //specific object
    public int objId;

    public SlotData (PageData page, int order)
    {
        slotOrder = page.slot[order];

        objType = page.pageId;

    }
}
