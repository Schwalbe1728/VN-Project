using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycleFilter : MonoBehaviour
{
    [SerializeField]
    private int[] HourMarkers;

    [SerializeField]
    private Color[] ColorAtMarkers;

    private TimeManagerScript TimeManager;
    private Image Panel;

    void Awake()
    {
        Panel = gameObject.GetComponent<Image>();

        TimeManager = GameObject.Find("Time Manager").GetComponent<TimeManagerScript>();
        TimeManager.OnMinutePassed += TimeManager_OnMinutePassed;

        TimeManager_OnMinutePassed();
    }

    private void TimeManager_OnMinutePassed()
    {
        int sec;
        int min;
        int hr;

        TimeManager.GetHour(out sec, out min, out hr);

        Color PanelColor = BlendColor(GetSeconds(sec, min, hr));

        Panel.color = PanelColor;
    }

    private Color BlendColor(int seconds)
    {
        int startIndex = ColorAtMarkers.Length - 1;
        int finishIndex = 0;

        for(int i = 0; i < HourMarkers.Length - 1; i++)
        {
            int startSeconds = GetSeconds(0, 0, HourMarkers[i]);
            int finishSeconds = GetSeconds(0, 0, HourMarkers[i + 1]);

            if (seconds == startSeconds) return ColorAtMarkers[i];
            if (seconds == finishSeconds) return ColorAtMarkers[i + 1];

            //Debug.Log("seconds: " + seconds + ", start: " + startSeconds + ", finish: " + finishSeconds);

            if (seconds > startSeconds && seconds < finishSeconds)
            {                
                startIndex = i;
                finishIndex = i + 1;
                break;
            }
        }

        int timeDistanceMax = 
            Mathf.Abs(
                GetSeconds(0, 0, HourMarkers[finishIndex]) - 
                GetSeconds(0, 0, HourMarkers[startIndex])
                );

        if (seconds - GetSeconds(0, 0, HourMarkers[startIndex]) < 0)
        {
            seconds += GetSeconds(0, 0, 24);
        }

        seconds = seconds - GetSeconds(0, 0, HourMarkers[startIndex]);
        

        return BlendColor(ColorAtMarkers[startIndex], ColorAtMarkers[finishIndex], ((float)seconds)/timeDistanceMax );
    }

    private Color BlendColor(Color Start, Color Finish, float progress)
    {
        float r = Start.r + (Finish.r - Start.r) * progress;
        float g = Start.g + (Finish.g - Start.g) * progress;
        float b = Start.b + (Finish.b - Start.b) * progress;
        float a = Start.a + (Finish.a - Start.a) * progress;

        return new Color(r, g, b, a);
    }

    private int GetSeconds(int seconds, int min, int hour)
    {
        return seconds + (min + hour * 60) * 60;
    }
}
