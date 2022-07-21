using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Candle : InteractableObject
{
    [Header("Lights")]
    [SerializeField]
    GameObject primeLight;
    [SerializeField]
    GameObject secondLight;

    [Header("Buttons")]
    public Button lightUp;
    public Button blowOut;
    public Button exitButton;

    Collider2D thisCollider;

    private bool lightOn;
    public bool LightOn
    {
        get { return lightOn; }
        set
        {
            lightOn = value;

            //either light up candle our blow out candle based on value
            if (lightOn) 
            {
                LightCandle();
                return;
            }

            BlowOutCandle();
            
        }
    }

    protected override void Start()
    {
        thisCollider = GetComponent<Collider2D>();
        objCanvas.gameObject.SetActive(false);

        if(TimeMng.instance.weatherState == 2) //if during the night
        {
            LightOn = true;
            return;
        }

        LightOn = false;
    }

    private void Update()
    {
        if(TimeMng.instance.weatherState == 2)
        {
            if(!thisCollider.enabled) thisCollider.enabled = true;
            return;
        }

        if (thisCollider.enabled) thisCollider.enabled = false;
    }

    public override IEnumerator InteractWithObject()
    {
        //move to object if needed
        Task t = new Task(base.InteractWithObject());

        while (t.Running)
            yield return null;

        //pause normal gameplay
        StateMng.instance.CanClick = false;

        // allow blowing out if on, and vice versa
        if (LightOn)
            blowOut.gameObject.SetActive(true);
        else
            lightUp.gameObject.SetActive(true);

        // allow exiting
        exitButton.gameObject.SetActive(true);

    }

    public void LightCandle()
    {
        //turn on lights
        primeLight.SetActive(true);
        secondLight.SetActive(true);
    }

    public void BlowOutCandle()
    {
        //turn on lights
        primeLight.SetActive(false);
        secondLight.SetActive(false);

    }
}
