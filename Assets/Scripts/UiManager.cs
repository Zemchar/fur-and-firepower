using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : NetworkBehaviour
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

    public void hostGame(string mode = "host")
    {
        StartCoroutine(HostGame(mode));
    }
    private IEnumerator HostGame(string mode  = "host")
    {
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
