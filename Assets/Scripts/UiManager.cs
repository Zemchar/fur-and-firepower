using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour //Network behaviour not required as this is a client only script, even though it has the ability to interface with NetworkManager
{
    [SerializeField] private GameObject mainMenu;
    private GameObject _currentMenu;

    private void Awake()
    {
        _currentMenu = mainMenu;
        mainMenu.SetActive(true);
    }

    public void SwitchMenu(GameObject menu)
    {
        if (_currentMenu != null)
        {
            _currentMenu.SetActive(false);
        }else{return;}
        menu.SetActive(true);
        _currentMenu = menu;
    }

    public void CycleStreet(TextMeshProUGUI text)
    {
        switch (StreetManagerController.Singleton.streetState)
        {
            case GlobalVars.StreetState.UnControlled:
                StreetManagerController.Singleton.SetStreetState(GlobalVars.StreetState.Unrest);
                text.text = "Current Street State: Unrest";
                break;
            case GlobalVars.StreetState.Unrest:
                StreetManagerController.Singleton.SetStreetState(GlobalVars.StreetState.Controlled);
                text.text = "Current Street State: Controlled";
                break;
            case GlobalVars.StreetState.Controlled:
                StreetManagerController.Singleton.SetStreetState(GlobalVars.StreetState.UnControlled);
                text.text = "Current Street State: UnControlled";
                break;
            
        } 
    }
    public void hostGame(string mode = "host")
    {
        StartCoroutine(HostGame(mode));
    }
    private IEnumerator HostGame(string mode  = "host")
    {
        if (mode == "JOIN_NOW")
        {
            NetworkManager.Singleton.StartHost();
            yield break;
        }
        if (mode is "host")
        {
            DontDestroyOnLoad(this.gameObject);
            //this shit is awesome how did i not know about async loading
            var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Scenes/SampleScene");
            print("Loading Scene");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            print("Scene Loaded");
            NetworkManager.Singleton.StartHost();
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            //this shit is awesome how did i not know about async loading
            var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Scenes/SampleScene");
            print("Loading Scene");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            print("Scene Loaded");
            NetworkManager.Singleton.StartClient();
            Destroy(this.gameObject);
        }
    }
    
}
