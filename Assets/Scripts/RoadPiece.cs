using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoadPiece : MonoBehaviour
{
    private TextMeshPro text;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();

        text.transform.rotation = Quaternion.Euler(90, 0, 0);
        text.text = this.name;
    }
}
