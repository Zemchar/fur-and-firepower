using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
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
    private void Start()
    {
        kb = Keyboard.current;
        ms = Mouse.current;
        Cursor.lockState = CursorLockMode.Confined;
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
        // if (rb.velocity.magnitude > 5)
        // {
        //     Debug.Log("Last Frame Time: " + Time.deltaTime + "\nSpeed: " + rb.velocity.magnitude);
        // }
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
