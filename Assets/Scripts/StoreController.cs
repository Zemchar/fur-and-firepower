using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using System.Linq;

public class StoreController : MonoBehaviour
{
    [SerializeField] double ViolentAffinity = 0.5;
    [SerializeField] double CharmAffinity = 0.5;
    [SerializeField] CinemachineVirtualCamera displayCamera;
    [SerializeField] private Canvas TakeoverScreen;
    private bool TakeoverInProgress = false;
    public GameObject ShopScreenPrefab { get; set; }
    public float health {get; private set;}
    

    public void Start()
    {
        health = 100f;
        if(ViolentAffinity + CharmAffinity != 1)
        {
            ViolentAffinity = 0.5;
            CharmAffinity = 0.5;
            throw new Exception("Violent and Charm Affinity must add up to 1, setting each to 0.5");
        }
        TakeoverScreen.gameObject.SetActive(false);
    }

    public void AttemptTakeover(GameObject instructor)
    {
        if ((StreetManagerController.Singleton.streetState == GlobalVars.StreetState.UnControlled)&& !TakeoverInProgress)
        {
            print("Attempting Takeover");
            //TODO: Maybe mutliple takeover sequences?
            Instantiate(ShopScreenPrefab, this.transform.position, Quaternion.identity).GetComponent<Canvas>().worldCamera =
                GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            StartCoroutine(TakeoverSequence(instructor));
        }
    }
    

    private IEnumerator TakeoverSequence(GameObject instructor)
    {
        displayCamera.m_Priority = 9; // Set to 9 so it doesnt overwrite other cameras
        TakeoverScreen.gameObject.SetActive(true);
        instructor.GetComponent<BossPlayerController>().vc.m_Priority = 8;
        yield return new WaitForSeconds(3); //TODO: Make this not a magic number
        instructor.GetComponent<BossPlayerController>().vc.m_Priority = 10;
        displayCamera.m_Priority = 0;
        TakeoverScreen.gameObject.SetActive(false);
    }

    public void InflictDamage(float damage)
    {
        health -= damage;
    }
}
