using System;
using UnityEngine;
using Cinemachine;

public class VCamera : MonoBehaviour
{
    public static Action<CinemachineVirtualCamera> OnAddedCamera;       // Event to tell the RepetitionCameraManager we exist
    public static Action<CinemachineVirtualCamera> OnRemovedCamera;     // Event to tell the RepetitionCameraManager we were destroyed

    private CinemachineVirtualCamera cvc;

    private void Awake()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        // When the CinemachineVirtualCamera is activated in the scene, we notify the listeners
        OnAddedCamera?.Invoke(cvc);
        gameObject.SetActive(false);

        // We set the Follow and LookAt of the Virtual Camera
        cvc.m_Follow = LevelManager.Instance.GetCar().transform;
        cvc.m_LookAt = LevelManager.Instance.GetCar().transform;
    }

    private void OnDestroy()
    {
        // If the Virtual Camera is destroyed, we notify the listeners
        OnRemovedCamera?.Invoke(cvc);
    }
}
