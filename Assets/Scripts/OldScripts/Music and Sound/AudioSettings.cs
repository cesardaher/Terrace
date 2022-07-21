using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class AudioSettings : MonoBehaviour
{
    FMOD.Studio.Bus Master;

    float masterVolume = 0.5f;
    public float currentVolume;

    void Awake()
    {
        Master = RuntimeManager.GetBus("bus:/");
        masterVolume = 1;
    }

    void Update()
    {
        
        Master.setVolume(masterVolume);

    }

    //change volume on logarithmic scale
    public void MasterVolumeLevel(float newMasterVolume)
    {
        //masterVolume = Mathf.Log10(newMasterVolume) * 20;
        masterVolume = newMasterVolume;
    }

}
