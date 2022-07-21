using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SoundList : MonoBehaviour
{
    public static SoundList instance;

    [Header("Inventory")]
    [SerializeField]
    [FMODUnity.EventRef]
    public string collectSound;

    [Header("Plant Related")]
    [SerializeField]
    [FMODUnity.EventRef]
    public string plantSound;

    [SerializeField]
    [FMODUnity.EventRef]
    private string harvestSound;

    [Header("Story related")]
    [SerializeField]
    [FMODUnity.EventRef]
    public string sparrowSound;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Fix this!" + gameObject.name);
        else
            instance = this;
    }

    public void PlaySound(string aSound, GameObject obj)
    {
        RuntimeManager.PlayOneShot(aSound, obj.transform.position);
    }

    public void CollectSound(GameObject obj)
    {
        RuntimeManager.PlayOneShot(collectSound, obj.transform.position);
    }

}
