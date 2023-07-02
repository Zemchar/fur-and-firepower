using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    [SerializeField] private GameObject storeUI;
    private GameObject healthBar;
    private Image healthNum;
    private GameObject minigame;
    private Image storeOwner;
    private Image gameBar;
    private GameObject startButton;
    private GameObject complete;
    private GameObject wrong;
    private bool dead = false;
    private bool TakeoverInProgress = false;

    private double ViolentAffinity = 0.5;
    private double CharmAffinity = 0.5;
    public GameObject ShopScreenPrefab { get; set; }
    public float health {get; private set;}
    private float totalHealth;
    public GlobalVars.StreetState streetState = GlobalVars.StreetState.UnControlled; //TODO: MOVE TO STREET CONTROLLER

    //Degradation state
    private enum State { h100, h75, h50, h25, h0, dead }
    private State storeHealth = State.h100;

    public void Start()
    {
        health = 100f;
        totalHealth = 100f;
        GameObject tempStore = Instantiate(storeUI, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), this.transform);
        tempStore.transform.localPosition = new Vector3(0.525f, 0.1f, 0);
        tempStore.transform.localRotation = Quaternion.Euler(0, 90, 0);


        healthBar = this.transform.Find("StoreUI(Clone)/HealthBar").gameObject;
        healthNum = this.transform.Find("StoreUI(Clone)/HealthBar/Health").gameObject.GetComponent<Image>();
        minigame = this.transform.Find("StoreUI(Clone)/Minigame").gameObject;
        storeOwner = this.transform.Find("StoreUI(Clone)/Minigame/StoreOwner").gameObject.GetComponent<Image>();
        gameBar = this.transform.Find("StoreUI(Clone)/Minigame/GameBar/Bar").gameObject.GetComponent<Image>();
        startButton = this.transform.Find("StoreUI(Clone)/StartButton").gameObject;
        complete = this.transform.Find("StoreUI(Clone)/Complete").gameObject;
        wrong = this.transform.Find("StoreUI(Clone)/Wrong").gameObject;

        healthNum.fillAmount = 0;
        gameBar.fillAmount = 0;

        int temp = UnityEngine.Random.Range(0, 2);
        if(temp == 0)
        {
            ViolentAffinity = 1;
            CharmAffinity = 0;
            storeOwner.sprite = Resources.Load<Sprite>("Sprites/violent jerma");
        }
        else
        {
            ViolentAffinity = 0;
            CharmAffinity = 1;
            storeOwner.sprite = Resources.Load<Sprite>("Sprites/charm jerma");
        }


        if (ViolentAffinity + CharmAffinity != 1)
        {
            ViolentAffinity = 0.5;
            CharmAffinity = 0.5;
            throw new Exception("Violent and Charm Affinity must add up to 1, setting each to 0.5");
        }
    }

    private void Update()
    {
        UpdateHealthState();
    }

    private void UpdateHealthState()//currently just changes health based off current health state, can be changed to have animation and effects later
    {
        if (storeHealth == State.h100)
        {
            this.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
        }
        else if (storeHealth == State.h75)
        {
            this.GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else if (storeHealth == State.h50)
        {
            this.GetComponent<Renderer>().material.color = new Color(0.6f, 0.6f, 0.6f);
        }
        else if (storeHealth == State.h25)
        {
            this.GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 0.4f);
        }
        else if (storeHealth == State.h0)
        {
            this.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f);
        }
        else if (storeHealth == State.dead && !dead)
        {
            dead = true;
            this.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
            StopAllCoroutines();
            healthBar.SetActive(false);
            minigame.SetActive(true);
        }
    }

    public void AttemptTakeover(GameObject instructor)
    {
        if ((StreetManagerController.Singleton.streetState == GlobalVars.StreetState.UnControlled)&& !TakeoverInProgress)
        {
            print("Attempting Takeover");
            //TODO: Maybe mutliple takeover sequences?
            TakeoverInProgress = true;
            Instantiate(ShopScreenPrefab, this.transform.position, Quaternion.identity).GetComponent<Canvas>().worldCamera =
                GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            /*TODO:
             * 1. Display Takeover Screen
             * 2. Start Coroutine to spin up takeover timer
             * 3. PLayer has mini game to threaten or charm shopkeep
             * 4. If player wins, takeover is successful, else player is kicked out and shop shuts down and doesnt count towards changing the street state.
             */
        }
    }
    
    public void InflictDamage(float damage)//changes health according to damage and updates health state
    {
        if (health <= 0)
        {
            health = 0;
            return;
        }

        health -= damage * 10; //temporialy multiplied by 10 for testing
        healthNum.fillAmount = health / totalHealth;
        StopCoroutine("ShowHealthBar");
        StartCoroutine("ShowHealthBar");

        if(health == totalHealth)
            storeHealth = State.h100;
        else if(health > totalHealth * 0.75f)
            storeHealth = State.h75;
        else if (health > totalHealth * 0.5f)
            storeHealth = State.h50;
        else if (health > totalHealth * 0.25f)
            storeHealth = State.h25;
        else if (health > 0)
            storeHealth = State.h0;
        else if (health == 0)
            storeHealth = State.dead;


    }

    public void ViolentButton()
    {
        Debug.Log(ViolentAffinity);
        if (ViolentAffinity == 1)
        {
            gameBar.fillAmount += 0.1f;
            Debug.Log("violent");
        }
        else
        {

        }
    }

    public void CharmButton()
    {
        Debug.Log(CharmAffinity);
        if (CharmAffinity == 1)
        {
            gameBar.fillAmount += 0.1f;
            Debug.Log("ChArm");
        }
        else
        {

        }
    }

    public void StartButton()
    {
        
    }

    private IEnumerator ShowHealthBar()
    {
        healthBar.SetActive(true);
        yield return new WaitForSeconds(5);
        healthBar.SetActive(false);
    }
}
