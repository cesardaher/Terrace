using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLayering : MonoBehaviour
{
    public Canvas myCanvas;

    private float interactRange = 3f;

    private void Start()
    {
        myCanvas = GetComponent<Canvas>();
    }

    void Update()
    {
        if (Vector2.Distance(gameObject.transform.position, ClickMng.instance.player.transform.position) < interactRange)
        {
            Interact();
        }

    }

    public void Interact()
    {
        if (transform.position.y < ClickMng.instance.player.transform.position.y)
        {
            myCanvas.sortingLayerName = "Foreground";           
        }
        else
            myCanvas.sortingLayerName = "Background";

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
