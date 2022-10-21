using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RepeatCameraManager : MonoBehaviour
{
    [SerializeField] GameObject repetitionCam;
    [SerializeField] private float minTimeBetweenChanges = 2;
    [SerializeField] private float maxTimeBetweenChanges = 6;

    private List<CinemachineVirtualCamera> allCameras = new List<CinemachineVirtualCamera>();
    private List<CinemachineVirtualCamera> possibleCameras = new List<CinemachineVirtualCamera>();
    private int currentCamera;
    private float timeSinceLastChange;
    private float secondsBetweenChanges = 3;
    private bool shouldAct;

    private void Awake()
    {
        VCamera.OnAddedCamera += AddedCamera;
        VCamera.OnRemovedCamera += RemovedCamera;
    }

    private void AddedCamera(CinemachineVirtualCamera cvc)
    {
        allCameras.Add(cvc);
        possibleCameras.Add(cvc);
    }

    private void RemovedCamera(CinemachineVirtualCamera cvc)
    {
        allCameras.Remove(cvc);
        possibleCameras.Remove(cvc);
    }

    private void Update()
    {
        if (!shouldAct)
            return;

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
    /// We deactivate the previous one and activate the new one
    /// We add the previous camera to the possible cameras for the next change
    /// We find the new camera's position in the complete list of cameras
    /// We remove the new camera from the possible cameras for the next change
    /// </summary>
    private void ChangeCamera()
    {
        int randomId = Random.Range(0, possibleCameras.Count);
        allCameras[currentCamera].gameObject.SetActive(false);
        possibleCameras[randomId].gameObject.SetActive(true);
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

    [ContextMenu("StartRepetition")]
    public void StartRepetition()
    {
        shouldAct = true;

        if (allCameras.Count > 0)
        {
            ChangeCamera();
        }

        repetitionCam.SetActive(true);
        secondsBetweenChanges = Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
    }

    [ContextMenu("StopRepetition")]
    public void StopRepetition()
    {
        shouldAct = false;

        repetitionCam.SetActive(false);
        timeSinceLastChange = 0;
    }

    private void OnDestroy()
    {
        VCamera.OnAddedCamera -= AddedCamera;
        VCamera.OnRemovedCamera -= RemovedCamera;
    }
}
