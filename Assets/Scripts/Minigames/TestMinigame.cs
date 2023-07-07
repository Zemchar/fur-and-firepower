using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class TestMinigame : MonoBehaviour
{
    
    public async void Start()
    {
        await StartMinigame().ContinueWith(r =>
        {
            Debug.Log("Done");
        });
    }

    public async Task StartMinigame()
    {
        //Debug.Log("Starting Minigame");
        //Debug.Log("Minigame Started");
        //Debug.Log("Minigame Ended");
    }
}
