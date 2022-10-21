using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamera : MonoBehaviour
{
    public static Action<CinemachineVirtualCamera> OnAddedCamera;
    public static Action<CinemachineVirtualCamera> OnRemovedCamera;

    private CinemachineVirtualCamera cvc;

    private void Awake()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        OnAddedCamera?.Invoke(cvc);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        OnRemovedCamera?.Invoke(cvc);
    }
}
