using UnityEngine;

/// <summary>
/// Parent class to define the Singleton pattern across all scenes
/// Used for global Managers that should be unique and accessible
/// </summary>
public class PersistentSingleton<T> : MonoBehaviour
{
    public static T Instance;

    void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = GetComponent<T>();
        }
    }
}
