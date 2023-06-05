using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPiece : MonoBehaviour
{
    public string type;

    private void Start()
    {
        foreach (BoxCollider box in this.GetComponentsInChildren<BoxCollider>())
            box.enabled = false;

        //Debug.Log(PieceChecker(x, y));

        foreach (BoxCollider box in this.GetComponentsInChildren<BoxCollider>())
            box.enabled = true;
    }

}
