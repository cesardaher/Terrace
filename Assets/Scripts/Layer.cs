using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public List<SpriteRenderer> spriteRenderer;
    public static GameObject player;

    private float interactRange = 3f;

    public float dist;

    private void Start()
    {
        spriteRenderer.Add(GetComponent<SpriteRenderer>());
        player = GameObject.Find("Player");

        //if cat, add all elements
        if(tag == "Cat")
        {
            foreach (Transform child in transform)
            {
                if(child.GetComponent<SpriteRenderer>() != null)
                {
                    spriteRenderer.Add(child.GetComponent<SpriteRenderer>());
                }
            }
        }
    }

    void Update()
    {
        dist = Vector2.Distance(gameObject.transform.position, player.transform.position);
        if (dist < interactRange)
        {
            Interact();
        }

    }

    public void Interact()
    {
        if (transform.position.y < player.transform.position.y)
        {
            foreach(SpriteRenderer renderer in spriteRenderer)
            {
                renderer.sortingLayerName = "Foreground";
            }

            return;
        }
           
        foreach (SpriteRenderer renderer in spriteRenderer)
        {
            renderer.sortingLayerName = "Background";
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}

