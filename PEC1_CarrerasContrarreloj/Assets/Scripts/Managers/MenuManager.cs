using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : PersistentSingleton<MenuManager>
{
    /// <summary>
    /// Method to load the Menu scene
    /// </summary>
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Method to load a Circuit Scene
    /// </summary>
    /// <param name="circuitId">Number of the circuit's scene</param>
    public void PlayCircuit(int circuitId)
    {
        SceneManager.LoadScene(circuitId);
    }

    /// <summary>
    /// Method to close the game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
