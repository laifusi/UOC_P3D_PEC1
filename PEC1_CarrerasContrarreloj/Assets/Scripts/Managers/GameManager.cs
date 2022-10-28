using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    private ChosenCarData carData;     //ChosenCarData object that has the chosen car and ghost prefabs

    /// <summary>
    /// Method called from the Car Buttons to save the chosen car Data
    /// </summary>
    /// <param name="selectedCarData">ChosenCarData related to the selected button</param>
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
