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

    private int[] HourMarkersInSeconds;

    private TimeManagerScript TimeManager;
    private Image Panel;

    void Awake()
    {
        Panel = gameObject.GetComponent<Image>();

        TimeManager = GameObject.Find("Time Manager").GetComponent<TimeManagerScript>();
        TimeManager.OnMinutePassed += TimeManager_OnMinutePassed;

        TimeManager_OnMinutePassed();

        HourMarkersInSeconds = new int[HourMarkers.Length];

        for(int i = 0; i < HourMarkers.Length; i++)
        {
            HourMarkersInSeconds[i] = TimeManagerScript.ToSeconds(0, HourMarkers[i], 0, 0);
        }
    }

    private void TimeManager_OnMinutePassed()
    {
        int sec;
        int min;
        int hr;

        TimeManager.GetHour(out sec, out min, out hr);

        Color PanelColor = BlendColor( TimeManagerScript.ToSeconds(TimeManager.Day, hr, min, sec), TimeManager.Day );

        Panel.color = PanelColor;
    }

    private Color BlendColor(int currentSeconds, int days)
    {
        int startIndex = ColorAtMarkers.Length - 1;
        int finishIndex = 0;

        int day = days;

        while( currentSeconds > TimeManagerScript.ToSeconds(day, HourMarkers[finishIndex], 0, 0) )
        {
            startIndex++;
            finishIndex++;

            startIndex %= ColorAtMarkers.Length;
            finishIndex %= ColorAtMarkers.Length;

            if (finishIndex == 0) day++;
        }

        int startSecs = TimeManagerScript.ToSeconds(day + ((finishIndex == 0) ? -1 : 0), HourMarkers[startIndex], 0, 0);
        int finishSecs = TimeManagerScript.ToSeconds(day, HourMarkers[finishIndex], 0, 0);

        int delta = finishSecs - startSecs;
        float t = ((float)(currentSeconds - startSecs)) / delta;

        //Debug.Log("Delta: " + delta + ", t = " + t.ToString("n3"));

        return BlendColor(ColorAtMarkers[startIndex], ColorAtMarkers[finishIndex], t );
    }

    private Color BlendColor(Color Start, Color Finish, float progress)
    {
        return Color.Lerp(Start, Finish, progress);
    }

    private int GetSeconds(int seconds, int min, int hour)
    {
        return seconds + (min + hour * 60) * 60;
    }
}
