using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject roadPiece;
    private int gridSize = 7;
    private int gridWidth = 60;

    private void Start()
    {
        for (var row = -gridSize * gridWidth; row <= gridSize * gridWidth; row += gridWidth)
        {
            for (var col = -gridSize * gridWidth; col <= gridSize * gridWidth; col += gridWidth)
            {
                Instantiate(roadPiece, new Vector3(row, 0, col), new Quaternion(0, 0, 0, 0));
            }
        }
    }
}
