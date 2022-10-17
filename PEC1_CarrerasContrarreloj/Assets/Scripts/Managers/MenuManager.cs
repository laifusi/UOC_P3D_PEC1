using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : PersistentSingleton<MenuManager>
{

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayCircuit(int circuitId)
    {
        SceneManager.LoadScene(circuitId);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
