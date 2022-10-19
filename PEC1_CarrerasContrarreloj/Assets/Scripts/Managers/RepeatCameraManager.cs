using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RepeatCameraManager : MonoBehaviour
{
    [SerializeField] private float secondsBetweenChanges = 3;

    private List<CinemachineVirtualCamera> allCameras;
    private List<CinemachineVirtualCamera> possibleCameras;
    private int currentCamera;
    private float timeSinceLastChange;

    private void Update()
    {
        if(timeSinceLastChange >= secondsBetweenChanges)
        {
            ChangeCamera();
            timeSinceLastChange = 0;
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
}
