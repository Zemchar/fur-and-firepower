using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class BossPlayerController : MonoBehaviour
{
    private Rigidbody rb;
    
    [FormerlySerializedAs("speed")] [SerializeField] float speedMultiplier = 12f;
    [SerializeField] float maxSpeed = 5f;
    Vector2 moveInput;
    private Keyboard kb;
    private Mouse ms;
    public GlobalVars.TeamAlignment teamAlignment;
    private List<GameObject> SelectedUnits = new List<GameObject>();
    [SerializeField] private HenchmenDirector henchmenDirector;
    private void Start()
    {
        kb = Keyboard.current;
        ms = Mouse.current;
        // Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        Debug.Log(kb);
        speedMultiplier *= 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (kb.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (ms.leftButton.wasPressedThisFrame) // TODO: Move these into own function tied to input system if game speed gets slow
        {
            var hit = ClickCastRay();
            if (hit.collider.gameObject.CompareTag("Henchman") && !SelectedUnits.Contains(hit.collider.gameObject)) // dont select if already selected
            {
                Debug.Log($"Hit {hit.collider.name}");
                hit.collider.gameObject.SendMessage("RequestSelect", this.gameObject); // Select Henchmen
            }
        }

        if (ms.rightButton.wasPressedThisFrame)
        {
            Debug.Log("Right Clicked");
            var hit = ClickCastRay();
            var tempDict = new Dictionary<GameObject, GameObject>();
            foreach(var unit in SelectedUnits)
            {
                tempDict.Add(unit, hit.collider.gameObject);
            }

            object[] tempArray = new object[2];
            tempArray[0] = tempDict;
            tempArray[1] = this.gameObject;
            henchmenDirector.SendMessage("RedirectHenchmen", tempArray);
        }
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        rb.velocity = Vector3.ClampMagnitude(move * (speedMultiplier * Time.deltaTime), maxSpeed);
    }

    private RaycastHit ClickCastRay()
    {
        Debug.Log($"MouseDown at {ms.position.ReadValue()}");
        RaycastHit hit;
        Ray target = Camera.main.ScreenPointToRay(ms.position.ReadValue());
        Physics.Raycast(target, out hit);
        Debug.Log($"Hit {hit.collider.name}");
        return hit;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    public void Select(GameObject unit)
    {
        Debug.Log("Selected " + unit.name);
        SelectedUnits.Add(unit);
        Debug.Log("BreakPoint");
    }

    // void OnRelease(InputValue value)
    // {d
    //     Debug.Log($"MouseDown at {ms.position.ReadValue()}");
    //     RaycastHit hit;
    //     Ray target = Camera.main.ScreenPointToRay(ms.position.ReadValue());
    //     Physics.Raycast(target, out hit);
    //     if (hit.collider.gameObject.layer == 4)
    //     {
    //         Dictionary<GameObject, GameObject> tempDict = new Dictionary<GameObject, GameObject>();
    //         foreach (var unit in SelectedUnits)
    //         {
    //             tempDict.Append(new KeyValuePair<GameObject, GameObject>(unit, hit.collider.gameObject));
    //         } 
    //         GameObject.FindObjectOfType<HenchmenDirector>().RedirectHenchmen(tempDict, this.gameObject); // Redirect Henchmen after selecting target
    //     }
    // }
}
