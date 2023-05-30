using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class BossPlayerController : MonoBehaviour
{
    private Rigidbody rb;
    
    [SerializeField] float speed = 30f;
    Vector2 moveInput;
    private Keyboard kb;
    private Mouse ms;
    private void Start()
    {
        kb = Keyboard.current;
        ms = Mouse.current;
        Cursor.lockState = CursorLockMode.Confined;
        rb = GetComponent<Rigidbody>();
        Debug.Log(kb);
    }

    // Update is called once per frame
    void Update()
    {
        if (kb.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        rb.velocity = move * (speed * Time.deltaTime);
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }
}
