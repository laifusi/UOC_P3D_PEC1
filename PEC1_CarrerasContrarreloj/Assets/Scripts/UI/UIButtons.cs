using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public void Menu()
    {
        MenuManager.Instance.Menu();
    }

    public void PlayCircuit(int circuitId)
    {
        MenuManager.Instance.PlayCircuit(circuitId);
    }

    public void Exit()
    {
        MenuManager.Instance.ExitGame();
    }

    public void SelectCar(ChosenCarData car)
    {
        GameManager.Instance.SelectCar(car);
    }
}
