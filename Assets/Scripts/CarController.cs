using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float groundDistance = 1.5f;
    [SerializeField] private float defualtSpeed; //how fast the car moves if no movement buttons are pressed
    [SerializeField] private float inputWeight; //how much the speed will change according to the input
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal") * inputWeight, 0, Input.GetAxis("Vertical") * inputWeight + defualtSpeed);
        rb.MovePosition(transform.position + direction * Time.deltaTime);


        GroundCheck();
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        // Checks for the ground below the car and moves the car to the appropriate distance above the ground
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.green);
            //Debug.Log("Hitting the ground");
            transform.position = new Vector3(transform.position.x, hit.transform.position.y + groundDistance, transform.position.z);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.Log("WHERE THE FUCK IS THE GROUND");
        }
    }
}
