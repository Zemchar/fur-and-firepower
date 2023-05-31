using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject roadPiece;
    private int gridSize = 7; //size of the grid
    private int tileWidth = 60; //size of each grid tile

    private void Start()
    {
        //makes rows and columns of the road pieces
        for (var row = -gridSize * tileWidth; row <= gridSize * tileWidth; row += tileWidth)
        {
            for (var col = -gridSize * tileWidth; col <= gridSize * tileWidth; col += tileWidth)
            {
                GameObject temp = Instantiate(roadPiece, new Vector3(row, 0, col), new Quaternion(0, 0, 0, 0));
                temp.transform.SetParent(this.transform);
            }
        }
    }
}
