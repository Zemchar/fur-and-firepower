using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class BossPlayerController : NetworkBehaviour
{
    private Rigidbody rb;

    [SerializeField] private CinemachineFreeLook vc;
    private Camera cam;
    [FormerlySerializedAs("speed")] [SerializeField] float speedMultiplier = 12f;
    [SerializeField] float maxSpeed = 5f;
    Vector2 moveInput;
    public GlobalVars.TeamAlignment teamAlignment;
    private List<GameObject> SelectedUnits = new();
    private HenchmenDirector henchmenDirector;
    [SerializeField] private LayerMask henchmenLayer;


    void OnDrawGizmos()
    {
        // Gizmos.color = Color.red; No Longer Needed
        // Gizmos.DrawWireCube(GetComponentInChildren<SphereCollider>().bounds.center, GetComponentInChildren<SphereCollider>().bounds.size);
        // Handles.Label(GetComponentInChildren<SphereCollider>().bounds.max, "Gaurdian Area");
        
    }



    public override void OnNetworkSpawn()
    {

        if(IsOwner)
        {
            cam.GetComponent<AudioListener>().enabled = true;
            vc.m_Priority = 10;
            print(Keyboard.current);
            print(Mouse.current);
        }
        else
        {
            this.GetComponent<PlayerInput>().enabled = false;
            vc.m_Priority = 0;
        }
    }

    private void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        henchmenDirector = GameObject.FindWithTag("HenchmanDirector").GetComponent<HenchmenDirector>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        if (IsOwner)
        {
            rb.velocity = Vector3.ClampMagnitude(dir * (speedMultiplier * Time.deltaTime), maxSpeed);
            if (Mouse.current.leftButton.wasPressedThisFrame) // I DONT KNOW WHY but with networking you cannot assign mouse.current to a variable
            {
                var hit = ClickCastRay();
                if (hit.collider.gameObject.CompareTag("Henchman") &&
                    !SelectedUnits.Contains(hit.collider.gameObject)) // dont select if already selected
                {
                    Debug.Log($"Hit {hit.collider.name}");
                    hit.collider.gameObject.SendMessage("RequestSelect", this.gameObject); // Select Henchmen
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Debug.Log("Right Clicked");
                var hit = ClickCastRay();
                var tempDict = new Dictionary<GameObject, GameObject>();
                foreach (var unit in SelectedUnits)
                {
                    tempDict.Add(unit, hit.collider.gameObject);
                }

                object[] tempArray = new object[2]; //doing this is required because of how send message works
                tempArray[0] = tempDict;
                tempArray[1] = this.gameObject;
                henchmenDirector.SendMessage("RedirectHenchmen", tempArray); // Player ==> Henchmen group parent
                SelectedUnits.Clear();                                       // reset dict so more entities can be selected
            }
        }else
        {
            return;
        }
    }
    

    private RaycastHit ClickCastRay()
    {
        Debug.Log($"MouseDown at {Mouse.current.position.ReadValue()}");
        RaycastHit hit;
        Ray target = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(target,out hit, Mathf.Infinity, henchmenLayer);
        Debug.Log($"Hit {hit.collider.name}");
        return hit;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        print(moveInput);
    }
    public void Select(GameObject unit)
    {
        Debug.Log("Selected " + unit.name);
        SelectedUnits.Add(unit);
        Debug.Log("BreakPoint");
    }
    
    void OnSelect(InputValue value)
    {
        var hit = ClickCastRay();
        if (hit.collider.gameObject.CompareTag("Henchman") && !SelectedUnits.Contains(hit.collider.gameObject)) // dont select if already selected
        {
            Debug.Log($"Hit {hit.collider.name}");
            hit.collider.gameObject.SendMessage("RequestSelect", this.gameObject); // Select Henchmen
        }
    }
}
