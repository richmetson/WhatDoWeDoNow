using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour {

    public Transform HourHand;
    public Transform MinuteHand;

    public int StartHour;
    public int EndHour;

    public int StartMinute;
    public int EndMinute;

    System.DateTime StartTime;
    System.DateTime EndTime;

    public float GameDuration = 120.0f;

    public delegate void TimeUp();

    public event TimeUp OnTimeUp;

    System.DateTime CurrentTime;

	// Use this for initialization
	void Start () {
        StartTime = new System.DateTime(2015, 1, 23, StartHour, StartMinute, 0);
        if (EndHour < StartHour)
        {
            EndTime = new System.DateTime(2015, 1, 24, EndHour, EndMinute, 0);
        }
        else
        {
            EndTime = new System.DateTime(2015, 1, 23, EndHour, EndMinute, 0);
        }
        UpdateClock(StartTime);

        CurrentTime = StartTime;
	}
	
	// Update is called once per frame
	void Update () {
        float Seconds = Mathf.Round((float)(EndTime - StartTime).TotalSeconds);
        float ScaleFactor = Seconds / GameDuration;

        CurrentTime = CurrentTime.AddSeconds(ScaleFactor * Time.deltaTime);

        if(CurrentTime >= EndTime)
        {
            if(OnTimeUp != null)
            {
                OnTimeUp();
            }
        }

        UpdateClock(CurrentTime);
	}

    void UpdateClock(System.DateTime CurrentTime)
    {
        HourHand.localRotation = Quaternion.Euler(0.0f, 0.0f, GetHourRotation(CurrentTime));
        MinuteHand.localRotation = Quaternion.Euler(0.0f, 0.0f, GetMinuteRotation(CurrentTime));
    }

    float GetHourRotation(System.DateTime Time)
    {
        int Hour = Time.Hour;
        if(Hour > 12)
        {
            Hour -= 12;
        }
        return 360.0f * (-Hour / 12.0f);
    }

    float GetMinuteRotation(System.DateTime Time)
    {
        int Minute = Time.Minute;
        return 360.0f * (-Minute / 60.0f);
    }
}
