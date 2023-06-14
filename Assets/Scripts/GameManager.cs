using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
    private static GameManager _instance;
    public NetworkVariable<float> _roundTimer = new NetworkVariable<float>(-1f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public GAMEVARS gameVars;
    public ROUNDVARS currentRound;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // Preserve Object
        if (_instance == null) // only one game manager
        {
            _instance = this; 
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void LateUpdate()
    {
        
    }
    /// <summary>
    /// Creates a new game when requested
    /// </summary>
    /// <param name="vars"></param>
    // public void NewGame(GAMEVARS vars)
    // {
    //     gameVars = vars;
    //     currentRound = new ROUNDVARS();
    //     currentRound.ROUNDNUMBER = 0;
    //     currentRound.DAYNUMBER = 0;
    //     currentRound.PLAYERCOUNT = 0;
    //     currentRound.ROUNDSTART = DateTime.Now.TimeOfDay;
    //     currentRound.ROUNDLENGTH = 60f;
    //     _roundTimer.Value = currentRound.Value.ROUNDLENGTH;
    //     StartCoroutine(GlobalCoordinatedTimer());
    // }

    IEnumerator GlobalCoordinatedTimer()
    {
        
        while (_roundTimer.Value > 0)
        {
            yield return new WaitForSeconds(1);
            _roundTimer.Value--; // this should automatically push to clients
        }
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
