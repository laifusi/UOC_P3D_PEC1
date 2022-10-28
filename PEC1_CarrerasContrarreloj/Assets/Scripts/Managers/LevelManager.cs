using System;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Transform startPosition;       // Position to instantiate the player
    [SerializeField] private GameObject endedRaceUIPanel;   // UI Panel with Repetition options
    [SerializeField] private GameObject gameCamera;         // Main game camera

    public static Action OnShowRepetition;          // Event to activate Race Repetition
    public static Action OnSwitchRepetition;        // Event to change the camera used in Repetition
    public static Action OnEndRepetition;           // Event to finish Repetition

    private GameObject car;     // Instantiated playable Car
    private GameObject ghost;   // Instantiated ghost
    private bool mainCamIsActive;   // bool to activate and deactivate the main camera

    /// <summary>
    /// Awake method where we instantiate the chosen car and ghost in the start position
    /// </summary>
    private void Awake()
    {
        base.Awake();
        car = Instantiate(GameManager.Instance.GetCar(), startPosition.position, startPosition.rotation);
        ghost = Instantiate(GameManager.Instance.GetGhost(), startPosition.position, startPosition.rotation);
        ghost.SetActive(false);
    }

    /// <summary>
    /// Method to activate the UI panel of a finished race
    /// </summary>
    public void EndRace()
    {
        endedRaceUIPanel.SetActive(true);
    }

    /// <summary>
    /// Method that activates the repetition of the race
    /// </summary>
    public void StartRepetition()
    {
        gameCamera.SetActive(false);
        mainCamIsActive = false;
        ghost.SetActive(false);
        OnShowRepetition?.Invoke();
    }

    /// <summary>
    /// Method that changes the active camera of the repetition
    /// </summary>
    public void SwitchRepetitionCamera()
    {
        gameCamera.SetActive(!mainCamIsActive);
        mainCamIsActive = !mainCamIsActive;
        OnSwitchRepetition?.Invoke();
    }

    /// <summary>
    /// Method that deactivates the repetition of the race
    /// </summary>
    public void StopRepetition()
    {
        gameCamera.SetActive(true);
        mainCamIsActive = true;
        OnEndRepetition?.Invoke();
    }

    /// <summary>
    /// Method to get the instance of the car
    /// </summary>
    /// <returns>Instance of the playable car</returns>
    public GameObject GetCar()
    {
        return car;
    }

    /// <summary>
    /// Method to get the instance of the ghost
    /// </summary>
    /// <returns>Instance of the ghost</returns>
    public GameObject GetGhost()
    {
        return ghost;
    }
}
