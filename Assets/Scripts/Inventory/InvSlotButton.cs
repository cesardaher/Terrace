using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvSlotButton : MonoBehaviour
{
    //mainObject
    public ObjDescription currentObject;

    //Inventory UI objects
    public Image objectIcon;
    public Image objectImage;
    public TextMeshProUGUI objectName;
    public TextMeshProUGUI objectInfo;

    private void Start()
    {
        //GetObject();
    }

    public void GetObject()
    {
        if (currentObject != null)
        { 
            //sets button's icon and enable
            objectIcon.sprite = currentObject.objectIcon;
            GetComponent<Button>().interactable = true;

        } else
        {
            //disable button if empty
            objectIcon.sprite = null;
            GetComponent<Button>().interactable = false;
        }

    }

    public void SetObject()
    {
        Debug.Log("inventory object was pressed");

        //foolproofing
        if (currentObject != null)
        {
            //set attributes in inventory page
            objectImage.sprite = currentObject.objectImage;
            objectName.text = currentObject.objectName;
            objectInfo.text = currentObject.objectInfo;
        }
    }
}
