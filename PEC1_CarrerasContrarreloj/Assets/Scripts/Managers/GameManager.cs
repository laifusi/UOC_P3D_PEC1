using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    private GameObject carPrefab;

    public void SelectCar(GameObject selectedCarPrefab)
    {
        carPrefab = selectedCarPrefab;
    }
}
