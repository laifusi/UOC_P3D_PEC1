using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapLine : MonoBehaviour
{
    [SerializeField] private int numberOfLaps = 3;

    private int currentLapId = 0;
    private float currentLapStartTime;
    private float currentLapEndTime;
    private float bestTime;

    public static event Action<bool> OnNewLap;
    public static event Action OnNewBestLap;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CarFront"))
        {
            currentLapId++;
            if(currentLapId != 1)
            {
                currentLapEndTime = Time.unscaledTime;

                if(bestTime == 0 || currentLapEndTime - currentLapStartTime < bestTime)
                {
                    Debug.Log("New Best Time!");
                    bestTime = currentLapEndTime - currentLapStartTime;
                    OnNewBestLap?.Invoke();
                }
            }

            Debug.Log("Lap Started!");
            OnNewLap?.Invoke(currentLapId == 1);

            currentLapStartTime = Time.unscaledTime;
            if(currentLapId > numberOfLaps)
            {
                Debug.Log("Race Finished!");
            }
        }
    }
}
