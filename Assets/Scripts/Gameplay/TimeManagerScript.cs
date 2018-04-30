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

    private static TimeManagerScript ManagerScriptInstance;    

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

    private bool correctInstance = false;
    public bool CorrectInstance { get { return correctInstance; } }

    public static void AdvanceTime(int seconds, int minutes = 0, int hours = 0)
    {        
        ManagerScriptInstance.AddSecond(ToSeconds(0, hours, minutes, seconds));
    }

    public static void AdvanceTimeWithRandomVariance(float var, int seconds, int minutes = 0, int hours = 0)
    {
        int zeit = Mathf.Max(1, Mathf.RoundToInt(Random.Range(1 - var, 1 + var) * ToSeconds(0, hours, minutes, seconds)) ) ;
        ManagerScriptInstance.AddSecond(zeit);
    }

    public void GetHour(out int seconds, out int minutes, out int hours)
    {
        seconds = Second;
        minutes = Minute;
        hours = Hour;
    }

    public static WorldDate GetDate()
    {
        return
            new WorldDate(
                ManagerScriptInstance.Second,
                ManagerScriptInstance.Minute,
                ManagerScriptInstance.Hour,
                ManagerScriptInstance.Day
                );
    }    

    public static int ToSeconds(int days, int hours, int minutes, int seconds)
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
        this.correctInstance = true;

        TimeFlowCoroutine = StartCoroutine(CountTime());
        OnMinutePassed += WriteDate;

        if(ManagerScriptInstance == null)
        {
            ManagerScriptInstance = this;
        }
	}

    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            AdvanceTime(24, 0, 0);
        }
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

    public override string ToString()
    {
        return
            GetDate().ToString();
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

[System.Serializable]
public class WorldDate
{
    private int seconds;
    private int minutes;
    private int hours;
    private int days;

    public int Seconds { get { return seconds; } }
    public int Minutes { get { return minutes; } }
    public int Hours { get { return hours; } }
    public int Days { get { return days; } }

    public bool ShowInFoldout;

    public WorldDate(int _seconds, int _minutes, int _hours, int _days)
    {
        seconds = _seconds;
        minutes = _minutes;
        hours = _hours;
        days = _days;

        if(seconds >= 60)
        {
            minutes += seconds / 60;
            seconds %= 60;
        }

        if(minutes >= 60)
        {
            hours += minutes / 60;
            minutes %= 60;
        }

        if(hours >= 24)
        {
            days += hours / 24;
            hours %= 24;
        }
    }

    public int ToSeconds()
    {
        return
            seconds +
            (minutes +
                (hours +
                    days * 24) * 60) * 60;
    }

    public int ToSecondsHoursOnly(bool asNextDay = false)
    {
        return
            seconds +
            (minutes +
                (hours +
                    (asNextDay? 1 : 0 ) * 24) * 60) * 60;
    }

    public override string ToString()
    {
        return
            "Day " + Days + ", " + 
            string.Format("{0:00.#}", Hours) + 
            ":" + string.Format("{0:00.#}", Minutes) + 
            ":" + string.Format("{0:00.#}", Seconds);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        bool result = obj is WorldDate;

        if(result)
        {
            WorldDate other = obj as WorldDate;
            result =
                this.ToSeconds() == other.ToSeconds();
        }

        return result;
    }

    public bool CompareTo(WorldDate otherDate, InequalityTypes inequalityTypeThanOther, bool checkHours, bool otherIsNextDay)
    {
        int thisToSeconds = (checkHours) ? this.ToSecondsHoursOnly(false) : this.ToSeconds();
        int otherToSeconds = (checkHours) ? this.ToSecondsHoursOnly(otherIsNextDay) : this.ToSeconds();

        bool result = false;

        switch(inequalityTypeThanOther)
        {
            case InequalityTypes.Equal:
                result = thisToSeconds == otherToSeconds;
                break;

            case InequalityTypes.Greater:
                result = thisToSeconds > otherToSeconds;
                break;

            case InequalityTypes.GreaterOrEqual:
                result = thisToSeconds >= otherToSeconds;
                break;

            case InequalityTypes.Less:
                result = thisToSeconds < otherToSeconds;
                break;

            case InequalityTypes.LessOrEqual:
                result = thisToSeconds <= otherToSeconds;
                break;
        }

        return result;
    }    
}
