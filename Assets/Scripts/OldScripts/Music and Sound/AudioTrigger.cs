using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public AudioSource audioSource;

    public bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isPlaying == false)
		{
            isPlaying = true;
            audioSource.Play();

		} else if (Input.GetKeyDown(KeyCode.Space) && isPlaying == true)
        {
            isPlaying = false;
            audioSource.Stop();
        }
    }
}
