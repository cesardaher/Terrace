using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubLayering : MonoBehaviour
{
    public SpriteRenderer parentRenderer;
    public SpriteRenderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        myRenderer.sortingLayerName = parentRenderer.sortingLayerName;
    }
}
