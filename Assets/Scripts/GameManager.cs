using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GAMEVARS
{
    public int MAXROUNDS { get; internal set; }
    public int MAXPLAYERS { get; internal set; }
    public int REALPLAYERS { get; internal set; } //human players
    public int CPUPLAYERS { get; internal set; }
}
public class ROUNDVARS
{
    public int ROUNDNUMBER { get; internal set; }
    public int DAYNUMBER { get; internal set; }
    public int PLAYERCOUNT { get; internal set; } // Used to handle client disconnects/player game-overs
    public TimeSpan ROUNDSTART { get; internal set; }
    public float ROUNDLENGTH { get; internal set; }
}
public class GameManager : NetworkBehaviour
{
    public static GameManager Singleton { get; private set; } // singleton
    public NetworkVariable<float> _roundTimer = new NetworkVariable<float>(-1f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> DAYNUM;
    public GAMEVARS gameVars;
    public ROUNDVARS currentRound;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // Preserve Object. Debating removing this.
        if (Singleton == null) // only one game manager
        {
            Singleton = this; 
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator GlobalCoordinatedTimer()
    {
        while (_roundTimer.Value > 0)
        {
            yield return new WaitForSeconds(1);
            _roundTimer.Value--; // this should automatically push to clients
        }
    }
    
    public void StartRound()
    {
        StartCoroutine(GlobalCoordinatedTimer());
        
    }
    private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (NetworkManager.ConnectedClientsIds.Count < gameVars.MAXPLAYERS)
        {
            response.Approved = true;
        }
        else
        {
            response.Approved = false;
            response.Reason = "Server Full.";
        }
    }
}
