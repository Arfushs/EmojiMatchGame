using System;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    public static GameMusic Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }
    
    
}
