using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapLine : Singleton<LapLine>
{
    [SerializeField] private int numberOfLaps = 3;

    private int currentLapId = 0;
    private float currentLapStartTime;
    private float currentLapEndTime;
    private float bestTime;

    public static Action<bool, bool> OnNewLap;                      //Event to reach to a new started or finished Lap
    public static Action OnNewBestLap;                              //Event to react to a new Best Time
    public static Action<TypeOfLapText, float> OnCurrentTimeUpdate; //Event to update the Text element for the current lap's time
    public static Action<TypeOfLapText, float> OnFixedTimeUpdate;   //Event to update the Text elements for each lap time

    /// <summary>
    /// Update method to update the time visual indication from the current lap
    /// </summary>
    private void Update()
    {
        if(currentLapId != 0 && currentLapId <= numberOfLaps)
        {
            OnCurrentTimeUpdate?.Invoke(TypeOfLapText.CurrentLap, Time.unscaledTime - currentLapStartTime);
        }
    }

    /// <summary>
    /// Method to control the actions of a finished or started lap
    /// </summary>
    /// <param name="other">Collider that triggered the method</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CarFront"))
        {
            currentLapId++;

            // If we aren't starting Lap 1, we save the End Time of the Lap, update the correct Text with the Lap's time and check if it was the best time yet
            if(currentLapId != 1)
            {
                currentLapEndTime = Time.unscaledTime;

                OnFixedTimeUpdate?.Invoke(GetFinishedLapTypeText(currentLapId), currentLapEndTime - currentLapStartTime);

                // If it was the best time, we save the time and invoke the events that will react to a new Best Lap
                if(bestTime == 0 || currentLapEndTime - currentLapStartTime < bestTime)
                {
                    bestTime = currentLapEndTime - currentLapStartTime;
                    OnNewBestLap?.Invoke();
                    OnFixedTimeUpdate?.Invoke(TypeOfLapText.BestLap, bestTime);
                }
            }

            // We invoke the event for a newly started and/or finished lap
            OnNewLap?.Invoke(currentLapId == 1, currentLapId > numberOfLaps);

            // We save the newly started lap's start time
            currentLapStartTime = Time.unscaledTime;

            //We check if we finished the race
            if(currentLapId > numberOfLaps)
            {
                LevelManager.Instance.EndRace();
                GetComponent<Collider>().enabled = false;
            }
        }
    }

    /// <summary>
    /// Method to define which type of TypeOfLapText should be sent through the OnFixedTimeUpdate event
    /// If we started Lap 2, we send the time of Lap 1
    /// If we started Lap 3, we send the time of Lap 2
    /// If we started Lap 4, we send the time of Lap 3 (and the race ends)
    /// If we started Lap 1, we send an invalid value that no one will react to
    /// </summary>
    /// <param name="currentLapId">Number of the newly started lap</param>
    /// <returns>TypeOfLapText associated to the finished lap</returns>
    private TypeOfLapText GetFinishedLapTypeText(int currentLapId)
    {
        switch(currentLapId)
        {
            case 2:
                return TypeOfLapText.FirstLap;
            case 3:
                return TypeOfLapText.SecondLap;
            case 4:
                return TypeOfLapText.ThirdLap;
        }

        return TypeOfLapText.Invalid;
    }
}
