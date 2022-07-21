using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangerCanvasPos : MonoBehaviour
{

    public GameObject parent;
    public GameObject child;

    [Header("Canvas objects")]
    public GameObject bottomGroup;

    [Header("Sprite Positions")]
    public float botPos;

    // Start is called before the first frame update
    void Awake()
    {
        parent = transform.parent.gameObject;

        if (transform.parent.childCount == 2)
            child = parent.transform.GetChild(1).gameObject;

    }

    // Update is called once per frame
    void OnEnable()
    {
        if (parent.GetComponent<HangerScript>().full)
        {
            if(child == null)
            {
                child = parent.transform.GetChild(1).gameObject;
            }

            botPos = child.GetComponent<SpriteRenderer>().bounds.center.y - child.GetComponent<SpriteRenderer>().bounds.extents.y;
            bottomGroup.transform.position = new Vector3(bottomGroup.transform.position.x, botPos - 0.5f, bottomGroup.transform.position.z);
        }
        else
        {
            botPos = parent.transform.parent.GetComponent<SpriteRenderer>().bounds.center.y - parent.transform.parent.GetComponent<SpriteRenderer>().bounds.extents.y;
            bottomGroup.transform.position = new Vector3(bottomGroup.transform.position.x, botPos - 0.5f, bottomGroup.transform.position.z);
        }

    }
}
