using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layering : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private float interactRange = 3f;

    public float dist;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        dist = Vector2.Distance(gameObject.transform.position, ClickManager.instance.player.transform.position);
        if (dist < interactRange)
        {
            Interact();
        }

    }

    public void Interact()
    {
        if (transform.position.y < ClickManager.instance.player.transform.position.y)
            spriteRenderer.sortingLayerName = "Foreground";
     
        else
            spriteRenderer.sortingLayerName = "Background";
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
