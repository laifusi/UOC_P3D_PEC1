using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private GameObject endedRaceUIPanel;
    [SerializeField] private GameObject gameCamera;

    public static Action OnShowRepetition;
    public static Action OnEndRepetition;

    private void Awake()
    {
        //Instantiate(GameManager.Instance.GetCar(), startPosition.position, startPosition.rotation);
    }

    public void EndRace()
    {
        endedRaceUIPanel.SetActive(true);
    }

    public void StartRepetition()
    {
        gameCamera.SetActive(false);
        OnShowRepetition?.Invoke();
    }

    public void StopRepetition()
    {
        gameCamera.SetActive(true);
        OnEndRepetition?.Invoke();
    }
}
