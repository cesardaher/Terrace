using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBoundsReader : MonoBehaviour
{
    public SpriteRenderer currentRenderer;

    public float extentsX;
    public float extentsY;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // Update is called once per frame
    void Update()
    {
        if(currentRenderer != null)
        {
            extentsX = currentRenderer.bounds.extents.x;
            extentsY = currentRenderer.bounds.extents.y;

            minX = currentRenderer.bounds.min.x;
            minY = currentRenderer.bounds.min.y;

            maxX = currentRenderer.bounds.max.x;
            maxY = currentRenderer.bounds.max.y;
        }
    }
}
