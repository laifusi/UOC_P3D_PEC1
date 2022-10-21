using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    private GameObject carPrefab;

    public void SelectCar(GameObject selectedCarPrefab)
    {
        carPrefab = selectedCarPrefab;
    }

    public GameObject GetCar()
    {
        return carPrefab;
    }
}
