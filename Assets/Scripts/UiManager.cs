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

    public void hostGame()
    {
        StartCoroutine(HostGame());
    }
    private IEnumerator HostGame()
    {
        DontDestroyOnLoad(this.gameObject);
        //this shit is awesome how did i not know about async loading
        var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Scenes/SampleScene");
        print("Loading Scene");
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
        print("Scene Loaded");
        NetworkManager.Singleton.StartHost();
        Destroy(this.gameObject);
    }
}
