using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    public static References instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public ClickManager clickManager;
    public Inventory inventory;
    public GameObject dialoguePanel;

}
