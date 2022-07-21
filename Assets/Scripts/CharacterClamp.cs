using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterClamp : MonoBehaviour
{
    public GameObject interactObj;
    public Camera currentCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 interactPos = this.transform.position;
        interactObj.transform.position = interactPos;
     
    }
}
