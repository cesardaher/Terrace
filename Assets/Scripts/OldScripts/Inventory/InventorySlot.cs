using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int slotId;
    public Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
    }

    public void DropItem(){
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    

    void Update()
    {
    
    }
}
