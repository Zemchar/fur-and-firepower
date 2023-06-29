using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    [SerializeField] private GameObject healthBarTemp;
    private GameObject healthBar;
    private double ViolentAffinity = 0.5;
    private double CharmAffinity = 0.5;
    public GameObject ShopScreenPrefab { get; set; }
    public float health {get; private set;}
    private float totalHealth;
    private Image healthNum;
    public GlobalVars.StreetState streetState = GlobalVars.StreetState.UnControlled; //TODO: MOVE TO STREET CONTROLLER

    //Degradation state
    private enum State { h100, h75, h50, h25, h0, dead }
    private State storeHealth = State.h100;

    public void Start()
    {
        health = 100f;
        totalHealth = 100f;
        healthBar = Instantiate(healthBarTemp, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), this.transform);
        healthBar.transform.localPosition = new Vector3(0.525f, 0.1f, 0);
        healthBar.transform.localRotation = Quaternion.Euler(0, 90, 0);
        healthNum = this.transform.Find("HealthBarCanvas(Clone)/HealthBar/Health").gameObject.GetComponent<Image>();


        if (ViolentAffinity + CharmAffinity != 1)
        {
            ViolentAffinity = 0.5;
            CharmAffinity = 0.5;
            throw new Exception("Violent and Charm Affinity must add up to 1, setting each to 0.5");
        }
    }

    private void Update()
    {
        if(storeHealth == State.h100)
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
        else if (storeHealth == State.dead)
        {
            this.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        }
    }

    public void AttemptTakeover(GameObject instructor)
    {
        if (streetState == GlobalVars.StreetState.Unrest && GameManager._instance.DAYNUM.Value== 2)
        {
            print("Attempting Takeover");
            //TODO: Maybe mutliple takeover sequences?
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
    
    public void InflictDamage(float damage)
    {
        if (health <= 0)
        {
            health = 0;
            return;
        }

        health -= damage;
        healthNum.fillAmount = health / totalHealth;
        if(health == totalHealth)
        {
            storeHealth = State.h100;
        }
        else if(health > totalHealth * 0.75f)
        {
            storeHealth = State.h75;
        }
        else if (health > totalHealth * 0.5f)
        {
            storeHealth = State.h50;
        }
        else if (health > totalHealth * 0.25f)
        {
            storeHealth = State.h25;
        }
        else if (health > 0)
        {
            storeHealth = State.h0;
        }
        else if (health == 0)
        {
            storeHealth = State.dead;
        }
    }
}
