using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogueHolder : MonoBehaviour
{
    public Vector3 boxNormalPos;
    public Vector3 boxZoomPos = new Vector3(3.5f, 11, 10); // IMPORTANT: this changes within scene dialogues and turns back on cutscene off


    [Header("Dialogue Positions")]
    public Vector3 normalStandingPos = new Vector3(2f, 12.5f, 0);
    public Vector3 normalSittingPos = new Vector3(2f, 12.5f, 0);
    public Vector3 standingZoomInPos = new Vector3(0, 11f, 10);
    public Vector3 sittingZoomInPos = new Vector3(0f, 9f, 10);
    public Vector3 catPos = new Vector3(2f, 7f, 0);
    public Vector3 birdsPos = new Vector3(2f, 7f, 0);

    // Start is called before the first frame update
    void Start()
    {
        switch (name)
        {
            case "Player":
                boxNormalPos = normalStandingPos;
                break;
            case "SparrowDialogue":
                boxNormalPos = birdsPos;
                break;
            case "Cat":
                boxNormalPos = catPos;
                break;
            case "Friend":
                boxZoomPos = standingZoomInPos;
                break;
            default:
                Debug.LogError(name + " is a character that does not exist. Is the name incorrect?");
                return;
        }    

        // make x positive
        if (boxNormalPos.x < 0)
        {
            boxNormalPos.x = Mathf.Abs(boxNormalPos.x);
        }

    }
}
