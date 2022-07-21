using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    [Header("Time State")]

    public static bool newGame;

    public int weatherState = 1;
    // 0 = sunny
    // 1 = rainy;
    // 2 = evening;
    // 3 = night;
    public int timeState = 0;

    [Header("Scene Settings")]
    public Volume sceneVolume;
    public Light2D globalLight;
    public float lerpSpeed;
    public float fadeTime = 3;
    public Light2D[] nightLights;

    [Header("PP Profiles")]
    public VolumeProfile sunProfile;
    public VolumeProfile eveningProfile;
    public VolumeProfile nightProfile;
    public VolumeProfile rainProfile;

    [Header("Weather Objects")]
    public GameObject sunnyObject;
    public GameObject eveningObject;
    public GameObject nightObject;
    public GameObject rainObject;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Fix this!" + gameObject.name);
        else
            instance = this;

    }


    // Start is called before the first frame update
    void Start()
    {
        GetLightIntensity();

        
        foreach (Light2D light in nightLights)
        {
            light.gameObject.GetComponent<Light2D>().intensity = 0;
        }
       

        if (newGame)
        {
            weatherState = 1;
            StartCoroutine(StartScene());

        } else
        {
            bool loadGame = SceneProperties.instance.LoadScene();

            if(loadGame)
            {
                StartCoroutine(ContinueScene());
            } else
            {
                StartCoroutine(StartScene());

            }
            
        }

        

    }

    public IEnumerator ContinueScene()
    {

        
        SceneProperties.instance.SpawnScene();

        StoryManager.instance.cutscene = true;

        //start scene with no light
        globalLight.intensity = 0;

        StateManager.instance.canClick = false;

        StartCoroutine(FadeIn(fadeTime));

        yield return new WaitForSeconds(fadeTime);

        StoryManager.instance.cutscene = false;
        StateManager.instance.canClick = true;
    }
    
    public IEnumerator StartScene()
    {
        ClickManager.instance.interactedObject = StoryManager.instance.sandwich;
        Inventory.instance.CollectObject();

        StoryManager.instance.cutscene = true;

        //start scene with no light
        globalLight.intensity = 0;

        StateManager.instance.canClick = false;

        StartCoroutine(FadeIn(fadeTime));

        yield return new WaitForSeconds(fadeTime);

        //play first dialogue
        DialogueManager.instance.EnqueueDialogue(DialogueManager.instance.gameObject.GetComponent<DialogueHolder>().storyDialogues[0]);

        while(StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }

        StoryManager.instance.playerBox.transform.localPosition = StoryManager.instance.playerBox.GetComponent<ClampBox>().boxZoomPos;

        StoryManager.instance.cutscene = false;
        StateManager.instance.canClick = true;
    }


    public IEnumerator FadeIn(float overTime)
    {

        float startTime = Time.time;

        //fade in
        while (Time.time < startTime + overTime)
        {
            globalLight.intensity = Mathf.Lerp(0, 1, (Time.time - startTime) / overTime);

            if (weatherState == 2)
            {
                foreach (Light2D light in nightLights)
                {
                    light.intensity = Mathf.Lerp(0, light.GetComponent<DefaultIntensity>().intensity, (Time.time - startTime) / overTime);
                }
            }

            yield return null;
        }

        if (StoryManager.instance.cutscene)
            StoryManager.instance.next = true;

    }

    public IEnumerator FadeOut(float overTime)
    {
        Debug.Log("faded out");

        float startTime = Time.time;

        //fade in
        while (Time.time < startTime + overTime)
        {
            globalLight.intensity = Mathf.Lerp(1, 0, (Time.time - startTime) / overTime);

            if (weatherState == 2)
            {
                foreach (Light2D light in nightLights)
                {
                    light.intensity = 0;
                }
            }
            else if (weatherState == 3)
            {
                foreach (Light2D light in nightLights)
                {
                    light.intensity = Mathf.Lerp(light.GetComponent<DefaultIntensity>().intensity, 0, (Time.time - startTime) / overTime);
                }
            }

            yield return null;
        }

        //make sure it's dark
        globalLight.intensity = 0;

        //set camera to player
        ClickManager.instance.zoomCamera.m_Follow = ClickManager.instance.player.transform;

        yield return new WaitForSeconds(2);

        if (StoryManager.instance.cutscene)
            StoryManager.instance.next = true;

    }

    public IEnumerator PassTime()
    {
        Debug.Log("passed time");
        StateManager.instance.canClick = false;

        //fadeout
        StartCoroutine(FadeOut(fadeTime));

        yield return new WaitForSeconds(fadeTime);

        //spawnplayer
        

        //change profile
        ChangeProfile();
        //rescan pathfinder
        ClickManager.instance.ReScan();

        yield return StartCoroutine(ClickManager.instance.SpawnPlayer());

        Debug.Log("player spawned");

        //fadein
        StartCoroutine(FadeIn(fadeTime));

        yield return new WaitForSeconds(fadeTime);

        //show credits at anytime
        if (StoryManager.instance.plantedCalendula && StoryManager.instance.plantedBasil && StoryManager.instance.metCat && !StoryManager.instance.seenCredits)
        {
            StoryManager.instance.StartCoroutine(StoryManager.instance.ShowCredits());
            StoryManager.instance.seenCredits = true;

        } else if (timeState % 3 == 0 && StoryManager.instance.hasPlanted && !StoryManager.instance.plantGrew)
        {
            //play first dialogue
            DialogueManager.instance.EnqueueDialogue(StoryManager.instance.firstHarvest);

            while (StateManager.instance.inDialogue) { yield return new WaitForSeconds(0.1f); }


            StoryManager.instance.cutscene = false;
            StoryManager.instance.plantGrew = true;
            StateManager.instance.canClick = true;

        //during the day, has dried valerian and hasn't met cat
        } else if(timeState % 3 == 0 && !StoryManager.instance.metCat && StoryManager.instance.hasDriedValerian)
        {

            StartCoroutine(ClickManager.instance.DeZoomCameraMethod());
            StoryManager.instance.StartCoroutine(StoryManager.instance.MeetCat());

        } else if (weatherState == 0 || weatherState == 1 || weatherState == 2) //if it's not night
        {
            //if has seen one of the storylines and don't have the shovel

            if((StoryManager.instance.metCat || StoryManager.instance.metFriend) && !StoryManager.instance.gotShovel)
            {
                StartCoroutine(ClickManager.instance.DeZoomCameraMethod());
                StoryManager.instance.StartCoroutine(StoryManager.instance.MeetNeighbour());

            } else
            {
                StartCoroutine(ClickManager.instance.DeZoomCameraMethod());
                StateManager.instance.canClick = true;
            }


        } 
        else {

            StartCoroutine(ClickManager.instance.DeZoomCameraMethod());
            StateManager.instance.canClick = true;

        }

    }

    public void CallPassTime()
    {
        StartCoroutine(PassTime());
    }

    public void ChangeProfile()
    {
        Debug.Log("changed profile");

        if(timeState == 0)
        {
            rainObject.SetActive(false);
            sunnyObject.SetActive(true);

            sceneVolume.profile = sunProfile;
            weatherState = 0;

            timeState = 3;
            return;

        }
        //change profile based on current profile (to the next)
        else if(weatherState == 0 || weatherState == 1)
        {
            sunnyObject.SetActive(false);
            rainObject.SetActive(false);

            Debug.Log("day");
            sceneVolume.profile = eveningProfile;
            weatherState = 2;

            eveningObject.SetActive(true);

        } else if (weatherState == 2)
        {
            eveningObject.SetActive(false);

            Debug.Log("evening");
            sceneVolume.profile = nightProfile;
            weatherState = 3;

            nightObject.SetActive(true);

        } else if (weatherState == 3)
        {
            nightObject.SetActive(false);
            Debug.Log("night");
            Debug.Log("night");
            //half the time sunny
            if (Random.value < 0.7f)
            {
                sunnyObject.SetActive(true);

                sceneVolume.profile = sunProfile;
                weatherState = 0;
            }
            //half the time rainy
            else
            {
                rainObject.SetActive(true);

                sceneVolume.profile = rainProfile;
                weatherState = 1;
            }            

        }

        //add to the time counter
        timeState++;
    }

    public void ChangeProfileCheat()
    {
        if (weatherState == 0)
        {
            //turn off all other objects
            eveningObject.SetActive(false);
            nightObject.SetActive(false);
            rainObject.SetActive(false);

            sunnyObject.SetActive(true);

            sceneVolume.profile = sunProfile;

        }
        else if(weatherState == 1)
        {
            //turn off all other objects
            eveningObject.SetActive(false);
            nightObject.SetActive(false);
            sunnyObject.SetActive(false);

            rainObject.SetActive(true);

            sceneVolume.profile = rainProfile;

        }
        else if (weatherState == 2)
        {
            rainObject.SetActive(false);
            nightObject.SetActive(false);
            sunnyObject.SetActive(false);

            eveningObject.SetActive(true);

            sceneVolume.profile = eveningProfile;

        }
        else if (weatherState == 3)
        {
            rainObject.SetActive(false);
            eveningObject.SetActive(false);
            sunnyObject.SetActive(false);

            nightObject.SetActive(true);

            sceneVolume.profile = nightProfile;
        }


    }

    void GetLightIntensity()
    {
        foreach(Light2D light in nightLights)
        {
            light.gameObject.GetComponent<DefaultIntensity>().intensity = light.GetComponent<Light2D>().intensity;
        }
    }

}
