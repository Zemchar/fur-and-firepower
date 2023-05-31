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
    private void Start()
    {
        kb = Keyboard.current;
        ms = Mouse.current;
        Cursor.lockState = CursorLockMode.Locked;
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
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        rb.velocity = Vector3.ClampMagnitude(move * (speedMultiplier * Time.deltaTime), maxSpeed);
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnSelect(InputValue value)
    {
        Debug.Log($"MouseDown at {ms.position.ReadValue()}");
        RaycastHit hit;
        Ray target = Camera.main.ScreenPointToRay(ms.position.ReadValue());
        Physics.Raycast(target, out hit);
        if (hit.collider.gameObject.layer == 4)
        {
            Debug.Log($"Hit {hit.collider.name}");
            SelectedUnits.Append(hit.collider.gameObject.GetComponent<HenchmenController>().RequestSelect(this.gameObject));
        }       
    }

    void OnRelease(InputValue value)
    {
        Debug.Log($"MouseDown at {ms.position.ReadValue()}");
        RaycastHit hit;
        Ray target = Camera.main.ScreenPointToRay(ms.position.ReadValue());
        Physics.Raycast(target, out hit);
        if (hit.collider.gameObject.layer == 4)
        {
            Dictionary<GameObject, GameObject> tempDict = new Dictionary<GameObject, GameObject>();
            foreach (var unit in SelectedUnits)
            {
                tempDict.Append(new KeyValuePair<GameObject, GameObject>(unit, hit.collider.gameObject));
            } 
            GameObject.FindObjectOfType<HenchmenDirector>().RedirectHenchmen(tempDict, this.gameObject);
        }
    }
}
