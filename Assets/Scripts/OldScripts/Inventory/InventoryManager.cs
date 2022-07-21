using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject dice1;
    public GameObject dice2;
    public GameObject dice3;

    public int counter = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (counter == 0)
        {
            dice1.gameObject.SetActive(false);

        } else if (counter == 1)
        {
            dice1.gameObject.SetActive(true);
            dice2.gameObject.SetActive(false);

        } else if (counter == 2)
        {
            dice2.gameObject.SetActive(true);
            dice3.gameObject.SetActive(false);

        } else if (counter == 3)
        {
            dice3.gameObject.SetActive(true);

        }
        
    }
}
