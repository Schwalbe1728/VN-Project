using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MinutePassed();

public class TimeManagerScript : MonoBehaviour
{
    public event MinutePassed OnMinutePassed;

    [SerializeField]
    private float RealTimeToGameSecond;

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

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(CountTime());
        OnMinutePassed += WriteDate;
	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            AdvanceTime(11, 3);
        }
    }

    private void AddSecond(int quantity = 1)
    {
        Seconds += quantity;

        while(Seconds > 59)
        {
            Seconds -= 60;
            AddMinute();

            if(OnMinutePassed != null)
            {
                OnMinutePassed();
            }
        }
    }

    private void AddMinute(int quantity = 1)
    {
        Minutes += quantity;

        while(Minutes > 59)
        {
            Minutes -= 60;
            AddHour();
        }
    }

    private void AddHour(int quantity = 1)
    {
        Hours += quantity;

        if(Hours > 23)
        {
            Hours -= 24;
            AddDay();
        }
    }

    private void AddDay(int quantity = 1)
    {
        Day += quantity;

        if(Day > 30)
        {
            Day -= 30;
            Day++;
            AddMonth();
        }
    }

    private void AddMonth(int quantity = 1)
    {
        Month += quantity;

        if(Month > 12)
        {
            Month -= 12;
            Month++;
        }
    }
    
    private void WriteDate()
    {
        //Debug.Log(Day + "." + Month + ", godz. " + Hours + ":" + Minutes + ":" + Seconds);
    }

    private IEnumerator CountTime()
    {
        while(true)
        {            
            yield return new WaitForSeconds(RealTimeToGameSecond);
            AddSecond();
        }
    }
}
