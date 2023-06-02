using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] roadPiece;
    private int gridSize = 7; //size of the grid
    private int tileWidth = 60; //size of each grid tile
    private string[,] grid;

    private void Start()
    {
        grid = new string[gridSize, gridSize];
        for (var row = 0; row < gridSize; row++)
        {
            for (var col = 0; col < gridSize; col++)
            {
                //int roadNum = Random.Range(0, roadPiece.Length);
                grid[row,col] = "0";
            }
        }

        string tempS = "\n";
        for (int i = 0; i < grid.GetLength(1); i++)
        {
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                tempS += grid[i, j];
                tempS += " ";
            }
            tempS += "\n";
        }
        Debug.Log(tempS);

        //makes rows and columns of the road pieces
        for (var row = -gridSize * tileWidth; row <= gridSize * tileWidth; row += tileWidth)
        {
            for (var col = -gridSize * tileWidth; col <= gridSize * tileWidth; col += tileWidth)
            {
                //int roadNum = Random.Range(0, roadPiece.Length);
                //GameObject temp = Instantiate(roadPiece[roadNum], new Vector3(row, 0, col), roadPiece[roadNum].transform.rotation);
                //temp.transform.SetParent(this.transform);
            }
        }
    }
}
