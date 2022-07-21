using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FlickeringLight : MonoBehaviour
{
	public static bool canFlicker;

	public Light2D fireLight;

	float lightInt;
	public float minInt = 3f, maxInt = 5f;

    private void Start()
    {
		fireLight = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
	{
		if(canFlicker)
        {
			lightInt = Random.Range(minInt, maxInt);
			fireLight.intensity = lightInt;
		}
		
	}
}
