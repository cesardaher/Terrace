using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using FMODUnity;

public class TimeMng : MonoBehaviour
{
    public static TimeMng instance;
    [Header("Time State")]
    public static bool newGame;

    public int weatherState = 2;
    // 0 = sunny
    // 1 = rainy;
    // 2 = night;
    private int timeState = 0;
    public int TimeState
    {
        get { return timeState; }
        set
        {
            timeState = value;
            PlantObject.TimeState = value;
            DriedPlant.TimeState = value;
        }
    }

    [Header("Scene Settings")]
    public CanvasGroup overlay;
    public Volume sceneVolume;
    public Light2D globalLight;
    public Light2D[] nightLights;
    public float lerpSpeed;
    public float fadeTime = 3;
    

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
    public GameObject nightRain;

    [Header("FMOD")]
    public StudioEventEmitter sunnySong;
    public StudioEventEmitter rainySong;
    public StudioEventEmitter nightSong;
    

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Fix this!" + gameObject.name);
        else
            instance = this;

        GetLightIntensity();

    }

    // Start is called before the first frame update
    void Start()
    {
        //turn off lights if it's not night
        if(weatherState != 2)
        {
            foreach (Light2D light in nightLights)
            {
                light.gameObject.GetComponent<Light2D>().intensity = 0;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PassTime()
    {
        Debug.Log("passed time");
        StateMng.instance.CanClick = false;

        //turn off door's canvas
        if (InteractableObject.currentObj != null && InteractableObject.currentObj is DoorScript)
            InteractableObject.currentObj.objCanvas.gameObject.SetActive(false);

        //fadeout
        //wait until finished fading out
        Task t = new Task(FadeOut(fadeTime));
        while (t.Running)
            yield return null;

        //change profile
        ChangeProfile();

        //rescan pathfinder
        ClickMng.instance.ReScan();

        //spawn player
        t = new Task(ClickMng.instance.SpawnPlayer());

        //disable tea and friend if on
        if (StoryMng.instance.teaSet.activeSelf) StoryMng.instance.teaSet.SetActive(false);
        if (StoryMng.instance.friend.gameObject.activeSelf) StoryMng.instance.friend.gameObject.SetActive(false);

        while (t.Running)
            yield return null;

        Debug.Log("player spawned");

        // if cat is hanging out, make it disappear the next day
        HideOrShowCat();

        yield return new WaitForSeconds(fadeTime / 2);


        //fadein
        t = new Task(FadeIn(fadeTime));
        while (t.Running)
            yield return null;

        //TODO: ADD CREDITS MAYBE?
        //add specific event story events
        if(!StoryMng.instance.metFriend && InventoryMng.instance.plantPage.Contains(6) && StoryMng.instance.brownChair.Standing)
        {
            StartCoroutine(StoryMng.instance.MeetFriend());
            yield break;
        }

        if (!StoryMng.instance.metCat && StoryMng.instance.valerianGrew)
        {
            StartCoroutine(StoryMng.instance.MeetCat());
            yield break;
        }

        if (!StoryMng.instance.metCatAgain && StoryMng.instance.valerianDried)
        {
            StartCoroutine(StoryMng.instance.MeetCatAgain());
            yield break;
        }

        // end story related events

        //give normal zoom
        t = new Task(ClickMng.instance.NormalZoom());
        while (t.Running)
            yield return null;

        Debug.Log("CAN CLICK");
        //allow player to click again
        StateMng.instance.CanClick = true;

    }

    private void HideOrShowCat()
    {
        //if valerian is dry and cat hasn't been met, show cat
        if(!StoryMng.instance.metCatAgain && StoryMng.instance.valerianDried)
        {
            StoryMng.instance.ShowCat();
            return;
        }

        //hide cat after first interaction
        if (StoryMng.instance.cat.gameObject.activeSelf && !StoryMng.instance.metCatAgain)
        {
            StoryMng.instance.cat.transform.position = StoryMng.instance.cat.initialPos;
            StoryMng.instance.cat.gameObject.SetActive(false);

            return;
        }
        

        // put cat in random position
        if (StoryMng.instance.cat.gameObject.activeSelf)
        StoryMng.instance.cat.SpawnCat(); 
    }

    public IEnumerator FadeIn(float overTime)
    {
        float startTime = Time.time;

        //fade in
        while (Time.time < startTime + overTime)
        {
            globalLight.intensity = Mathf.Lerp(0, 1, (Time.time - startTime) / overTime);
            overlay.alpha = Mathf.Lerp(0, 1, (Time.time - startTime) / overTime);

            if (nightObject.activeSelf) //if it's night, turn on the other lights
            {
                foreach(Light2D light in nightLights)
                {
                    light.intensity = Mathf.Lerp(0, light.GetComponent<DefaultIntensity>().intensity, (Time.time - startTime) / overTime);
                }

            }

            yield return null;
        }

        FlickeringLight.canFlicker = true; //enable flickering by candle
    }

    public IEnumerator FadeOut(float overTime)
    {
        Debug.Log("faded out");

        FlickeringLight.canFlicker = false; //disable flickering by candle

        float startTime = Time.time;

        //fade out
        while (Time.time < startTime + overTime)
        {
            globalLight.intensity = Mathf.Lerp(1, 0, (Time.time - startTime) / overTime);
            overlay.alpha = Mathf.Lerp(1, 0, (Time.time - startTime) / overTime);

            if (nightObject.activeSelf) //if it's night, turn on the other lights
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
        overlay.alpha = 0;

        if (nightObject.activeSelf) //if it's night, turn on the other lights
        {
            foreach (Light2D light in nightLights)
            {
                light.intensity = 0;
            }

        }

    }

    void GetLightIntensity()
    {
        foreach (Light2D light in nightLights)
        {
            light.GetComponent<DefaultIntensity>().intensity = light.GetComponent<Light2D>().intensity;
        }
    }

    public void ChangeProfile()
    {

        if (TimeState == 0)
        {
            nightObject.SetActive(false);
            sunnyObject.SetActive(true);
            //disable night rain after first time
            nightRain.SetActive(false);

            //change color of UI elements
            NightColor.IsDay = true;

            sceneVolume.profile = sunProfile;
            weatherState = 0;

            TimeState = 2;
            return;

        }
        //change profile based on current profile (to the next)
        else if (weatherState == 0 || weatherState == 1) //if it's day, go to night
        {
            sunnyObject.SetActive(false);
            rainObject.SetActive(false);

            //change color of UI elements to day
            NightColor.IsDay = false;

            Debug.Log("day");
            sceneVolume.profile = nightProfile;
            weatherState = 2;

            nightObject.SetActive(true);

        }
        else if (weatherState == 2) //if it's night, go to day
        {
            nightObject.SetActive(false);

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

            //change color of UI elements to day
            NightColor.IsDay = true;

        }

        //add to the time counter
        TimeState++;
    }

    public void SetNight()
    {
        sunnyObject.SetActive(false);
        rainObject.SetActive(false);

        //change color of UI elements to day
        NightColor.IsDay = false;

        Debug.Log("day");
        sceneVolume.profile = nightProfile;
        weatherState = 2;

        nightObject.SetActive(true);
    }

    
} //class
