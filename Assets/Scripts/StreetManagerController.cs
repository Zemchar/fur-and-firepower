using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StreetManagerController : MonoBehaviour
{
    public GlobalVars.StreetState streetState { get; private set; } = GlobalVars.StreetState.UnControlled;
    public static StreetManagerController Singleton { get; private set; }
    
    public int totalStores { get; private set; }
    public static event Action<GlobalVars.TeamAlignment, GlobalVars.TeamAlignment> OnTeamsRegistered;
    public static event Action<Dictionary<GlobalVars.TeamAlignment, int>, GlobalVars.TeamAlignment, int> OnStoreCapture;
    internal Dictionary<GlobalVars.TeamAlignment, int> teamCap = new ();

    private void Awake()
    {
        Singleton ??= this;
    }

    private void Start()
    {
        Singleton ??= this;
    }
    public void SetStreetState(GlobalVars.StreetState state)
    {
        streetState = state;
    }
    
    //registration functions
    public void RegisterStore()
    {
        totalStores++;
    }
    public void RegisterTeamPresent(GlobalVars.TeamAlignment newTeam)
    {
        teamCap.Add(newTeam, 0);
        if (teamCap.Count >= 1)
        {
            OnTeamsRegistered?.Invoke(teamCap.Keys.First(), newTeam);
        }
    }
    public void RegisterCapture(GlobalVars.TeamAlignment newTeam)
    {
        teamCap[newTeam]++; 
        OnStoreCapture?.Invoke(teamCap, newTeam, totalStores);
    }

}
