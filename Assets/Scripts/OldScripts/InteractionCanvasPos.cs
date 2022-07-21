using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCanvasPos : MonoBehaviour
{
    public GameObject parent;
    public GameObject child;

    [Header("Canvas objects")]
    public GameObject mainGroup;
    public GameObject plantGroup;
    public GameObject bottomGroup;
    public GameObject mainButton;


    [Header("Sprite Positions")]
    public float topPos;
    public float botPos;

    // Start is called before the first frame update
    void Awake()
    {
        parent = transform.parent.gameObject;

        if (transform.parent.childCount > 2)
            child = parent.transform.GetChild(2).gameObject;


    }

    // Update is called once per frame
    void OnEnable()
    {

        if (parent.gameObject.tag == "Pot")
        {

            if (transform.parent.childCount == 2) //empty pot
            {
                topPos = parent.GetComponent<SpriteRenderer>().bounds.center.y + parent.GetComponent<SpriteRenderer>().bounds.extents.y;

                mainGroup.transform.position = new Vector3(mainGroup.transform.position.x, topPos + 0.5f, mainGroup.transform.position.z);
                plantGroup.transform.position = new Vector3(mainGroup.transform.position.x, topPos + 0.5f, mainGroup.transform.position.z);

            }            
            else
            {
                if (child == null)
                    child = parent.transform.GetChild(2).gameObject;

                topPos = child.GetComponent<SpriteRenderer>().bounds.center.y + child.GetComponent<SpriteRenderer>().bounds.extents.y;

                mainGroup.transform.position = new Vector3(mainGroup.transform.position.x, topPos + 0.5f, mainGroup.transform.position.z);
            }

            botPos = parent.GetComponent<SpriteRenderer>().bounds.center.y - parent.GetComponent<SpriteRenderer>().bounds.extents.y;
            bottomGroup.transform.position = new Vector3(bottomGroup.transform.position.x, botPos - 0.5f, bottomGroup.transform.position.z);

        } else
        {
            topPos = parent.GetComponent<SpriteRenderer>().bounds.center.y + parent.GetComponent<SpriteRenderer>().bounds.extents.y;
            mainGroup.transform.position = new Vector3(mainGroup.transform.position.x, topPos + 0.5f, mainGroup.transform.position.z);
        }    

    }
}
