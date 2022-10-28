using UnityEngine;

/// <summary>
/// Scriptable Object to save the prefabs connected to the different car options
/// </summary>
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
