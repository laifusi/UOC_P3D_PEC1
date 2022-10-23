using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILapTimes : MonoBehaviour
{
    [SerializeField] TypeOfLapText type;

    private TMP_Text text;
    private int hours;
    private int minutes;
    private float seconds;
    private string time;

    private void Start()
    {
        if(type == TypeOfLapText.CurrentLap)
        {
            LapLine.OnCurrentTimeUpdate += UpdateTime;
        }
        else
        {
            LapLine.OnFixedTimeUpdate += UpdateTime;
        }

        text = GetComponent<TMP_Text>();
        text.enabled = false;
    }

    private void UpdateTime(TypeOfLapText type, float timeInSeconds)
    {
        if(this.type == type)
        {
            hours = (int)timeInSeconds / 3600;
            minutes = (int)(timeInSeconds - hours*3600) / 60;
            seconds = timeInSeconds - hours * 3600 - minutes * 60;
            time = (int)hours > 9 ? hours.ToString() : '0' + hours.ToString();
            time += ':';
            time += (int)minutes > 9 ? minutes.ToString() : '0' + minutes.ToString();
            time += ':';
            time += (int)seconds > 9 ? seconds.ToString("F2") : '0' + seconds.ToString("F2");
            text.SetText(time);
            text.enabled = true;
        }
    }

    private void OnDestroy()
    {
        if (type == TypeOfLapText.CurrentLap)
        {
            LapLine.OnCurrentTimeUpdate -= UpdateTime;
        }
        else
        {
            LapLine.OnFixedTimeUpdate -= UpdateTime;
        }
    }
}

public enum TypeOfLapText
{
    CurrentLap, BestLap, FirstLap, SecondLap, ThirdLap, Invalid
}
