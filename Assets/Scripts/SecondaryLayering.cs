using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryLayering : MonoBehaviour
{
    public Canvas myCanvas;
    public SpriteRenderer parentCanvas;


    // Start is called before the first frame update
    void Start()
    {
        myCanvas = GetComponent<Canvas>();
        parentCanvas = transform.parent.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if(parentCanvas.sortingLayerName != myCanvas.sortingLayerName)
        {
            myCanvas.sortingLayerName = parentCanvas.sortingLayerName;
        }
    }
}
