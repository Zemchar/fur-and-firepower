using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] roadPiece;
    [SerializeField] private int gridSize = 7; //size of the grid
    private int tileWidth = 60; //size of each grid tile

    private Dictionary<string, GameObject> pieces = new Dictionary<string, GameObject>(); //keeps track of all 17 version of pieces

    private void Start()
    {
        FillDictionary();

        GameObject temp;

        gridSize += 2; //just to add a border around the grid
        for (var row = 0; row < gridSize; row++)
        {
            for (var col = 0; col < gridSize; col++)
            {
                //converts row and col numbers into usable x and y coordinates
                int x = tileWidth * (col - gridSize / 2);
                int y = tileWidth * (row - gridSize / 2);

                if (row == 0 || col == 0 || row == gridSize-1 || col == gridSize-1) //coordinates of the border of the grid
                {
                    if (row == gridSize / 2 && col == 0)
                        temp = Instantiate(pieces["deadend-right"], new Vector3(x, 0, y), pieces["deadend-right"].transform.rotation); //left-most piece
                    else if (row == gridSize / 2 && col == gridSize - 1)
                        temp = Instantiate(pieces["deadend-left"], new Vector3(x, 0, y), pieces["deadend-left"].transform.rotation); //right-most piece
                    else if (col == gridSize / 2 && row == 0)
                        temp = Instantiate(pieces["deadend-up"], new Vector3(x, 0, y), pieces["deadend-up"].transform.rotation); //bottom-most piece
                    else if (col == gridSize / 2 && row == gridSize - 1)
                        temp = Instantiate(pieces["deadend-down"], new Vector3(x, 0, y), pieces["deadend-down"].transform.rotation); //top-most piece
                    else
                        temp = Instantiate(roadPiece[1], new Vector3(x, 0, y), roadPiece[1].transform.rotation);

                    temp.transform.SetParent(this.transform);
                }
                else if(row == gridSize / 2 || col == gridSize / 2) //makes a cross in the grid, filling it with 4-way pieces
                {
                    temp = Instantiate(pieces["cross"], new Vector3(x, 0, y), pieces["cross"].transform.rotation);
                    temp.transform.SetParent(this.transform);
                }
                else //delete this entire chunk later, just filler until everything works properly
                {
                    temp = Instantiate(pieces["empty"], new Vector3(x, 0, y), pieces["empty"].transform.rotation);
                    temp.transform.SetParent(this.transform);
                }
            }
        }



    }


    private void FillDictionary() //fills the dictionary "pieces" with all 17 possible piece types/rotations
    {
        GameObject temp = Instantiate(roadPiece[0]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("empty", temp);
        Destroy(temp);  

        temp = Instantiate(roadPiece[1]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("border", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[2]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("deadend-down", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[2]);
        temp.transform.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("deadend-left", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[2]);
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
        pieces.Add("deadend-up", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[2]);
        temp.transform.rotation = Quaternion.Euler(0, 270, 0);
        pieces.Add("deadend-right", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[3]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("straight-vertical", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[3]);
        temp.transform.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("straight-horizontal", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[4]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("L-down", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[4]);
        temp.transform.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("L-left", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[4]);
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
        pieces.Add("L-up", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[4]);
        temp.transform.rotation = Quaternion.Euler(0, 270, 0);
        pieces.Add("L-right", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[5]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("T-down", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[5]);
        temp.transform.rotation = Quaternion.Euler(0, 90, 0);
        pieces.Add("T-left", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[5]);
        temp.transform.rotation = Quaternion.Euler(0, 180, 0);
        pieces.Add("T-up", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[5]);
        temp.transform.rotation = Quaternion.Euler(0, 270, 0);
        pieces.Add("T-right", temp);
        Destroy(temp);

        temp = Instantiate(roadPiece[6]);
        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
        pieces.Add("cross", temp);
        Destroy(temp);

    }


}
