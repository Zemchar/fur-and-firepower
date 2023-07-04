using System;
using System.Collections;
using System.Collections.Generic;using Unity.VisualScripting;
using UnityEngine;

public class StreetManagerController : MonoBehaviour
{
    public GlobalVars.StreetState streetState { get; private set; } = GlobalVars.StreetState.UnControlled;
    public static StreetManagerController Singleton { get; private set; }

    private void Start()
    {
        Singleton ??= this;
    }
    
    public void SetStreetState(GlobalVars.StreetState state)
    {
        streetState = state;
    }
}
