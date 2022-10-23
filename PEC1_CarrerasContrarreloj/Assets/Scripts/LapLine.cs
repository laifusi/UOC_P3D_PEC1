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

    public static Action<bool, bool> OnNewLap;
    public static Action OnNewBestLap;
    public static Action<TypeOfLapText, float> OnCurrentTimeUpdate;
    public static Action<TypeOfLapText, float> OnFixedTimeUpdate;

    private void Update()
    {
        if(currentLapId != 0 && currentLapId <= numberOfLaps)
        {
            OnCurrentTimeUpdate?.Invoke(TypeOfLapText.CurrentLap, Time.unscaledTime - currentLapStartTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CarFront"))
        {
            currentLapId++;
            if(currentLapId != 1)
            {
                currentLapEndTime = Time.unscaledTime;

                OnFixedTimeUpdate?.Invoke(GetFinishedLapTypeText(currentLapId), currentLapEndTime - currentLapStartTime);

                if(bestTime == 0 || currentLapEndTime - currentLapStartTime < bestTime)
                {
                    Debug.Log("New Best Time!");
                    bestTime = currentLapEndTime - currentLapStartTime;
                    OnNewBestLap?.Invoke();
                    OnFixedTimeUpdate?.Invoke(TypeOfLapText.BestLap, bestTime);
                }
            }

            Debug.Log("Lap Started!");
            OnNewLap?.Invoke(currentLapId == 1, currentLapId > numberOfLaps);

            currentLapStartTime = Time.unscaledTime;
            if(currentLapId > numberOfLaps)
            {
                Debug.Log("Race Finished!");
                LevelManager.Instance.EndRace();
                GetComponent<Collider>().enabled = false;
            }
        }
    }

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
