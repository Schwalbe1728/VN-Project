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

    /*
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
    */

    [SerializeField]
    private int Seconds;

    private int Minutes { get { return Seconds / 60; } }
    private int Hours { get { return Minutes / 60; } }
    private int Days { get { return Hours / 24; } }

    public int Second { get { return Seconds % 60; } }
    public int Minute { get { return Minutes % 60; } }
    public int Hour { get { return Hours % 24; } }
    public int Day { get { return Days; } }

    private Coroutine TimeFlowCoroutine;

    public void AdvanceTime(int seconds, int minutes = 0, int hours = 0)
    {        
        AddSecond(ToSeconds(0, hours, minutes, seconds));
    }

    public void GetHour(out int seconds, out int minutes, out int hours)
    {
        seconds = Second;
        minutes = Minute;
        hours = Hour;
    }

    public int ToSeconds(int days, int hours, int minutes, int seconds)
    {
        return
            seconds +
            (minutes +
                (hours +
                    days * 24) * 60) * 60;
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
        if (Input.GetKey(KeyCode.P)) AdvanceTime(24, 0, 0);
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
            if(Second == 0)
            {
                if (OnMinutePassed != null) OnMinutePassed();

                if(Minute == 0)
                {
                    if (OnHourPassed != null) OnHourPassed();

                    if(Hour == 0)
                    {
                        if (OnDayPassed != null) OnDayPassed();
                    }
                }
            }

        }
    }    
    
    private void WriteDate()
    {
        //Debug.Log("Dzień " + Day + ", godz. " + Hour + ":" + Minute + ":" + Second);
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
