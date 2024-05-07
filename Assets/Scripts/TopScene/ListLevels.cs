using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListLevels : MonoBehaviour
{
    public static ListLevels Instance;
    public int levelIndex;

    public void Awake()
    {
        if (Instance != null) 
        {
            DestroyImmediate(gameObject);
        } 
        else 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
