using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] roadPiece;
    [SerializeField] private int gridSize = 7; //size of the grid
    private int tileWidth = 60; //size of each grid tile

    private void Start()
    {
        GameObject temp;
        gridSize += 2; //just to add a border around the grid
        for (var row = 0; row < gridSize; row++)
        {
            for (var col = 0; col < gridSize; col++)
            {
                int roadNum = Random.Range(0, roadPiece.Length);

                //converts row and col numbers into usable x and y coordinates
                int x = tileWidth * (col - gridSize / 2);
                int y = tileWidth * (row - gridSize / 2);


                if (row == 0 || col == 0 || row == gridSize-1 || col == gridSize-1) //coordinates of the border of the grid
                {
                    if (row == gridSize / 2 && col == 0)
                        temp = Instantiate(roadPiece[2], new Vector3(x, 0, y), Quaternion.Euler(0, 270, 0)); //left-most piece
                    else if (row == gridSize / 2 && col == gridSize - 1)
                        temp = Instantiate(roadPiece[2], new Vector3(x, 0, y), Quaternion.Euler(0, 90, 0)); //right-most piece
                    else if (col == gridSize / 2 && row == 0)
                        temp = Instantiate(roadPiece[2], new Vector3(x, 0, y), Quaternion.Euler(0, 180, 0)); //bottom-most piece
                    else if (col == gridSize / 2 && row == gridSize - 1)
                        temp = Instantiate(roadPiece[2], new Vector3(x, 0, y), Quaternion.Euler(0, 0, 0)); //top-most piece
                    else
                        temp = Instantiate(roadPiece[1], new Vector3(x, 0, y), roadPiece[1].transform.rotation);
                }
                else if(row == gridSize / 2 || col == gridSize / 2) //makes a cross in the grid, filling it with 4-way pieces
                    temp = Instantiate(roadPiece[6], new Vector3(x, 0, y), roadPiece[6].transform.rotation);
                else
                    temp = Instantiate(roadPiece[0], new Vector3(x, 0, y), roadPiece[0].transform.rotation);

                temp.transform.SetParent(this.transform);
            }
        }



    }
}
