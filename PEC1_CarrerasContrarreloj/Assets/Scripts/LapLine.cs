using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapLine : MonoBehaviour
{
    [SerializeField] private int numberOfLaps = 3;

    private int currentLapId = 0;
    Lap currentLap;
    List<Lap> laps = new List<Lap>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CarFront"))
        {
            currentLapId++;
            if(currentLapId != 1)
            {
                currentLap.endTime = Time.unscaledTime;
                laps.Add(currentLap);
            }
            Debug.Log("Lap Started!");
            currentLap = new Lap();
            currentLap.lapNumber = currentLapId;
            currentLap.startTime = Time.unscaledTime;
            if(currentLapId > numberOfLaps)
            {
                Debug.Log("Race Finished!");
            }
        }
    }

    [System.Serializable]
    public struct Lap
    {
        public int lapNumber;
        public float startTime;
        public float endTime;
    }
}
