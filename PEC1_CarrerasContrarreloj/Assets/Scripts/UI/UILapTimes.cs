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

    /// <summary>
    /// Method that translates and updates the time to a 00:00:00.00 formatted time Text
    /// </summary>
    /// <param name="type">TypeOfLapText that defines if the UILapTimes is affected by the invoked event</param>
    /// <param name="timeInSeconds">Time in seconds that needs to be written in the Text</param>
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

/// <summary>
/// Enum that defines the types of Time texts that can be updated
/// Invalid won't be used by anyone
/// </summary>
public enum TypeOfLapText
{
    CurrentLap, BestLap, FirstLap, SecondLap, ThirdLap, Invalid
}
