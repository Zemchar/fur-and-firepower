using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoadPiece : MonoBehaviour
{
    private TextMeshPro text;
    public int value = -1;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();

        text.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void Update()
    {
        text.text = "" + value;
        if (value == -1 && this.name != "Border-Piece(Clone)")
            Check();

    }

    [InspectorButton("Check")]
    public bool check;

    private void Check()
    {
        foreach(Collider coll in GetComponentsInChildren<Collider>())
        {
            coll.enabled = false;
        }

        int height = 15;
        int smallestVal = -1;

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(this.transform.position.x, height, this.transform.position.z), transform.TransformDirection(Vector3.back), out hit, 40, LayerMask.GetMask("Ground")))
        {
            Debug.DrawRay(new Vector3(this.transform.position.x, height, this.transform.position.z), transform.TransformDirection(Vector3.back) * hit.distance, Color.green, 5);
            if (hit.transform.tag == "Road")
            {
                int temp = hit.collider.GetComponentInParent<RoadPiece>().value;
                if ((smallestVal > temp || smallestVal == -1) && temp != -1)
                    smallestVal = temp;
            }
        }
        //else
            //Debug.Log("Hitting nothing back");

        if (Physics.Raycast(new Vector3(this.transform.position.x, height, this.transform.position.z), transform.TransformDirection(Vector3.left), out hit, 40, LayerMask.GetMask("Ground")))
        {
            Debug.DrawRay(new Vector3(this.transform.position.x, height, this.transform.position.z), transform.TransformDirection(Vector3.left) * hit.distance, Color.red, 5);
            if (hit.transform.tag == "Road")
            {
                int temp = hit.collider.GetComponentInParent<RoadPiece>().value;
                if ((smallestVal > temp || smallestVal == -1) && temp != -1)
                    smallestVal = temp;
            }
        }
        //else
            //Debug.Log("Hitting nothing left");

        if (Physics.Raycast(new Vector3(this.transform.position.x, height, this.transform.position.z), transform.TransformDirection(Vector3.right), out hit, 40, LayerMask.GetMask("Ground")))
        {
            Debug.DrawRay(new Vector3(this.transform.position.x, height, this.transform.position.z), transform.TransformDirection(Vector3.right) * hit.distance, Color.blue, 5);
            if (hit.transform.tag == "Road")
            {
                int temp = hit.collider.GetComponentInParent<RoadPiece>().value;
                if ((smallestVal > temp || smallestVal == -1) && temp != -1)
                    smallestVal = temp;
            }
        }
        //else
            //Debug.Log("Hitting nothing right");

        if (Physics.Raycast(new Vector3(this.transform.position.x, height, this.transform.position.z), transform.TransformDirection(Vector3.forward), out hit, 40, LayerMask.GetMask("Ground")))
        {
            Debug.DrawRay(new Vector3(this.transform.position.x, height, this.transform.position.z), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, 5);
            if (hit.transform.tag == "Road")
            {
                int temp = hit.collider.GetComponentInParent<RoadPiece>().value;
                if ((smallestVal > temp || smallestVal == -1) && temp != -1)
                    smallestVal = temp;
            }
        }
        //else
            //Debug.Log("Hitting nothing forward");
        if(smallestVal != -1)
            value = smallestVal + 1;

        foreach (Collider coll in GetComponentsInChildren<Collider>())
        {
            coll.enabled = true;
        }
    }
}
