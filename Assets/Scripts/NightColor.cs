using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NightColor : MonoBehaviour
{
    private static bool isDay;
    public static bool IsDay { 
        get { return isDay; }
        set { 
            // change variable value
            isDay = value;

            // change color based on day/night state
            foreach(NightColor UIgraphic in graphicList)
            {

                if(isDay)
                {
                    UIgraphic.graphic.color = UIgraphic.dayColor;
                } else
                {
                    UIgraphic.graphic.color = UIgraphic.nightColor;
                }

            }
        }
    }
    public static List<NightColor> graphicList = new List<NightColor>();

    Color dayColor;
    Color dayPanelColor = Color.white;
    Color dayTextColor = new Color(0.46f, 0.36f, 0.27f, 1);

    Color nightColor; //blueish hue
    Color nightPanelColor = new Color(0.82f, 0.82f, 1, 1);
    Color nightTextColor = new Color(0.38f, 0.29f, 0.27f, 1);
    Color nightObjectUIColor = new Color(0.45f, 0.45f, 1, 1);
    Color nightTextUIColor = new Color(0.6f, 0.6f, 1, 1);

    [SerializeField] bool objectUI;

    public MaskableGraphic graphic;

    private void Awake()
    {
        graphicList.Add(this);
        graphic = GetComponent<MaskableGraphic>();

        nightColor = graphic is TextMeshProUGUI ? nightTextColor : nightPanelColor;
        dayColor = graphic is TextMeshProUGUI ? dayTextColor : dayPanelColor;

        if(graphic is TextMeshProUGUI) //if text, use text colors
        {
            if (objectUI) // if object is from object UI
            {
                dayColor = Color.white;
                nightColor = nightTextUIColor;
                return;
            }

            dayColor = dayTextColor;
            nightColor = nightTextColor;

        } else
        {
            if (objectUI) // if object is from object UI, just use white
            {
                dayColor = Color.white;
                nightColor = nightObjectUIColor;
                return;
            }

            dayColor = dayPanelColor;
            nightColor = nightPanelColor;

        }
    }

    private void Start()
    {
        if (TimeMng.instance.weatherState == 0 || TimeMng.instance.weatherState == 1)
        {

            graphic.color = dayColor;
        }
        else if (TimeMng.instance.weatherState == 2)
        {

            graphic.color = nightColor;
        }

    }

    private void OnDestroy()
    {
        graphicList.Remove(this);   
    }
}
