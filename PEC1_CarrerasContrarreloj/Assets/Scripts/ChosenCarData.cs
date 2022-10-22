using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ChosenCarData : ScriptableObject
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] GameObject ghostPrefab;

    public GameObject GetCar()
    {
        return carPrefab;
    }

    public GameObject GetGhost()
    {
        return ghostPrefab;
    }
}
