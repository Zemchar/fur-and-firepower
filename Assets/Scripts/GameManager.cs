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
    public Time ROUNDSTART { get; internal set; }
    public float ROUNDLENGTH { get; internal set; }
}
public class GameManager : NetworkBehaviour
{
    private NetworkVariable<float> _roundTimer = new();
    private GAMEVARS gameVars;
    private ROUNDVARS currentRound;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // Preserve Object
        _roundTimer.Value = currentRound.ROUNDLENGTH; // this timere counts down
        
        /* TODO: Send request to laod scene
        // Spawn Players
        Begin Round once players have control */
    }

    private void LateUpdate()
    {
        
    }

    IEnumerator GlobalCoordinatedTimer()
    {
        
        while (_roundTimer.Value > 0)
        {
            yield return new WaitForSeconds(1);
            _roundTimer.Value--;
        }
    }
}
