using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour //Network behaviour not required as this is a client only script, even though it has the ability to interface with NetworkManager
{
    [SerializeField] private GameObject mainMenu;
    private GameObject _currentMenu;
    public static GameObject UI_Singleton { get; private set; }
    [SerializeField] private Slider vsSlider;
    private Image vsSliderFill;
    private Image vsSliderBackground;
    private void Awake()
    {
        StreetManagerController.OnTeamsRegistered += InitVSslider; // only called when both teams are ready.
        StreetManagerController.OnStoreCapture += UpdateStoreSlider;
        // TODO: GAME TIMER: GameManager.OnShouldUpdateTime += UpdateTime;
        UI_Singleton ??= this.gameObject;
        _currentMenu = mainMenu;
        mainMenu.SetActive(true);
        if (vsSlider)
        {
            vsSliderFill = vsSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
            vsSliderBackground = vsSlider.transform.Find("Background").GetComponent<Image>();
        }
    }

    private void Start()
    {

        if (vsSlider)
        {
            vsSliderFill = vsSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
            vsSliderBackground = vsSlider.transform.Find("Background").GetComponent<Image>();
        }

    }
    
    
    //Setup vsSlider
    private void InitVSslider(GlobalVars.TeamAlignment team1, GlobalVars.TeamAlignment team2)
    {
        vsSlider.maxValue = StreetManagerController.Singleton.totalStores;
        vsSlider.minValue = 0;
        vsSlider.value = (int) StreetManagerController.Singleton.totalStores/2;
        // vsSliderFill.color = Utils.GetTeam(team1, RPSingleton.Access.rp.gcv_teamProps).Color;
        // vsSliderBackground.color = Utils.GetTeam(team2, RPSingleton.Access.rp.gcv_teamProps).Color;
        // //TODO: Icons of teams on either side of slider
        //
    }
    

    private void UpdateStoreSlider(Dictionary<GlobalVars.TeamAlignment, int> dictionary, GlobalVars.TeamAlignment capTeam, int totalStores)
    {
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
        // TODO: Move to another file and rename this one to not be the menu ui manager, just the gameplay one 
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
