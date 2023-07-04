using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPSingleton : MonoBehaviour
{
    public static RPSingleton Access { get; private set; }
    public RESOURCEPOOL rp;
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // Preserve Object. Debating removing this.
        if (Access == null) // only one Resource Pool, as this file can be quite large in theory
        {
            Access = this; 
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
