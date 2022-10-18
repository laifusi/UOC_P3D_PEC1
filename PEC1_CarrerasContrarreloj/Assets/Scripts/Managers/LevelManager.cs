using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform startPosition;

    private void Awake()
    {
        Instantiate(GameManager.Instance.GetCar(), startPosition.position, startPosition.rotation);
    }
}
