using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    public static T Instance;

    void Start()
    {
        Debug.Log("HELLOO");
        if (Instance != null)
        {
            Debug.Log("NOT UNIQUE");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("UNIQUE");
            Instance = GetComponent<T>();
        }
    }
}
