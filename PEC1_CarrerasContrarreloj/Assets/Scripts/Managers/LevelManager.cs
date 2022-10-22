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

    private GameObject car;
    private GameObject ghost;

    private void Awake()
    {
        base.Awake();
        car = Instantiate(GameManager.Instance.GetCar(), startPosition.position, startPosition.rotation);
        ghost = Instantiate(GameManager.Instance.GetGhost(), startPosition.position, startPosition.rotation);
        ghost.SetActive(false);
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

    public GameObject GetCar()
    {
        return car;
    }

    public GameObject GetGhost()
    {
        return ghost;
    }
}
