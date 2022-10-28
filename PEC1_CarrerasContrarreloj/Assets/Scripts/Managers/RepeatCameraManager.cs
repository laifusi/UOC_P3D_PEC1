using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RepeatCameraManager : MonoBehaviour
{
    [SerializeField] GameObject repetitionCam;
    [SerializeField] private float minTimeBetweenChanges = 2;
    [SerializeField] private float maxTimeBetweenChanges = 6;

    private List<CinemachineVirtualCamera> allCameras = new List<CinemachineVirtualCamera>();       // List of all the Virtual Cameras
    private List<CinemachineVirtualCamera> possibleCameras = new List<CinemachineVirtualCamera>();  // List of the possible cameras to change to
    private int currentCamera;              // Position of the currently active camera in the all cameras list
    private float timeSinceLastChange;      // Seconds since the last change of TV-like camera
    private float secondsBetweenChanges;    // Current randomly calculated time to wait until the next change of camera
    private bool shouldAct;                 // Boolean to define if repetition is active

    private void Awake()
    {
        VCamera.OnAddedCamera += AddedCamera;
        VCamera.OnRemovedCamera += RemovedCamera;
        LevelManager.OnShowRepetition += StartRepetition;
        LevelManager.OnSwitchRepetition += SwitchCameraActive;
        LevelManager.OnEndRepetition += StopRepetition;
    }

    /// <summary>
    /// Method to add all the virtual cameras in the scene
    /// </summary>
    /// <param name="cvc">New CinemachineVirtualCamera detected</param>
    private void AddedCamera(CinemachineVirtualCamera cvc)
    {
        allCameras.Add(cvc);
        possibleCameras.Add(cvc);
    }

    /// <summary>
    /// Method to remove destroyed virtual cameras
    /// </summary>
    /// <param name="cvc">CinemachineVirtualCamera destroyed detected</param>
    private void RemovedCamera(CinemachineVirtualCamera cvc)
    {
        allCameras.Remove(cvc);
        possibleCameras.Remove(cvc);
    }

    /// <summary>
    /// Update method to update the time passed since the last change and activate the change when needed
    /// </summary>
    private void Update()
    {
        if (!shouldAct)
            return;

        // If the random time between changes passed, we change the camera and calculate a new random time for the next change
        if(timeSinceLastChange >= secondsBetweenChanges)
        {
            ChangeCamera();
            timeSinceLastChange = 0;
            secondsBetweenChanges = Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
        }
        else
        {
            timeSinceLastChange += Time.deltaTime;
        }
    }

    /// <summary>
    /// Method to randomly change the active virtual camera
    /// We get a random camera different from the current one
    /// We reduce the previous one and increase the new one
    /// We add the previous camera to the possible cameras for the next change
    /// We find the new camera's position in the complete list of cameras
    /// We remove the new camera from the possible cameras for the next change
    /// </summary>
    private void ChangeCamera()
    {
        int randomId = Random.Range(0, possibleCameras.Count);
        allCameras[currentCamera].m_Priority = 10;
        possibleCameras[randomId].m_Priority = 11;
        possibleCameras.Add(allCameras[currentCamera]);
        for(int i = 0; i < allCameras.Count; i++)
        {
            if(allCameras[i] == possibleCameras[randomId])
            {
                currentCamera = i;
                break;
            }
        }
        possibleCameras.RemoveAt(randomId);
    }

    /// <summary>
    /// Method that reacts to the start of the camera repetition, activating the cameras and randomly setting the first active one
    /// </summary>
    public void StartRepetition()
    {
        repetitionCam.SetActive(true);

        for(int i = 0; i < allCameras.Count; i++)
        {
            allCameras[i].gameObject.SetActive(true);
        }

        shouldAct = true;

        if (allCameras.Count > 0)
        {
            ChangeCamera();
        }

        secondsBetweenChanges = Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
    }

    /// <summary>
    /// Method to activate/deactivate the cameras when the Switch Camera Button is pressed
    /// </summary>
    private void SwitchCameraActive()
    {
        shouldAct = !shouldAct;
        repetitionCam.SetActive(shouldAct);
    }

    /// <summary>
    /// Method to deactivate the repetition and the cameras
    /// </summary>
    public void StopRepetition()
    {
        shouldAct = false;

        for (int i = 0; i < allCameras.Count; i++)
        {
            allCameras[i].gameObject.SetActive(false);
        }

        repetitionCam.SetActive(false);
        timeSinceLastChange = 0;
    }

    private void OnDestroy()
    {
        VCamera.OnAddedCamera -= AddedCamera;
        VCamera.OnRemovedCamera -= RemovedCamera;
        LevelManager.OnShowRepetition -= StartRepetition;
        LevelManager.OnSwitchRepetition -= SwitchCameraActive;
        LevelManager.OnEndRepetition -= StopRepetition;
    }
}
