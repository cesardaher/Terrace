using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableFloor : MonoBehaviour
{
    public GameObject player;
    public PlayerControllerRegular playerController;

    void Start() {
        playerController = player.GetComponent<PlayerControllerRegular>();

    }

    private void OnMouseDown() {
        Debug.Log("clicked");
        playerController.isTargetted = true;
    }
}
