using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockHandsScript : MonoBehaviour {

    [SerializeField]
    private RectTransform HourHand;

    [SerializeField]
    private RectTransform MinuteHand;

    [SerializeField]
    private TimeManagerScript TimeManager;

    private float hourRotation;
    private float minuteRotation;

    void Awake()
    {
        TimeManager.OnMinutePassed += MinutePassed;

        int startSeconds;
        int startMinutes;
        int startHours;

        TimeManager.GetHour(out startSeconds, out startMinutes, out startHours);

        HoursAdvance(startHours);
        MinutesAdvance(startMinutes);

        UpdateHands();
    }

    private void MinutesAdvance(int adv)
    {
        minuteRotation += adv * 360f / 60;
        hourRotation += adv * 360f / (12 * 60);
    }

    private void HoursAdvance(int adv)
    {
        hourRotation += adv * 360f / 12;
    }

    private void MinutePassed()
    {
        MinutesAdvance(1);
        UpdateHands();
    }

    private void UpdateHands()
    {
        MinuteHand.localRotation = Quaternion.AngleAxis(minuteRotation, Vector3.back);
        HourHand.localRotation = Quaternion.AngleAxis(hourRotation, Vector3.back);
    }
}
