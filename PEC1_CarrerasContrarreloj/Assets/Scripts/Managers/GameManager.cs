using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    private  ChosenCarData carData;

    public void SelectCar(ChosenCarData selectedCarData)
    {
        carData = selectedCarData;
    }

    public GameObject GetCar()
    {
        return carData.GetCar();
    }

    public GameObject GetGhost()
    {
        return carData.GetGhost();
    }
}
