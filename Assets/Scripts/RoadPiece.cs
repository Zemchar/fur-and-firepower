using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoadPiece : MonoBehaviour
{
    [SerializeField] private bool checkPath = false;
    public TextMeshPro stats;
    public string type;

    private void Start()
    {
        foreach (BoxCollider box in this.GetComponentsInChildren<BoxCollider>())
            box.enabled = false;

        //Debug.Log(PieceChecker(x, y));

        foreach (BoxCollider box in this.GetComponentsInChildren<BoxCollider>())
            box.enabled = true;

        if (stats != null)
        {
            stats.enabled = true;
            stats.text = type;
            stats.transform.rotation = Quaternion.Euler(90, 0, 0);
        }

    }



}
