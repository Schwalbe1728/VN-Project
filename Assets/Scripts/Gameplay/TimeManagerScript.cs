using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SecondPassed();
public delegate void MinutePassed();
public delegate void HourPassed();
public delegate void DayPassed();

public class TimeManagerScript : MonoBehaviour
{
    public event SecondPassed OnSecondPassed;
    public event MinutePassed OnMinutePassed;
    public event HourPassed OnHourPassed;
    public event DayPassed OnDayPassed;

    [SerializeField]
    private float GameSecondsPerRealTimeSecond;
    private float gameSecsPerRealSecondBackup;

    [SerializeField]
    private int Seconds;

    [SerializeField]
    private int Minutes;

    [SerializeField]
    private int Hours;

    [SerializeField]
    private int Day;

    [SerializeField]
    private int Month;

    private Coroutine TimeFlowCoroutine;

    public void AdvanceTime(int seconds, int minutes = 0, int hours = 0)
    {
        int totalSeconds =
            seconds +
            minutes * 60 +
            hours * 60 * 60;

        AddSecond(totalSeconds);
    }

    public void GetHour(out int seconds, out int minutes, out int hours)
    {
        seconds = Seconds;
        minutes = Minutes;
        hours = Hours;
    }

    public void AccelerateTime(float gameSecsPerRealSec)
    {
        StopCoroutine(TimeFlowCoroutine);

        gameSecsPerRealSecondBackup = GameSecondsPerRealTimeSecond;
        GameSecondsPerRealTimeSecond = gameSecsPerRealSec;

        TimeFlowCoroutine = StartCoroutine(CountTime());
    }

    public void RestoreTimeFlow()
    {
        Debug.Log("Restore");
        StopCoroutine(TimeFlowCoroutine);
        GameSecondsPerRealTimeSecond = gameSecsPerRealSecondBackup;
        TimeFlowCoroutine = StartCoroutine(CountTime());
    }

	// Use this for initialization
	void Start ()
    {
        TimeFlowCoroutine = StartCoroutine(CountTime());
        OnMinutePassed += WriteDate;
	}

    void Update()
    {

    }

    void OnValidate()
    {
        StopAllCoroutines();
        TimeFlowCoroutine = StartCoroutine(CountTime());
    }

    private void AddSecond(int quantity = 1)
    {
        //Seconds += quantity;

        while(quantity > 0)
        {
            quantity--;
            Seconds++;

            if (OnSecondPassed != null) OnSecondPassed();
            if(Seconds > 59)
            {
                Seconds -= 60;
                AddMinute();
            }

        }
    }

    private void AddMinute(int quantity = 1)
    {       
        while(quantity > 0)
        {
            quantity--;
            Minutes++;

            if (OnMinutePassed != null) OnMinutePassed();

            if(Minutes > 59)
            {
                Minutes -= 60;
                AddHour();
            }
        }
    }

    private void AddHour(int quantity = 1)
    {
        while (quantity > 0)
        {
            quantity--;
            Hours++;

            if (OnHourPassed != null) OnHourPassed();

            if (Hours > 23)
            {
                Hours -= 24;
                AddDay();
            }
        }
    }

    private void AddDay(int quantity = 1)
    {
        while (quantity > 0)
        {
            quantity--;
            Day++;

            if (OnDayPassed != null) OnDayPassed();

            if (Day > 29)
            {
                Day -= 29;
                AddMonth();
            }
        }
    }

    private void AddMonth(int quantity = 1)
    {
        Month += quantity;

        if(Month > 12)
        {
            Month -= 11;
            //Month++;
        }
    }
    
    private void WriteDate()
    {
        //Debug.Log(Day + "." + Month + ", godz. " + Hours + ":" + Minutes + ":" + Seconds);
    }

    private IEnumerator CountTime()
    {
        float waitTime = 1;
        float secondsAdd = 0;

        while (true)
        {
            waitTime = Mathf.Max(1.0f / 60, 1.0f / GameSecondsPerRealTimeSecond);
            secondsAdd += Mathf.Max(1, GameSecondsPerRealTimeSecond / 60);

            yield return new WaitForSeconds(waitTime);

            for (; secondsAdd >= 1; secondsAdd--)
            {
                AddSecond();
            }
        }
    }
}
