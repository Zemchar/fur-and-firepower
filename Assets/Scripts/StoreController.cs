using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreController : MonoBehaviour
{
    private double ViolentAffinity = 0.5;
    private double CharmAffinity = 0.5;
    public GameObject ShopScreenPrefab { get; set; }
    public float health {get; private set;}
    public GlobalVars.StreetState streetState = GlobalVars.StreetState.UnControlled; //TODO: MOVE TO STREET CONTROLLER
    

    public void Start()
    {
        health = 100f;
        if(ViolentAffinity + CharmAffinity != 1)
        {
            ViolentAffinity = 0.5;
            CharmAffinity = 0.5;
            throw new Exception("Violent and Charm Affinity must add up to 1, setting each to 0.5");
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
        health -= damage;
    }
}
