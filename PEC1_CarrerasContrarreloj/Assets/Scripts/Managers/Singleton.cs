using UnityEngine;

/// <summary>
/// Parent class to define the Singleton pattern only in the current scene
/// Used for particular Managers that should be unique and accessible in their own scene
/// </summary>
public class Singleton<T> : MonoBehaviour
{
    public static T Instance;

    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = GetComponent<T>();
        }
    }

    private void OnDestroy()
    {
        Instance = default(T);
    }
}
